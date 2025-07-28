// Licenciado a la .NET Foundation bajo uno o más acuerdos.
// La .NET Foundation le otorga esta licencia bajo la licencia MIT.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using QpiqueWeb.Models;

namespace QpiqueWeb.Areas.Identity.Pages.Account
{
    public class ForgotPasswordModel : PageModel
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly IEmailSender _emailSender;

        public ForgotPasswordModel(UserManager<Usuario> userManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _emailSender = emailSender;
        }

        /// <summary>
        ///     Esta API es compatible con la infraestructura de la interfaz de usuario predeterminada de ASP.NET Core Identity y no está destinada a ser utilizada
        ///     directamente desde su código. Esta API puede cambiar o eliminarse en futuras versiones.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     Esta API es compatible con la infraestructura de la interfaz de usuario predeterminada de ASP.NET Core Identity y no está destinada a ser utilizada
        ///     directamente desde su código. Esta API puede cambiar o eliminarse en futuras versiones.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     Esta API es compatible con la infraestructura de la interfaz de usuario predeterminada de ASP.NET Core Identity y no está destinada a ser utilizada
            ///     directamente desde su código. Esta API puede cambiar o eliminarse en futuras versiones.
            /// </summary>
            [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
            [EmailAddress(ErrorMessage = "El correo electrónico no es válido.")]
            public string Email { get; set; }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(Input.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    // No revelar que el usuario no existe o no está confirmado
                    return RedirectToPage("./ForgotPasswordConfirmation");
                }

                // Para más información sobre cómo habilitar la confirmación de cuenta y el restablecimiento de contraseña
                // visite https://go.microsoft.com/fwlink/?LinkID=532713
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Page(
                    "/Account/ResetPassword",
                    pageHandler: null,
                    values: new { area = "Identity", code },
                    protocol: Request.Scheme);

                await _emailSender.SendEmailAsync(
                    Input.Email,
                    "Restablecer contraseña",
                    $"Por favor restablezca su contraseña haciendo <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clic aquí</a>.");

                return RedirectToPage("./ForgotPasswordConfirmation");
            }

            return Page();
        }
    }
}
