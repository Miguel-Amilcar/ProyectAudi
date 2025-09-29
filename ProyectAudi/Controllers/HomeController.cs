using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectAudi.Data;
using ProyectAudi.Models;
using ProyectAudi.Modelspartial;
using System.Diagnostics;

namespace ProyectAudi.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ProyectDbContext _context;

        public HomeController(ILogger<HomeController> logger, ProyectDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var usuarioId = HttpContext.Session.GetInt32("UsuarioId");
            if (usuarioId == null)
                return RedirectToAction("Index", "Credenciales");

            var credencial = await _context.CREDENCIAL
                .Include(c => c.USUARIO)
                .FirstOrDefaultAsync(c => c.USUARIO_ID == usuarioId);

            if (credencial == null)
                return RedirectToAction("Index", "Credenciales");

            var model = new HomeViewModel
            {
                UsuarioNombre = credencial.USUARIO_NOMBRE,
                MFA_Activo = credencial.MFA_ENABLED
            };

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
