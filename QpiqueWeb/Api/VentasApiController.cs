using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QpiqueWeb.Data;
using QpiqueWeb.Models;
using System.Security.Claims;

namespace QpiqueWeb.Controllers.Api
{
    // Controla que se use JWT en vez de Cookies
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class VentasApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public VentasApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // DTO para creación de venta
        public class VentaCrearDTO
        {
            public int ClienteId { get; set; }
            public List<DetalleCrearDTO> Detalles { get; set; }
        }

        // DTO para detalles de venta
        public class DetalleCrearDTO
        {
            public int ProductoId { get; set; }
            public int Cantidad { get; set; }
        }

        // DTOs para actualización PUT
        public class VentaActualizarDTO
        {
            public List<DetalleActualizarDTO> Detalles { get; set; }
        }

        // DTO para actualizar detalles de venta
        public class DetalleActualizarDTO
        {
            public int ProductoId { get; set; }
            public int Cantidad { get; set; }
            public decimal PrecioUnitario { get; set; }
        }

        // Trae Ventas Con Paginada
        // GET: api/VentasApi/Filtradas
        [HttpGet("Filtradas")]
        public async Task<IActionResult> GetVentasFiltradas(
            [FromQuery] string? cliente,
            [FromQuery] string? producto,
            [FromQuery] DateTime? dia,
            [FromQuery] DateTime? fechaDesde,
            [FromQuery] DateTime? fechaHasta,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 6)
        {
            var query = _context.Ventas
                .Include(v => v.Cliente)
                .Include(v => v.Detalles).ThenInclude(d => d.Producto)
                .AsQueryable();

            if (!string.IsNullOrEmpty(cliente))
                query = query.Where(v => v.Cliente.Nombre.Contains(cliente));

            if (!string.IsNullOrEmpty(producto))
                query = query.Where(v => v.Detalles.Any(d => d.Producto.Nombre.Contains(producto)));

            if (dia.HasValue)
            {
                var inicioDia = dia.Value.Date;
                var finDia = inicioDia.AddDays(1);
                query = query.Where(v => v.Fecha >= inicioDia && v.Fecha < finDia);
            }
            else
            {
                if (fechaDesde.HasValue)
                {
                    var desde = fechaDesde.Value.Date;
                    query = query.Where(v => v.Fecha >= desde);
                }
                if (fechaHasta.HasValue)
                {
                    var hasta = fechaHasta.Value.Date.AddDays(1); // para incluir todo el día final
                    query = query.Where(v => v.Fecha < hasta);
                }
            }

            var total = await query.CountAsync();

            var ventas = await query
                .OrderByDescending(v => v.Fecha)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(v => new
                {
                    v.Id,
                    v.Fecha,
                    ClienteNombre = v.Cliente.Nombre,
                    v.Total,
                    Detalles = v.Detalles.Select(d => new
                    {
                        d.ProductoId,
                        ProductoNombre = d.Producto.Nombre,
                        d.Cantidad,
                        d.PrecioUnitario,
                        ImagenUrl = d.Producto.ImagenUrl
                    })
                })
                .ToListAsync();

            return Ok(new { ventas, total });
        }

        // Trae las ganancias por Dia
        [HttpGet("GananciasPorDia")]
        public IActionResult GetGananciasPorDia()
        {
            var hoy = DateTime.Today;
            var ganancias = _context.Ventas
                .Where(v => v.Fecha.Date == hoy)
                .Sum(v => (decimal?)v.Total) ?? 0;

            return Ok(ganancias);
        }

        // Trae las ganancias por Semana
        [HttpGet("GananciasPorSemana")]
        public IActionResult GetGananciasPorSemana()
        {
            var hoy = DateTime.Today;
            int diferencia = (int)hoy.DayOfWeek == 0 ? 6 : (int)hoy.DayOfWeek - 1; // lunes = 0
            var inicioSemana = hoy.AddDays(-diferencia).Date;
            var finSemana = inicioSemana.AddDays(7);

            var ganancias = _context.Ventas
                .Where(v => v.Fecha >= inicioSemana && v.Fecha < finSemana)
                .Sum(v => (decimal?)v.Total) ?? 0;

            return Ok(ganancias);
        }

        // Trae las ganancias por Mes
        [HttpGet("GananciasPorMes")]
        public IActionResult GetGananciasPorMes()
        {
            var hoy = DateTime.Today;
            var inicioMes = new DateTime(hoy.Year, hoy.Month, 1);
            var finMes = inicioMes.AddMonths(1);

            var ganancias = _context.Ventas
                .Where(v => v.Fecha >= inicioMes && v.Fecha < finMes)
                .Sum(v => (decimal?)v.Total) ?? 0;

            return Ok(ganancias);
        }

        // Trae las ganancias
        [HttpGet("Resumen")]
        public async Task<IActionResult> GetResumenGanancias()
        {
            var hoy = DateTime.Today;
            var inicioMes = new DateTime(hoy.Year, hoy.Month, 1);
            var inicioSemana = hoy.AddDays(-(int)hoy.DayOfWeek + 1); // lunes

            decimal totalHoy = await _context.Ventas.Where(v => v.Fecha.Date == hoy).SumAsync(v => v.Total);
            decimal totalSemana = await _context.Ventas.Where(v => v.Fecha >= inicioSemana && v.Fecha < inicioSemana.AddDays(7)).SumAsync(v => v.Total);
            decimal totalMes = await _context.Ventas.Where(v => v.Fecha >= inicioMes && v.Fecha < inicioMes.AddMonths(1)).SumAsync(v => v.Total);

            return Ok(new { totalHoy, totalSemana, totalMes });
        }

