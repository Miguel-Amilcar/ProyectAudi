using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProyectAudi.Data;
using ProyectAudi.Models;
using ProyectAudi.Services;
using ProyectAudi.ViewModels;
using System.Security.Cryptography;
using System.Text;
using ProyectAudi.Modelspartial;
using ProyectAudi.ViewModels.Crear_Contraseña;

namespace ProyectAudi.Controllers
{
    public class Crear_ContraseñasController : Controller
    {
        private readonly ProyectDbContext _context;

        public Crear_ContraseñasController(ProyectDbContext context)
        {
            _context = context;
        }

        // 🔹 INDEX
        public IActionResult Index()
        {
            int? rolId = HttpContext.Session.GetInt32("RolId");
            var permisos = PermisoService.ObtenerPermisos(_context, rolId ?? 0);
            ViewBag.Permisos = permisos;

            if (!PermisoService.Tiene(permisos, "VER"))
                return Forbid();

            var lista = _context.CREDENCIAL
                .Select(c => new IndexViewModel
                {
                    CREDENCIAL_ID = c.CREDENCIAL_ID,
                    USUARIO_NOMBRE = c.USUARIO_NOMBRE,
                    USUARIO_ID = c.USUARIO_ID,
                    MFA_ENABLED = c.MFA_ENABLED,
                    INTENTOS_FALLIDOS = c.INTENTOS_FALLIDOS,
                    BLOQUEADO_HASTA = c.BLOQUEADO_HASTA,
                    FECHA_ULTIMO_INTENTO = c.FECHA_ULTIMO_INTENTO
                }).ToList();

            return View(lista);
        }

        // 🔹 CREATE (GET)
        public IActionResult Create()
        {
            var permisos = PermisoService.ObtenerPermisos(_context, HttpContext.Session.GetInt32("RolId") ?? 0);
            if (!PermisoService.Tiene(permisos, "CREAR"))
            return Forbid();

            ViewBag.Permisos = permisos;
            ViewBag.Usuarios = ObtenerUsuariosSelectList();
            return View(new CreateViewModel());
        }

