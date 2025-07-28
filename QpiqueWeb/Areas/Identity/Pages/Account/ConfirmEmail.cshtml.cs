// Licenciado a la .NET Foundation bajo uno o más acuerdos.
// La .NET Foundation le otorga esta licencia bajo la licencia MIT.
#nullable disable

using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using QpiqueWeb.Models;

namespace QpiqueWeb.Areas.Identity.Pages.Account
{
    public class ConfirmEmailModel : PageModel
    {
        private readonly UserManager<Usuario> _userManager;

        public ConfirmEmailModel(UserManager<Usuario> userManager)
        {
            _userManager = userManager;
        }

        /// <summary>
        ///     Esta API es compatible con la infraestructura de la interfaz de usuario predeterminada de ASP.NET Core Identity y no está destinada a ser utilizada
        ///     directamente desde su código. Esta API puede cambiar o eliminarse en futuras versiones.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }
        public async Task<IActionResult> OnGetAsync(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToPage("/Index");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"No se pudo cargar el usuario con ID '{userId}'.");
            }

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, code);
            StatusMessage = result.Succeeded ? "Gracias por confirmar su correo electrónico." : "Error al confirmar su correo electrónico.";
            return Page();
        }
    }
}