        // Crea las Ventas
        // POST: api/VentasApi
        [HttpPost]
        public async Task<IActionResult> PostVenta([FromBody] VentaCrearDTO dto)
        {
            if (dto.Detalles == null || !dto.Detalles.Any())
                return BadRequest("Debe enviar al menos un producto.");

            var productoIds = dto.Detalles.Select(d => d.ProductoId).ToList();
            var productos = await _context.Productos
                .Where(p => productoIds.Contains(p.Id))
                .ToListAsync();

            if (productos.Count != productoIds.Count)
                return BadRequest("Uno o más productos no existen.");

            // Validar stock
            foreach (var det in dto.Detalles)
            {
                var producto = productos.First(p => p.Id == det.ProductoId);
                if (det.Cantidad < 1)
                    return BadRequest("Cantidad debe ser mayor a cero.");
                if (det.Cantidad > producto.Stock)
                    return BadRequest($"Stock insuficiente para el producto {producto.Nombre}.");
            }

            // Crear venta
            var venta = new Venta
            {
                ClienteId = dto.ClienteId,
                Fecha = DateTime.Now,
                Total = 0,
                UsuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                Detalles = new List<DetalleVenta>()
            };

            foreach (var det in dto.Detalles)
            {
                var producto = productos.First(p => p.Id == det.ProductoId);

                var detalle = new DetalleVenta
                {
                    ProductoId = det.ProductoId,
                    Cantidad = det.Cantidad,
                    PrecioUnitario = producto.Precio
                };

                venta.Detalles.Add(detalle);
                venta.Total += producto.Precio * det.Cantidad;

                producto.Stock -= det.Cantidad;
            }

            _context.Ventas.Add(venta);
            await _context.SaveChangesAsync();

            return Ok(new { venta.Id, venta.Total });
        }

        // Modifica Venta
        // PUT : api/VentasApi/id
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVenta(int id, [FromBody] VentaActualizarDTO dto)
        {
            var venta = await _context.Ventas
                .Include(v => v.Detalles)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (venta == null)
                return NotFound();

            if (dto.Detalles == null || !dto.Detalles.Any())
                return BadRequest("Debe proporcionar detalles de venta.");

            // IDs de productos involucrados (viejos y nuevos)
            var productoIds = dto.Detalles.Select(d => d.ProductoId)
                .Union(venta.Detalles.Select(vd => vd.ProductoId))
                .Distinct()
                .ToList();

            var productos = await _context.Productos
                .Where(p => productoIds.Contains(p.Id))
                .ToListAsync();

            if (productos.Count != productoIds.Count)
                return BadRequest("Uno o más productos no existen.");

            // Revertir stock de la venta original (devolver stock viejo)
            foreach (var detViejo in venta.Detalles)
            {
                var producto = productos.FirstOrDefault(p => p.Id == detViejo.ProductoId);
                if (producto != null)
                {
                    producto.Stock += detViejo.Cantidad;
                }
            }

            // Validar stock con los nuevos valores
            foreach (var detNuevo in dto.Detalles)
            {
                if (detNuevo.Cantidad < 1)
                    return BadRequest("La cantidad debe ser mayor a cero.");

                var producto = productos.First(p => p.Id == detNuevo.ProductoId);
                if (detNuevo.Cantidad > producto.Stock)
                {
                    return BadRequest($"Stock insuficiente para el producto {producto.Nombre}. Disponible: {producto.Stock}");
                }
            }

            // Limpiar detalles anteriores
            venta.Detalles.Clear();
            venta.Total = 0;

            // Agregar nuevos detalles y descontar stock
            foreach (var detNuevo in dto.Detalles)
            {
                var producto = productos.First(p => p.Id == detNuevo.ProductoId);
                producto.Stock -= detNuevo.Cantidad;

                venta.Detalles.Add(new DetalleVenta
                {
                    ProductoId = detNuevo.ProductoId,
                    Cantidad = detNuevo.Cantidad,
                    PrecioUnitario = detNuevo.PrecioUnitario
                });

                venta.Total += detNuevo.Cantidad * detNuevo.PrecioUnitario;
            }

            await _context.SaveChangesAsync();

            return Ok(new { venta.Total });
        }

        // Borrar Ventas
        // DELETE : api/VentasApi/id
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVenta(int id)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            var venta = await _context.Ventas
                .Include(v => v.Detalles)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (venta == null) return NotFound();

            foreach (var detalle in venta.Detalles)
            {
                var producto = await _context.Productos.FindAsync(detalle.ProductoId);
                if (producto != null)
                {
                    producto.Stock += detalle.Cantidad;
                }
            }

            _context.DetallesVenta.RemoveRange(venta.Detalles);
            _context.Ventas.Remove(venta);
            await _context.SaveChangesAsync();

            await transaction.CommitAsync();

            return NoContent();
        }
    }
}
