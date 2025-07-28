using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QpiqueWeb.Data;
using QpiqueWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using QpiqueWeb.ViewModels;

namespace QpiqueWeb.Controllers
{
    public class VentasController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Usuario> _userManager;

        public VentasController(ApplicationDbContext context, UserManager<Usuario> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Vista Index Ventas(Consume la api VentasApiController)
        public IActionResult Index()
        {
            return View();
        }
        
        // Vista Crear Ventas(Consume la api VentasApiController)
        public IActionResult Crear()
        {
            return View();
        }

        // Vista Ganancias Ventas(Consume la api VentasApiController)
        public IActionResult Ganancias()
        {
            return View();
        }
    }
}