        // 🔹 CREATE (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateViewModel model)
        {
            var permisos = PermisoService.ObtenerPermisos(_context, HttpContext.Session.GetInt32("RolId") ?? 0);
            if (!PermisoService.Tiene(permisos, "CREAR"))
                return Forbid();

            if (!ModelState.IsValid)
            {
                ViewBag.Usuarios = ObtenerUsuariosSelectList();
                model.NombreCompletoUsuario = ObtenerNombreCompleto(model.USUARIO_ID);
                return View(model);
            }

            var existeUsuarioNombre = _context.CREDENCIAL
                .Any(c => c.USUARIO_NOMBRE == model.USUARIO_NOMBRE);

            if (existeUsuarioNombre)
            {
                ModelState.AddModelError(nameof(model.USUARIO_NOMBRE), "Este nombre de usuario ya está en uso.");
                ViewBag.Usuarios = ObtenerUsuariosSelectList();
                model.NombreCompletoUsuario = ObtenerNombreCompleto(model.USUARIO_ID);
                return View(model);
            }

            var salt = GenerateSalt();
            var hash = HashPassword(model.PlainPassword, salt);

            var credencial = new CREDENCIAL
            {
                USUARIO_NOMBRE = model.USUARIO_NOMBRE,
                USUARIO_CONTRASENA_HASH = hash,
                USUARIO_SALT = salt,
                INTENTOS_FALLIDOS = 0,
                MFA_ENABLED = false,
                USUARIO_ID = model.USUARIO_ID
            };

            _context.CREDENCIAL.Add(credencial);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        // 🔹 EDIT (GET)
        public IActionResult Edit(int id)
        {
            var permisos = PermisoService.ObtenerPermisos(_context, HttpContext.Session.GetInt32("RolId") ?? 0);
            if (!PermisoService.Tiene(permisos, "EDITAR"))
                return Forbid();

            var c = _context.CREDENCIAL.Find(id);
            if (c == null) return NotFound();

            var model = new EditViewModel
            {
                CREDENCIAL_ID = c.CREDENCIAL_ID,
                USUARIO_NOMBRE = c.USUARIO_NOMBRE,
                USUARIO_ID = c.USUARIO_ID,
                INTENTOS_FALLIDOS = c.INTENTOS_FALLIDOS,
                MFA_ENABLED = c.MFA_ENABLED
            };

            ViewBag.Usuarios = ObtenerUsuariosSelectList();
            return View(model);
        }

        // 🔹 EDIT (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(EditViewModel model)
        {
            var permisos = PermisoService.ObtenerPermisos(_context, HttpContext.Session.GetInt32("RolId") ?? 0);
            if (!PermisoService.Tiene(permisos, "EDITAR"))
                return Forbid();

            if (!ModelState.IsValid)
            {
                ViewBag.Permisos = permisos;   //Si no le pasás los permisos otra vez, la vista podría ocultar el formulario o mostrar un mensaje de advertencia aunque el usuario sí tenga el permiso
                ViewBag.Usuarios = ObtenerUsuariosSelectList();
                return View(model);
            }

            var c = _context.CREDENCIAL.Find(model.CREDENCIAL_ID);
            if (c == null) return NotFound();

            c.USUARIO_NOMBRE = model.USUARIO_NOMBRE;
            c.USUARIO_ID = model.USUARIO_ID;
            c.INTENTOS_FALLIDOS = model.INTENTOS_FALLIDOS;
            c.MFA_ENABLED = model.MFA_ENABLED;

            c.FECHA_ULTIMO_INTENTO = DateTime.Now;
            c.PASSWORD_ULTIMO_CAMBIO = DateTime.Now;
            c.MFA_ULTIMO_USO = model.MFA_ENABLED ? DateTime.Now : null;
            c.BLOQUEADO_HASTA = model.INTENTOS_FALLIDOS >= 5 ? DateTime.Now.AddMinutes(15) : null;

            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        // 🔹 Helpers
        private List<SelectListItem> ObtenerUsuariosSelectList()
        {
            return _context.USUARIO
                .Select(u => new SelectListItem
                {
                    Value = u.USUARIO_ID.ToString(),
                    Text =
                        (u.PRIMERNOMBRE ?? "") + " " +
                        (u.SEGUNDONOMBRE ?? "") + " " +
                        (u.TERCERNOMBRE ?? "") + " " +
                        (u.PRIMERAPELLIDO ?? "") + " " +
                        (u.SEGUNDOAPELLIDO ?? "") + " " +
                        (u.APELLIDOCASADA ?? "")
                })
                .ToList();
        }

        private string ObtenerNombreCompleto(int usuarioId)
        {
            var u = _context.USUARIO.Find(usuarioId);
            if (u == null) return "";

            return
                (u.PRIMERNOMBRE ?? "") + " " +
                (u.SEGUNDONOMBRE ?? "") + " " +
                (u.TERCERNOMBRE ?? "") + " " +
                (u.PRIMERAPELLIDO ?? "") + " " +
                (u.SEGUNDOAPELLIDO ?? "") + " " +
                (u.APELLIDOCASADA ?? "");
        }

        private static byte[] GenerateSalt()
        {
            var buffer = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(buffer);
            return buffer;
        }

        private static byte[] HashPassword(string password, byte[] salt)
        {
            var combinado = Encoding.UTF8.GetBytes(password).Concat(salt).ToArray();
            using var sha = SHA512.Create();
            return sha.ComputeHash(combinado);
        }
        // 🔹 DELETE (GET)
        public IActionResult Delete(int id)
        {
            var permisos = PermisoService.ObtenerPermisos(_context, HttpContext.Session.GetInt32("RolId") ?? 0);
            if (!PermisoService.Tiene(permisos, "ELIMINAR"))
                return Forbid();

            var c = _context.CREDENCIAL.Find(id);
            if (c == null) return NotFound();

            var model = new DeleteViewModel
            {
                CREDENCIAL_ID = c.CREDENCIAL_ID,
                USUARIO_NOMBRE = c.USUARIO_NOMBRE,
                USUARIO_ID = c.USUARIO_ID,
                INTENTOS_FALLIDOS = c.INTENTOS_FALLIDOS,
                BLOQUEADO_HASTA = c.BLOQUEADO_HASTA,
                FECHA_ULTIMO_INTENTO = c.FECHA_ULTIMO_INTENTO,
                PASSWORD_ULTIMO_CAMBIO = c.PASSWORD_ULTIMO_CAMBIO,
                MFA_ENABLED = c.MFA_ENABLED,
                MFA_ULTIMO_USO = c.MFA_ULTIMO_USO
            };

            return View(model);
        }

        // 🔹 DELETE (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var permisos = PermisoService.ObtenerPermisos(_context, HttpContext.Session.GetInt32("RolId") ?? 0);
            if (!PermisoService.Tiene(permisos, "ELIMINAR"))
                return Forbid();

            var c = _context.CREDENCIAL.Find(id);
            if (c == null) return NotFound();

            _context.CREDENCIAL.Remove(c);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        // 🔹 DETAILS
        public IActionResult Details(int id)
        {
            var permisos = PermisoService.ObtenerPermisos(_context, HttpContext.Session.GetInt32("RolId") ?? 0);
            if (!PermisoService.Tiene(permisos, "VER"))
                return Forbid();

            var c = _context.CREDENCIAL.Find(id);
            if (c == null) return NotFound();

            var model = new DetailsViewModel
            {
                CREDENCIAL_ID = c.CREDENCIAL_ID,
                USUARIO_NOMBRE = c.USUARIO_NOMBRE,
                USUARIO_ID = c.USUARIO_ID,
                INTENTOS_FALLIDOS = c.INTENTOS_FALLIDOS,
                BLOQUEADO_HASTA = c.BLOQUEADO_HASTA,
                FECHA_ULTIMO_INTENTO = c.FECHA_ULTIMO_INTENTO,
                PASSWORD_ULTIMO_CAMBIO = c.PASSWORD_ULTIMO_CAMBIO,
                MFA_ENABLED = c.MFA_ENABLED,
                MFA_ULTIMO_USO = c.MFA_ULTIMO_USO
            };

            return View(model);
        }
    }
}
