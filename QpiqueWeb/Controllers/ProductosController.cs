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

namespace QpiqueWeb.Controllers
{
    public class ProductosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Usuario> _userManager;

        public ProductosController(ApplicationDbContext context, UserManager<Usuario> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Api: Productos(Index)
        public IActionResult Index()
        {
            return View();
        }

        // GET: Productos/Create
        public IActionResult Create()
        {
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Nombre");
            return View();
        }

        // POST: Productos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Descripcion,Precio,Stock,CategoriaId")] Producto producto, IFormFile ImagenArchivo)
        {
            if (ModelState.IsValid)
            {
                if (ImagenArchivo != null && ImagenArchivo.Length > 0)
                {
                    // Ruta de destino
                    var carpeta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/productos");
                    Directory.CreateDirectory(carpeta); // Asegura que la carpeta exista

                    var nombreArchivo = Guid.NewGuid().ToString() + Path.GetExtension(ImagenArchivo.FileName);
                    var rutaCompleta = Path.Combine(carpeta, nombreArchivo);

                    // Guardar la imagen en disco
                    using (var stream = new FileStream(rutaCompleta, FileMode.Create))
                    {
                        await ImagenArchivo.CopyToAsync(stream);
                    }

                    // Ruta relativa para guardar en la base de datos
                    producto.ImagenUrl = "/img/productos/" + nombreArchivo;
                }

                _context.Add(producto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Nombre", producto.CategoriaId);
            return View(producto);
        }
    }
}
