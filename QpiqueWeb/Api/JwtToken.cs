using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using QpiqueWeb.Models;
using Microsoft.AspNetCore.Identity; // o el namespace donde esté tu clase Usuario

public class JwtTokenService
{
    private readonly UserManager<Usuario> _userManager;
    private readonly IConfiguration _config;

    public JwtTokenService(IConfiguration config, UserManager<Usuario> userManager)
    {
        _config = config;
        _userManager = userManager;
    }

    public async Task<string> GenerateTokenAsync(Usuario usuario)
    {
        var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
        new Claim(ClaimTypes.Name, usuario.UserName ?? ""),
        new Claim(ClaimTypes.Email, usuario.Email ?? "")
    };

        var roles = await _userManager.GetRolesAsync(usuario);
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.UtcNow.AddDays(1);

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: expires,
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
