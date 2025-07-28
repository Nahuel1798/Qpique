// Licenciado a la .NET Foundation bajo uno o más acuerdos.
// La .NET Foundation le otorga esta licencia bajo la licencia MIT.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
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
    public class ResetPasswordModel : PageModel
    {
        private readonly UserManager<Usuario> _userManager;

        public ResetPasswordModel(UserManager<Usuario> userManager)
        {
            _userManager = userManager;
        }

        /// <summary>
        ///     Esta API es compatible con la infraestructura de la IU predeterminada de ASP.NET Core Identity y no está destinada a ser utilizada
        ///     directamente desde su código. Esta API puede cambiar o eliminarse en versiones futuras.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     Esta API es compatible con la infraestructura de la IU predeterminada de ASP.NET Core Identity y no está destinada a ser utilizada
        ///     directamente desde su código. Esta API puede cambiar o eliminarse en versiones futuras.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     Esta API es compatible con la infraestructura de la IU predeterminada de ASP.NET Core Identity y no está destinada a ser utilizada
            ///     directamente desde su código. Esta API puede cambiar o eliminarse en versiones futuras.
            /// </summary>
            [Required]
            [EmailAddress]
            [Display(Name = "Correo electrónico")]
            public string Email { get; set; }

            /// <summary>
            ///     Esta API es compatible con la infraestructura de la IU predeterminada de ASP.NET Core Identity y no está destinada a ser utilizada
            ///     directamente desde su código. Esta API puede cambiar o eliminarse en versiones futuras.
            /// </summary>
            [Required]
            [StringLength(100, ErrorMessage = "La {0} debe tener al menos {2} y como máximo {1} caracteres.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Contraseña")]
            public string Password { get; set; }

            /// <summary>
            ///     Esta API es compatible con la infraestructura de la IU predeterminada de ASP.NET Core Identity y no está destinada a ser utilizada
            ///     directamente desde su código. Esta API puede cambiar o eliminarse en versiones futuras.
            /// </summary>
            [DataType(DataType.Password)]
            [Display(Name = "Confirmar contraseña")]
            [Compare("Password", ErrorMessage = "La contraseña y la confirmación no coinciden.")]
            public string ConfirmPassword { get; set; }

            /// <summary>
            ///     Esta API es compatible con la infraestructura de la IU predeterminada de ASP.NET Core Identity y no está destinada a ser utilizada
            ///     directamente desde su código. Esta API puede cambiar o eliminarse en versiones futuras.
            /// </summary>
            [Required]
            public string Code { get; set; }

        }

        public IActionResult OnGet(string code = null)
        {
            if (code == null)
            {
                return BadRequest("Debe proporcionar un código para restablecer la contraseña.");
            }
            else
            {
                Input = new InputModel
                {
                    Code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code))
                };
                return Page();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.FindByEmailAsync(Input.Email);
            if (user == null)
            {
                // No revelar que el usuario no existe
                return RedirectToPage("./ResetPasswordConfirmation");
            }

            var result = await _userManager.ResetPasswordAsync(user, Input.Code, Input.Password);
            if (result.Succeeded)
            {
                return RedirectToPage("./ResetPasswordConfirmation");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return Page();
        }
    }
}
