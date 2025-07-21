using Microsoft.AspNetCore.Mvc; 
using Microsoft.EntityFrameworkCore;
using QpiqueWeb.Data;
using QpiqueWeb.Models;

namespace QpiqueWeb.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProductosApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("Todos")]
        public async Task<ActionResult<IEnumerable<ProductoDto>>> GetTodos()
        {
            var productos = await _context.Productos
                .Include(p => p.Categoria)
                .Select(p => new ProductoDto
                {
                    Id = p.Id,
                    Nombre = p.Nombre,
                    Descripcion = p.Descripcion,
                    Precio = p.Precio,
                    Stock = p.Stock,
                    CategoriaNombre = p.Categoria.Nombre
                })
                .ToListAsync();

            return Ok(productos);
        }

        [HttpGet("Filtrados")]
        public async Task<IActionResult> GetFiltrados(
            [FromQuery] int? categoriaId,
            [FromQuery] string? nombre,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 6)
        {
            var query = _context.Productos.Include(p => p.Categoria).AsQueryable();

            if (categoriaId.HasValue)
                query = query.Where(p => p.CategoriaId == categoriaId.Value);

            if (!string.IsNullOrWhiteSpace(nombre))
                query = query.Where(p => EF.Functions.Like(p.Nombre.ToLower(), $"%{nombre.ToLower()}%"));

            var total = await query.CountAsync();

            var productos = await query
                .OrderBy(p => p.Nombre)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new ProductoDto
                {
                    Id = p.Id,
                    Nombre = p.Nombre,
                    Descripcion = p.Descripcion,
                    ImagenUrl = string.IsNullOrEmpty(p.ImagenUrl) ? "/img/sinimagen.jpg" : p.ImagenUrl,
                    Precio = p.Precio,
                    Stock = p.Stock,
                    CategoriaNombre = p.Categoria != null ? p.Categoria.Nombre : ""
                })
                .ToListAsync();

            return Ok(new { total, productos });
        }



        // GET: api/ProductosApi/Categorias
        [HttpGet("Categorias")]
        public async Task<IActionResult> GetCategorias()
        {
            var categorias = await _context.Categorias.ToListAsync();
            return Ok(categorias);
        }

        [HttpGet("Ids")]
        public async Task<ActionResult<IEnumerable<Producto>>> GetProductosPorIds([FromQuery] string ids)
        {
            if (string.IsNullOrWhiteSpace(ids))
                return BadRequest();

            var idList = ids.Split(',').Select(int.Parse).ToList();

            var productos = await _context.Productos
                .Where(p => idList.Contains(p.Id))
                .ToListAsync();

            return Ok(productos);
        }


        // POST: api/ProductosApi
        [HttpPost]
        public async Task<IActionResult> CrearProducto([FromBody] Producto producto)
        {
            if (producto == null)
                return BadRequest();

            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProductoPorId), new { id = producto.Id }, producto);
        }

        // GET: api/ProductosApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Producto>> GetProductoPorId(int id)
        {
            var producto = await _context.Productos.FindAsync(id);

            if (producto == null)
                return NotFound();

            return Ok(producto);
        }

        // PUT: api/ProductosApi/5
        [HttpPut("{id}")]
        public async Task<IActionResult> EditarProducto(int id, [FromBody] Producto producto)
        {
            if (id != producto.Id)
                return BadRequest("El ID del producto no coincide con el de la URL.");

            var productoExistente = await _context.Productos.FindAsync(id);

            if (productoExistente == null)
                return NotFound();

            // Actualizar campos (puedes ajustar según qué campos quieras permitir editar)
            productoExistente.Nombre = producto.Nombre;
            productoExistente.Descripcion = producto.Descripcion;
            productoExistente.Precio = producto.Precio;
            productoExistente.Stock = producto.Stock;
            productoExistente.CategoriaId = producto.CategoriaId;
            productoExistente.ImagenUrl = producto.ImagenUrl;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductoExiste(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // DELETE: api/ProductosApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> BorrarProducto(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
                return NotFound();

            _context.Productos.Remove(producto);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductoExiste(int id)
        {
            return _context.Productos.Any(e => e.Id == id);
        }
    }
}
