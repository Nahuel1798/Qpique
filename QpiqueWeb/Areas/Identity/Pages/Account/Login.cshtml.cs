// Licenciado a la .NET Foundation bajo uno o más acuerdos.
// La .NET Foundation le otorga esta licencia bajo la licencia MIT.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using QpiqueWeb.Models;

namespace QpiqueWeb.Areas.Identity.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<Usuario> _signInManager;
        private readonly ILogger<LoginModel> _logger;

        private readonly UserManager<Usuario> _userManager;
        private readonly JwtTokenService _jwtTokenService;

        public LoginModel(SignInManager<Usuario> signInManager, ILogger<LoginModel> logger, UserManager<Usuario> userManager, JwtTokenService jwtTokenService)
        {
            _signInManager = signInManager;
            _logger = logger;
            _userManager = userManager;
            _jwtTokenService = jwtTokenService;
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
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        /// <summary>
        ///     Esta API es compatible con la infraestructura de la interfaz de usuario predeterminada de ASP.NET Core Identity y no está destinada a ser utilizada
        ///     directamente desde su código. Esta API puede cambiar o eliminarse en futuras versiones.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     Esta API es compatible con la infraestructura de la interfaz de usuario predeterminada de ASP.NET Core Identity y no está destinada a ser utilizada
        ///     directamente desde su código. Esta API puede cambiar o eliminarse en futuras versiones.
        /// </summary>
        [TempData]
        public string ErrorMessage { get; set; }

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
            [Required]
            [EmailAddress]
            [Display(Name = "Correo electrónico")]
            public string Email { get; set; }

            /// <summary>
            ///     Esta API es compatible con la infraestructura de la interfaz de usuario predeterminada de ASP.NET Core Identity y no está destinada a ser utilizada
            ///     directamente desde su código. Esta API puede cambiar o eliminarse en futuras versiones.
            /// </summary>
            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Contraseña")]
            public string Password { get; set; }

            /// <summary>
            ///     Esta API es compatible con la infraestructura de la interfaz de usuario predeterminada de ASP.NET Core Identity y no está destinada a ser utilizada
            ///     directamente desde su código. Esta API puede cambiar o eliminarse en futuras versiones.
            /// </summary>
            [Display(Name = "¿Recordarme?")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/");

            // Limpiar la cookie externa existente para asegurar un proceso de inicio de sesión limpio
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        public string? JwtToken { get; set; } // Token expuesto a la vista

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
    {
        returnUrl ??= Url.Content("~/");

        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByEmailAsync(Input.Email);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Cuenta inválida.");
                return Page();
            }

            var result = await _signInManager.PasswordSignInAsync(user.UserName, Input.Password, Input.RememberMe, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                // ✅ Generar el token JWT
                JwtToken = await _jwtTokenService.GenerateTokenAsync(user);

                _logger.LogInformation("Usuario inició sesión.");
                return Page(); // No redirigimos para poder mostrar el token
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Intento de inicio de sesión inválido.");
                return Page();
            }
        }

        // Si llegamos hasta aquí, es que algo falló
        return Page();
    }
    }
}
