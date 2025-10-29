using Microsoft.AspNetCore.Mvc;
using ProyectAudi.Data;
using ProyectAudi.Modelspartial.Ver_contraseña;
using ProyectAudi.Services;

namespace ProyectAudi.Controllers
{
    public class Ver_ContraseñasController : Controller
    {
        private readonly ProyectDbContext _context;

        public Ver_ContraseñasController(ProyectDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            int? rolId = HttpContext.Session.GetInt32("RolId");

            // 🔒 Solo super admin puede acceder
            if (rolId != 1)
            {
                return RedirectToAction("AccesoDenegado", "Credenciales", new { ReturnUrl = "/Ver_Contraseñas" });
            }

            var lista = _context.CREDENCIAL
                .Where(c => !c.ELIMINADO && c.CONTRASENA_CIFRADA != null)
                .Select(c => new VerContraseñaViewModel
                {
                    CREDENCIAL_ID = c.CREDENCIAL_ID,
                    USUARIO_ID = c.USUARIO_ID,
                    USUARIO_NOMBRE = c.USUARIO_NOMBRE,
                    CONTRASENA = PasswordEncryptor.Decrypt(c.CONTRASENA_CIFRADA),
                    PASSWORD_ULTIMO_CAMBIO = c.PASSWORD_ULTIMO_CAMBIO,
                    MFA_ENABLED = c.MFA_ENABLED,
                    ULTIMA_IP = c.ULTIMA_IP,
                    ULTIMO_USER_AGENT = c.ULTIMO_USER_AGENT
                }).ToList();

            return View(lista);
        }
    }
}
