using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Formats.Tar;
using System.Threading.Tasks;
using QpiqueWeb.Models; // Asegúrate de que este namespace es correcto para tu proyecto

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class UserInfoController : ControllerBase
{
    private readonly UserManager<Usuario> _userManager;

    public UserInfoController(UserManager<Usuario> userManager)
    {
        _userManager = userManager;
    }

    [HttpGet("roles")]
    public async Task<IActionResult> GetRoles()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Unauthorized();

        var roles = await _userManager.GetRolesAsync(user);
        return Ok(roles); // Devuelve lista de roles
    }
}
