// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QpiqueWeb.Models;

namespace QpiqueWeb.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly SignInManager<Usuario> _signInManager;

        public IndexModel(
            UserManager<Usuario> userManager,
            SignInManager<Usuario> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Display(Name = "Nombre")]
            [Required]
            public string Nombre { get; set; }

            [Display(Name = "Apellido")]
            [Required]
            public string Apellido { get; set; }

            [Phone]
            [Display(Name = "Número de teléfono")]
            public string PhoneNumber { get; set; }

            [Display(Name = "Rol")]
            public string Rol { get; set; }

            [Display(Name = "Avatar")]
            public IFormFile AvatarNuevo { get; set; }

            public string AvatarActual { get; set; }
        }

        private async Task LoadAsync(Usuario user)
        {
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var rolActual = roles.FirstOrDefault();

            Input = new InputModel
            {
                Nombre = user.Nombre,
                Apellido = user.Apellido,
                PhoneNumber = phoneNumber,
                Rol = rolActual,
                AvatarActual = user.Avatar
            };
        }


        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"No se pudo cargar el usuario con ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            // Nombre y Apellido
            user.Nombre = Input.Nombre;
            user.Apellido = Input.Apellido;

            // Teléfono
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Error inesperado al intentar configurar el número de teléfono.";
                    return RedirectToPage();
                }
            }

            // Avatar
            if (Input.AvatarNuevo != null && Input.AvatarNuevo.Length > 0)
            {
                var carpeta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "avatars");
                Directory.CreateDirectory(carpeta);

                var nombreArchivo = $"{Guid.NewGuid()}{Path.GetExtension(Input.AvatarNuevo.FileName)}";
                var rutaArchivo = Path.Combine(carpeta, nombreArchivo);

                using (var stream = new FileStream(rutaArchivo, FileMode.Create))
                {
                    await Input.AvatarNuevo.CopyToAsync(stream);
                }

                user.Avatar = "/img/avatars/" + nombreArchivo;
            }

            // Rol (opcional, solo si el usuario tiene permiso para cambiarlo)
            var rolesActuales = await _userManager.GetRolesAsync(user);
            var rolActual = rolesActuales.FirstOrDefault();

            if (!string.IsNullOrEmpty(Input.Rol) && Input.Rol != rolActual)
            {
                if (rolActual != null)
                    await _userManager.RemoveFromRoleAsync(user, rolActual);

                await _userManager.AddToRoleAsync(user, Input.Rol);
            }

            await _userManager.UpdateAsync(user);
            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Tu perfil ha sido actualizado";
            return RedirectToPage();
        }
    }
}
