using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using QpiqueWeb.Models;

namespace QpiqueWeb.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    // Vista Nostros
    public IActionResult Nosotros()
    {
        return View();
    }

    // Vista Contacto
    [HttpGet]
    public IActionResult Contacto()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult EnviarContacto(string Nombre, string Email, string Mensaje)
    {
        if (string.IsNullOrWhiteSpace(Nombre) || string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Mensaje))
        {
            ModelState.AddModelError(string.Empty, "Todos los campos son obligatorios.");
            return View("Contacto");
        }

        // Aquí procesarías el formulario: enviar email, guardar en base de datos, etc.
        // Por ahora vamos a mostrar un mensaje simple y volver a la página.

        TempData["MensajeExito"] = "Gracias por contactarnos. Te responderemos pronto.";

        return RedirectToAction("Contacto");
    }

    // Vista Index
    public IActionResult Index()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
