using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QpiqueWeb.Data;
using QpiqueWeb.Models;
using System.Security.Claims;

namespace QpiqueWeb.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class VentasApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public VentasApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // DTOs para creación y respuesta
        public class VentaCrearDTO
        {
            public int ClienteId { get; set; }
            public List<DetalleDTO> Detalles { get; set; }
        }

        public class DetalleDTO
        {
            public int ProductoId { get; set; }
            public int Cantidad { get; set; }
        }
        
        // PUT : api/VentasApi/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVenta(int id, [FromBody] VentaDto dto)
        {
            var venta = await _context.Ventas
                .Include(v => v.Detalles)
                .ThenInclude(d => d.Producto)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (venta == null) return NotFound();

            venta.Total = 0;

            foreach (var det in venta.Detalles)
            {
                var nuevo = dto.Detalles.FirstOrDefault(d => d.ProductoNombre == det.Producto.Nombre);
                if (nuevo != null)
                {
                    det.Cantidad = nuevo.Cantidad;
                    det.PrecioUnitario = nuevo.PrecioUnitario;
                    venta.Total += nuevo.Cantidad * nuevo.PrecioUnitario;
                }
            }

            await _context.SaveChangesAsync();
            return Ok(new { venta.Total });
        }


        // GET: api/VentasApi?fecha=2025-07-01&producto=pique&cliente=juan
        [HttpGet("Filtradas")]
        public async Task<IActionResult> GetVentasFiltradas(
            [FromQuery] DateTime? fecha,
            [FromQuery] string? producto,
            [FromQuery] string? cliente,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 5)
        {
            var query = _context.Ventas
                .Include(v => v.Cliente)
                .Include(v => v.Usuario)
                .Include(v => v.Detalles).ThenInclude(d => d.Producto)
                .AsQueryable();

            if (fecha.HasValue)
                query = query.Where(v => v.Fecha.Date == fecha.Value.Date);

            if (!string.IsNullOrEmpty(producto))
                query = query.Where(v => v.Detalles.Any(d => d.Producto.Nombre.Contains(producto)));

            if (!string.IsNullOrEmpty(cliente))
                query = query.Where(v => (v.Cliente.Nombre + " " + v.Cliente.Apellido).Contains(cliente));

            var total = await query.CountAsync();

            var ventas = await query
                .OrderByDescending(v => v.Fecha)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var ventasDto = ventas.Select(v => new VentaDto
            {
                Id = v.Id,
                Fecha = v.Fecha,
                ClienteNombre = $"{v.Cliente.Nombre} {v.Cliente.Apellido}",
                Total = v.Total,
                Detalles = v.Detalles.Select(d => new DetalleDto
                {
                    ProductoNombre = d.Producto.Nombre,
                    PrecioUnitario = d.PrecioUnitario,
                    Cantidad = d.Cantidad
                }).ToList()
            }).ToList();

            return Ok(new { total, ventas = ventasDto });
        }


        [HttpPost]
        public async Task<IActionResult> PostVenta([FromBody] VentaCrearDTO dto)
        {
            // Validar cliente
            var cliente = await _context.Clientes.FindAsync(dto.ClienteId);
            if (cliente == null)
                return BadRequest("Cliente no válido.");

            if (dto.Detalles == null || dto.Detalles.Count == 0)
                return BadRequest("Debe agregar al menos un detalle de venta.");

            // Validar productos
            var productoIds = dto.Detalles.Select(d => d.ProductoId).ToList();
            var productos = await _context.Productos
                .Where(p => productoIds.Contains(p.Id))
                .ToListAsync();

            if (productos.Count != productoIds.Count)
                return BadRequest("Uno o más productos no existen.");

            decimal total = 0;
            var detalles = new List<DetalleVenta>();

            foreach (var item in dto.Detalles)
            {
                if (item.Cantidad < 1)
                    return BadRequest("La cantidad debe ser mayor a cero.");

                var producto = productos.First(p => p.Id == item.ProductoId);

                if (item.Cantidad > producto.Stock)
                    return BadRequest($"Stock insuficiente para {producto.Nombre}.");

                var precioUnitario = producto.Precio;
                total += item.Cantidad * precioUnitario;

                detalles.Add(new DetalleVenta
                {
                    ProductoId = item.ProductoId,
                    Cantidad = item.Cantidad,
                    PrecioUnitario = precioUnitario
                });

                // Descontar stock
                producto.Stock -= item.Cantidad;
            }

            var venta = new Venta
            {
                ClienteId = dto.ClienteId,
                Fecha = DateTime.Now,
                Total = total,
                UsuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                Detalles = detalles
            };

            _context.Ventas.Add(venta);
            await _context.SaveChangesAsync();

            // Crear DTO para la respuesta, sin ciclos
            var ventaDto = new VentaDto
            {
                Id = venta.Id,
                ClienteNombre = $"{cliente.Nombre} {cliente.Apellido}",
                Total = venta.Total,
                Fecha = venta.Fecha,
                Detalles = detalles.Select(d =>
                {
                    var prod = productos.First(p => p.Id == d.ProductoId);
                    return new DetalleDto
                    {
                        ProductoNombre = prod.Nombre,
                        Cantidad = d.Cantidad,
                        PrecioUnitario = d.PrecioUnitario
                    };
                }).ToList()
            };

            return CreatedAtAction(nameof(GetVenta), new { id = venta.Id }, ventaDto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<VentaDto>> GetVenta(int id)
        {
            var venta = await _context.Ventas
                .Include(v => v.Cliente)
                .Include(v => v.Detalles)
                    .ThenInclude(d => d.Producto)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (venta == null)
                return NotFound();

            var dto = new VentaDto
            {
                Id = venta.Id,
                ClienteNombre = venta.Cliente.Nombre + " " + venta.Cliente.Apellido,
                Total = venta.Total,
                Fecha = venta.Fecha,
                Detalles = venta.Detalles.Select(d => new DetalleDto
                {
                    ProductoNombre = d.Producto.Nombre,
                    Cantidad = d.Cantidad,
                    PrecioUnitario = d.PrecioUnitario
                }).ToList()
            };

            return Ok(dto);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<VentaDto>>> GetVentas()
        {
            var ventas = await _context.Ventas
                .Include(v => v.Cliente)
                .Include(v => v.Detalles)
                    .ThenInclude(d => d.Producto)
                .OrderByDescending(v => v.Fecha)
                .ToListAsync();

            var ventasDto = ventas.Select(venta => new VentaDto
            {
                Id = venta.Id,
                ClienteNombre = venta.Cliente.Nombre + " " + venta.Cliente.Apellido,
                Total = venta.Total,
                Fecha = venta.Fecha,
                Detalles = venta.Detalles.Select(d => new DetalleDto
                {
                    ProductoNombre = d.Producto.Nombre,
                    Cantidad = d.Cantidad,
                    PrecioUnitario = d.PrecioUnitario
                }).ToList()
            });

            return Ok(ventasDto);
        }
    }
}
