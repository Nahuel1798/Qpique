using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QpiqueWeb.Data;
using QpiqueWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace QpiqueWeb.Controllers.Api
{
    // Controla que se use JWT en vez de Cookies
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProductosApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Trae todos los productos con Paginado
        // GET: api/ProductosApi/Filtrados?categoriaId=1&nombre=producto&page=1&pageSize=6
        [AllowAnonymous]
        [HttpGet("Filtrados")]
        public async Task<IActionResult> GetFiltrados(
            [FromQuery] int? categoriaId,
            [FromQuery] string? nombre,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 6)
        {
            var query = _context.Productos
                .Include(p => p.Categoria)
                .Where(p => !p.Estado) // Filtro borrado lógico
                .AsQueryable();

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
                    CategoriaNombre = p.Categoria != null ? p.Categoria.Nombre : "",
                    CategoriaId = p.CategoriaId
                })
                .ToListAsync();

            return Ok(new { total, productos });
        }

        // Trae todos los productos por categoria
        // GET: api/ProductosApi/Categorias
        [AllowAnonymous]
        [HttpGet("Categorias")]
        public async Task<IActionResult> GetCategorias()
        {
            var categorias = await _context.Categorias.ToListAsync();
            return Ok(categorias);
        }

        // GET: api/ProductosApi/Ids?ids=1,2,3
        [HttpGet("Ids")]
        public async Task<ActionResult<IEnumerable<Producto>>> GetProductosPorIds([FromQuery] string ids)
        {
            if (string.IsNullOrWhiteSpace(ids))
                return BadRequest();

            var idList = ids.Split(',').Select(int.Parse).ToList();

            var productos = await _context.Productos
                .Where(p => idList.Contains(p.Id) && !p.Estado) // Filtro borrado lógico
                .ToListAsync();

            return Ok(productos);
        }

        // Todos los Productos por id
        // GET: api/ProductosApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Producto>> GetProductoPorId(int id)
        {
            var producto = await _context.Productos.FirstOrDefaultAsync(p => p.Id == id && !p.Estado); // Filtro lógico

            if (producto == null)
                return NotFound();

            return Ok(producto);
        }

        // Actualizar Productos
        // PUT: api/ProductosApi/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Administrador, Empleado")]
        public async Task<IActionResult> PutProducto(int id, [FromForm] ProductoDto productoDto, IFormFile? nuevaImagen)
        {
            if (id != productoDto.Id)
                return BadRequest("El ID del producto no coincide con el de la URL.");

            var productoExistente = await _context.Productos.FirstOrDefaultAsync(p => p.Id == id && !p.Estado);
            if (productoExistente == null)
                return NotFound();

            // Si hay una nueva imagen, borrar la anterior y guardar la nueva
            if (nuevaImagen != null && nuevaImagen.Length > 0)
            {
                // Borrar imagen anterior si existe y no es la por defecto
                if (!string.IsNullOrEmpty(productoExistente.ImagenUrl) && productoExistente.ImagenUrl != "/img/sinimagen.jpg")
                {
                    var rutaFisica = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", productoExistente.ImagenUrl.TrimStart('/'));
                    if (System.IO.File.Exists(rutaFisica))
                        System.IO.File.Delete(rutaFisica);
                }

                var extension = Path.GetExtension(nuevaImagen.FileName);
                var nombreArchivo = $"{Guid.NewGuid()}{extension}";
                var rutaCarpeta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/productos");

                if (!Directory.Exists(rutaCarpeta))
                    Directory.CreateDirectory(rutaCarpeta);

                var rutaCompleta = Path.Combine(rutaCarpeta, nombreArchivo);
                using (var stream = new FileStream(rutaCompleta, FileMode.Create))
                {
                    await nuevaImagen.CopyToAsync(stream);
                }

                productoExistente.ImagenUrl = $"/img/productos/{nombreArchivo}";
            }

            // Actualizar otros campos
            productoExistente.Nombre = productoDto.Nombre;
            productoExistente.Descripcion = productoDto.Descripcion;
            productoExistente.Precio = productoDto.Precio;
            productoExistente.Stock = productoDto.Stock;
            productoExistente.CategoriaId = productoDto.CategoriaId;

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

        // Borrar Producto
        // DELETE: api/ProductosApi/5
        [Authorize(Roles = "Administrador")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> BorrarProducto(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null || producto.Estado) // Ya eliminado
                return NotFound();

            producto.Estado = true; // Borrado lógico
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Verifica si un producto existe
        private bool ProductoExiste(int id)
        {
            return _context.Productos.Any(e => e.Id == id && !e.Estado); // Filtro lógico
        }


        // DTO para Producto
        public class ProductoDto
        {
            public int Id { get; set; }
            public string Nombre { get; set; } = "";
            public string? Descripcion { get; set; }
            public string ImagenUrl { get; set; } = "/img/sinimagen.jpg";
            public decimal Precio { get; set; }
            public int Stock { get; set; }
            public bool Estado { get; set; } = false;
            public string CategoriaNombre { get; set; } = "";
            public int CategoriaId { get; set; }
        }

    }
}
