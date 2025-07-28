using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QpiqueWeb.Models;

namespace QpiqueWeb.Controllers.Api
{
    // No usa Token usa Cookies
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Administrador")]
    public class RolesApiController : ControllerBase
    {
        private readonly UserManager<Usuario> _userManager;
       
        public RolesApiController(UserManager<Usuario> userManager)
        {
            _userManager = userManager;
        }

        // Trae los Usuarios
        // GET: api/RolesApi?searchString=abc
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsuarioDto>>> GetUsers([FromQuery] string? searchString = null)
        {
            var query = _userManager.Users.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                var lowerSearch = searchString.ToLower();
                query = query.Where(u =>
                    (!string.IsNullOrEmpty(u.Nombre) && u.Nombre.ToLower().Contains(lowerSearch)) ||
                    (!string.IsNullOrEmpty(u.Apellido) && u.Apellido.ToLower().Contains(lowerSearch))
                );
            }

            var usuarios = await query.ToListAsync();

            var resultado = new List<UsuarioDto>();

            foreach (var usuario in usuarios)
            {
                var roles = await _userManager.GetRolesAsync(usuario);
                resultado.Add(new UsuarioDto
                {
                    Id = usuario.Id,
                    Nombre = usuario.Nombre,
                    Apellido = usuario.Apellido,
                    Email = usuario.Email,
                    UserName = usuario.UserName,
                    Avatar = usuario.Avatar,
                    Roles = roles.ToList()
                });
            }

            return Ok(resultado);
        }

        // Trae por Id
        // GET: api/RolesApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UsuarioDto>> GetUser(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest("Id requerido");

            var usuario = await _userManager.FindByIdAsync(id);
            if (usuario == null)
                return NotFound();

            var roles = await _userManager.GetRolesAsync(usuario);

            var dto = new UsuarioDto
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Email = usuario.Email,
                UserName = usuario.UserName,
                Avatar = usuario.Avatar,
                Roles = roles.ToList()
            };

            return Ok(dto);
        }

        // Trae Usuarios Con Paginado
        // GET: api/RolesApi?searchString=abc&page=1&pageSize=10
        [HttpGet("Usuarios")]
        public async Task<IActionResult> GetUsers(
            [FromQuery] string? searchString = null,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var query = _userManager.Users.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                var lowerSearch = searchString.ToLower();
                query = query.Where(u =>
                    (!string.IsNullOrEmpty(u.Nombre) && u.Nombre.ToLower().Contains(lowerSearch)) ||
                    (!string.IsNullOrEmpty(u.Apellido) && u.Apellido.ToLower().Contains(lowerSearch))
                );
            }

            var total = await query.CountAsync();

            var usuarios = await query
                .OrderBy(u => u.Nombre)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var resultado = new List<UsuarioDto>();

            foreach (var usuario in usuarios)
            {
                var roles = await _userManager.GetRolesAsync(usuario);
                resultado.Add(new UsuarioDto
                {
                    Id = usuario.Id,
                    Nombre = usuario.Nombre,
                    Apellido = usuario.Apellido,
                    Email = usuario.Email,
                    UserName = usuario.UserName,
                    Avatar = usuario.Avatar,
                    Roles = roles.ToList()
                });
            }

            return Ok(new { total, usuarios = resultado });
        }

        // Modifica Usarios
        // PUT: api/RolesApi/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] UsuarioUpdateDto dto)
        {
            if (id != dto.Id)
                return BadRequest("El id no coincide");

            var usuario = await _userManager.FindByIdAsync(id);
            if (usuario == null)
                return NotFound();

            usuario.Nombre = dto.Nombre;
            usuario.Apellido = dto.Apellido;
            usuario.Email = dto.Email;
            usuario.UserName = dto.Email;

            var resultUpdate = await _userManager.UpdateAsync(usuario);
            if (!resultUpdate.Succeeded)
                return BadRequest(resultUpdate.Errors);

            var rolesActuales = await _userManager.GetRolesAsync(usuario);
            var rolActual = rolesActuales.FirstOrDefault();

            if (rolActual != dto.NuevoRol)
            {
                if (!string.IsNullOrEmpty(rolActual))
                {
                    var removeResult = await _userManager.RemoveFromRoleAsync(usuario, rolActual);
                    if (!removeResult.Succeeded)
                        return BadRequest("Error removiendo rol anterior");
                }

                var addResult = await _userManager.AddToRoleAsync(usuario, dto.NuevoRol);
                if (!addResult.Succeeded)
                    return BadRequest("Error agregando nuevo rol");
            }

            return NoContent();
        }

        // Borra Usuarios
        // DELETE: api/RolesApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest("Id requerido");

            var usuario = await _userManager.FindByIdAsync(id);
            if (usuario == null)
                return NotFound();

            if (usuario.Id == _userManager.GetUserId(User))
                return BadRequest("No podés eliminar tu propio usuario.");

            var administradores = await _userManager.GetUsersInRoleAsync("Administrador");
            if (administradores.Count == 1 && administradores[0].Id == usuario.Id)
                return BadRequest("No podés eliminar al último administrador.");

            var result = await _userManager.DeleteAsync(usuario);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return NoContent();
        }

        // Crear Avatar
        // POST: api/RolesApi/5/avatar
        [HttpPost("{id}/avatar")]
        public async Task<IActionResult> ChangeAvatar(string id)
        {
            var usuario = await _userManager.FindByIdAsync(id);
            if (usuario == null)
                return NotFound();

            var file = Request.Form.Files.FirstOrDefault();
            if (file == null || file.Length == 0)
                return BadRequest("Debe seleccionar un archivo válido.");

            var carpeta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "avatars");
            Directory.CreateDirectory(carpeta);

            var nombreArchivo = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var rutaArchivo = Path.Combine(carpeta, nombreArchivo);

            using var stream = new FileStream(rutaArchivo, FileMode.Create);
            await file.CopyToAsync(stream);

            usuario.Avatar = "/img/avatars/" + nombreArchivo;
            await _userManager.UpdateAsync(usuario);

            return Ok(new { message = "Avatar actualizado correctamente.", avatarUrl = usuario.Avatar });
        }
    }

    // UsuarioDto(Evita exponer mas campos del modelo real)
    public class UsuarioDto
    {
        public string Id { get; set; } = null!;
        public string? Nombre { get; set; }
        public string? Apellido { get; set; }
        public string Email { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string? Avatar { get; set; }
        public List<string> Roles { get; set; } = new();
    }

    public class UsuarioUpdateDto
    {
        public string Id { get; set; } = null!;
        public string? Nombre { get; set; }
        public string? Apellido { get; set; }
        public string Email { get; set; } = null!;
        public string NuevoRol { get; set; } = null!;
    }
}
