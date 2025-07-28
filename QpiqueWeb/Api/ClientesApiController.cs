using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QpiqueWeb.Data;
using QpiqueWeb.Models;

namespace QpiqueWeb.Controllers.Api
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ClientesApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/ClientesApi
        [HttpGet]
        public async Task<IActionResult> GetClientes()
        {
            var clientes = await _context.Clientes
                .OrderBy(c => c.Apellido)
                .ThenBy(c => c.Nombre)
                .Select(c => new ClienteDto
                {
                    Id = c.Id,
                    Nombre = c.Nombre,
                    Apellido = c.Apellido,
                    Telefono = c.Telefono,
                    Email = c.Email
                })
                .ToListAsync();

            return Ok(clientes);
        }

        // GET: api/ClientesApi/Paginado?page=1&pageSize=10&search=nombre
        [HttpGet("Paginado")]
        public async Task<IActionResult> GetClientesPaginado(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string search = "")
        {
            var query = _context.Clientes.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var lowered = search.ToLower();
                query = query.Where(c =>
                    c.Nombre.ToLower().Contains(lowered) ||
                    c.Apellido.ToLower().Contains(lowered));
            }

            var total = await query.CountAsync();

            var clientes = await query
                .OrderBy(c => c.Apellido)
                .ThenBy(c => c.Nombre)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(c => new ClienteDto
                {
                    Id = c.Id,
                    Nombre = c.Nombre,
                    Apellido = c.Apellido,
                    Telefono = c.Telefono,
                    Email = c.Email
                })
                .ToListAsync();

            return Ok(new { total, clientes });
        }

        // GET: api/ClientesApi/Buscar?nombre
        [HttpGet("Buscar")]
        public async Task<IActionResult> BuscarClientes([FromQuery] string nombre)
        {
            var filtro = nombre?.Trim().ToLower() ?? "";

            var clientes = await _context.Clientes
                .Where(c => c.Nombre.ToLower().Contains(filtro) || c.Apellido.ToLower().Contains(filtro))
                .OrderBy(c => c.Apellido)
                .ThenBy(c => c.Nombre)
                .Select(c => new ClienteDto
                {
                    Id = c.Id,
                    Nombre = c.Nombre,
                    Apellido = c.Apellido,
                    Telefono = c.Telefono,
                    Email = c.Email
                })
                .ToListAsync();

            return Ok(clientes);
        }

        // GET: api/ClientesApi/id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCliente(int id)
        {
            var cliente = await _context.Clientes
                .Where(c => c.Id == id)
                .Select(c => new ClienteDto
                {
                    Id = c.Id,
                    Nombre = c.Nombre,
                    Apellido = c.Apellido,
                    Telefono = c.Telefono,
                    Email = c.Email
                })
                .FirstOrDefaultAsync();

            if (cliente == null)
                return NotFound();

            return Ok(cliente);
        }

        // POST: api/ClientesApi
        [HttpPost]
        public async Task<IActionResult> PostCliente([FromBody] ClienteDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Nombre) || string.IsNullOrWhiteSpace(dto.Apellido))
                return BadRequest("El nombre y apellido son obligatorios.");

            var cliente = new Cliente
            {
                Nombre = dto.Nombre.Trim(),
                Apellido = dto.Apellido.Trim(),
                Telefono = (dto.Telefono ?? "").Trim(),
                Email = (dto.Email ?? "").Trim()
            };

            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCliente), new { id = cliente.Id }, new
            {
                cliente.Id,
                cliente.Nombre,
                cliente.Apellido,
                cliente.Email
            });
        }

        // PUT: api/ClientesApi/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCliente(int id, [FromBody] ClienteDto dto)
        {
            if (id != dto.Id)
                return BadRequest("El ID del cliente no coincide.");

            if (string.IsNullOrWhiteSpace(dto.Nombre) || string.IsNullOrWhiteSpace(dto.Apellido))
                return BadRequest("El nombre y apellido son obligatorios.");

            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
                return NotFound("Cliente no encontrado.");

            cliente.Nombre = dto.Nombre.Trim();
            cliente.Apellido = dto.Apellido.Trim();
            cliente.Telefono = (dto.Telefono ?? "").Trim();
            cliente.Email = (dto.Email ?? "").Trim();

            try
            {
                await _context.SaveChangesAsync();
                return Ok("Cliente actualizado correctamente.");
            }
            catch
            {
                return StatusCode(500, "Error al actualizar el cliente.");
            }
        }

        // DELETE: api/ClientesApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCliente(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
                return NotFound("Cliente no encontrado.");

            _context.Clientes.Remove(cliente);

            try
            {
                await _context.SaveChangesAsync();
                return Ok("Cliente eliminado correctamente.");
            }
            catch
            {
                return StatusCode(500, "Error al eliminar el cliente.");
            }
        }

        // DTO para Cliente
        public class ClienteDto
        {
            public int Id { get; set; }
            public string Nombre { get; set; }
            public string Apellido { get; set; }
            public string Telefono { get; set; }
            public string Email { get; set; }
        }
    }
}
