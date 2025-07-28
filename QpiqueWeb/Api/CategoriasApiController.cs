using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QpiqueWeb.Data;
using QpiqueWeb.Models;

namespace QpiqueWeb.Controllers.Api
{
    // Controla que se use JWT en vez de Cookies
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Administrador")]
    public class CategoriasApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context; // Usa Entity para conectarse a la base de datos
        private readonly IWebHostEnvironment _env; // Manejo de imagenes

        public CategoriasApiController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // Trae todas las Categorias
        // GET: api/Categorias
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Categoria>>> GetCategorias()
        {
            return await _context.Categorias.ToListAsync(); //Lista todas las categorias
        }

        // Trae las Categorias por id
        // GET: api/Categorias/id
        [HttpGet("{id}")]
        public async Task<ActionResult<Categoria>> GetCategoria(int id)
        {
            var categoria = await _context.Categorias.FindAsync(id); //Busca por id
            if (categoria == null)
                return NotFound();

            return categoria;
        }

        // Crea la Categoria
        // POST: api/Categorias
        [HttpPost]
        public async Task<ActionResult<Categoria>> PostCategoria([FromForm] Categoria categoria, IFormFile imagen)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Verifica la imagen
            if (imagen != null && imagen.Length > 0)
            {
                if (!imagen.ContentType.StartsWith("image/"))
                    return BadRequest("El archivo debe ser una imagen válida.");

                categoria.ImagenUrl = await GuardarImagen(imagen);
            }

            _context.Categorias.Add(categoria); //Agrega la Categoria
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCategoria), new { id = categoria.Id }, categoria);
        }

        // Modifica las Categorias
        // PUT: api/Categorias/id
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategoria(int id, [FromForm] CategoriaDto categoriaDto)
        {
            if (id != categoriaDto.Id)
            {
                return BadRequest("El ID no coincide");
            }

            var categoria = await _context.Categorias.FindAsync(id); //Busca por id
            if (categoria == null)
            {
                return NotFound();
            }

            categoria.Nombre = categoriaDto.Nombre;

            // Procesar imagen si viene
            if (categoriaDto.Imagen != null)
            {
                // Guardar nueva imagen y asignar ruta (ejemplo)
                var fileName = Guid.NewGuid() + Path.GetExtension(categoriaDto.Imagen.FileName);
                var path = Path.Combine("wwwroot/img/categorias", fileName);
                using (var stream = System.IO.File.Create(path))
                {
                    await categoriaDto.Imagen.CopyToAsync(stream);
                }
                categoria.ImagenUrl = "/img/categorias/" + fileName;
            }

            _context.Entry(categoria).State = EntityState.Modified; //Modifica
            await _context.SaveChangesAsync();

            return Ok(categoria);
        }

        // Metodo para ingresar Imagenes
        private async Task<string> GuardarImagen(IFormFile imagen)
        {
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(imagen.FileName)}";
            var folderPath = Path.Combine(_env.WebRootPath, "img", "categorias");
            Directory.CreateDirectory(folderPath);
            var filePath = Path.Combine(folderPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imagen.CopyToAsync(stream);
            }

            return $"/img/categorias/{fileName}";
        }

        // Borrar categoria
        // DELETE: api/Categorias/id
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategoria(int id)
        {
            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria == null)
                return NotFound();

            if (!string.IsNullOrEmpty(categoria.ImagenUrl))
            {
                var rutaImagen = Path.Combine(_env.WebRootPath, categoria.ImagenUrl.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
                if (System.IO.File.Exists(rutaImagen))
                    System.IO.File.Delete(rutaImagen);
            }

            categoria.Estado = true; // Marcar como eliminada
            _context.Categorias.Update(categoria); //Borrado logico
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // CategoriaDto(Evita exponer mas campos del modelo real)
        public class CategoriaDto
        {
            public int Id { get; set; }
            public string Nombre { get; set; }
            public IFormFile? Imagen { get; set; }
        }
    }
}
