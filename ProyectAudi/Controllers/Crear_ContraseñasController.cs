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
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;
using System.Diagnostics;
using System.Security.Policy;

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
                .Where(c => !c.ELIMINADO) // 👈 Filtra solo credenciales activas
                .Select(c => new IndexViewModel
                {
                    CREDENCIAL_ID = c.CREDENCIAL_ID,
                    USUARIO_NOMBRE = c.USUARIO_NOMBRE,
                    USUARIO_ID = c.USUARIO_ID,
                    MFA_ENABLED = c.MFA_ENABLED,
                    ULTIMA_IP = c.ULTIMA_IP,
                    ULTIMO_USER_AGENT = c.ULTIMO_USER_AGENT
                })
                .ToList();

            return View(lista);
        }



        // 🔹 CREATE (GET)
        public IActionResult Create()
        {

            int? rolId = HttpContext.Session.GetInt32("RolId");                     ////desde aca hasta 
            var permisos = PermisoService.ObtenerPermisos(_context, rolId ?? 0);

            if (!PermisoService.Tiene(permisos, "CREAR"))
                return Forbid();

            ViewBag.Permisos = permisos;
            ViewBag.Usuarios = ObtenerUsuariosSelectList();
            return View(new CreateViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateViewModel model)
        {
            int? rolId = HttpContext.Session.GetInt32("RolId");
            var permisos = PermisoService.ObtenerPermisos(_context, rolId ?? 0);

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
            var encryptedPassword = PasswordEncryptor.Encrypt(model.PlainPassword); // ✅ AES

            var credencial = new CREDENCIAL
            {
                USUARIO_NOMBRE = model.USUARIO_NOMBRE,
                USUARIO_CONTRASENA_HASH = hash,
                USUARIO_SALT = salt,
                CONTRASENA_CIFRADA = encryptedPassword, // ✅ Guardar cifrada
                MFA_ENABLED = false,
                USUARIO_ID = model.USUARIO_ID,
                PASSWORD_ULTIMO_CAMBIO = DateTime.Now
            };

            _context.CREDENCIAL.Add(credencial);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }


        // 🔹 EDIT (GET)
        public IActionResult Edit(int id)
        {
            int? rolId = HttpContext.Session.GetInt32("RolId");                     ///desde aca hasta
            var permisos = PermisoService.ObtenerPermisos(_context, rolId ?? 0);

            if (!PermisoService.Tiene(permisos, "EDITAR"))
                return Forbid();

            ViewBag.Permisos = permisos; 


            var c = _context.CREDENCIAL.Find(id);
            if (c == null) return NotFound();

            var model = new EditViewModel
            {
                CREDENCIAL_ID = c.CREDENCIAL_ID,
                USUARIO_NOMBRE_ACTUAL = c.USUARIO_NOMBRE,
                USUARIO_NOMBRE_NUEVO = c.USUARIO_NOMBRE // para que el campo venga prellenado
            };


            return View(model);
        }


        // 🔹 EDIT (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(EditViewModel model)
        {
            int? rolId = HttpContext.Session.GetInt32("RolId");
            var permisos = PermisoService.ObtenerPermisos(_context, rolId ?? 0);

            if (!PermisoService.Tiene(permisos, "EDITAR"))
                return Forbid();

            if (!ModelState.IsValid)
                return View(model);

            var c = _context.CREDENCIAL.Find(model.CREDENCIAL_ID);
            if (c == null)
            {
                ModelState.AddModelError(nameof(model.USUARIO_NOMBRE_ACTUAL), "No se encontró la credencial.");
                return View(model);
            }

            bool quiereCambiarContraseña = !string.IsNullOrWhiteSpace(model.ContraseñaActual);

            if (quiereCambiarContraseña)
            {
                if (!ValidarPassword(model.ContraseñaActual, c.USUARIO_CONTRASENA_HASH, c.USUARIO_SALT))
                {
                    ModelState.AddModelError(nameof(model.ContraseñaActual), "La contraseña actual es incorrecta.");
                    return View(model);
                }

                var nuevoSalt = GenerateSalt();
                var nuevoHash = HashPassword(model.NuevaContraseña, nuevoSalt);
                var nuevaCifrada = PasswordEncryptor.Encrypt(model.NuevaContraseña); // ✅ AES

                c.USUARIO_CONTRASENA_HASH = nuevoHash;
                c.USUARIO_SALT = nuevoSalt;
                c.CONTRASENA_CIFRADA = nuevaCifrada; // ✅ Guardar cifrada
                c.PASSWORD_ULTIMO_CAMBIO = DateTime.Now;
            }

            c.USUARIO_NOMBRE = model.USUARIO_NOMBRE_NUEVO;

            _context.SaveChanges();
            TempData["Alerta"] = "Credencial actualizada correctamente.";
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
            var combinado = salt.Concat(Encoding.UTF8.GetBytes(password)).ToArray();
            using var sha = SHA256.Create(); // ✅ SHA256 para que coincida con el login
            return sha.ComputeHash(combinado);
        }


        private static bool ValidarPassword(string password, byte[] hashAlmacenado, byte[] salt)
        {
            var hashCalculado = HashPassword(password, salt);

            Debug.WriteLine("Salt: " + Convert.ToBase64String(salt));
            Debug.WriteLine("Hash esperado: " + Convert.ToBase64String(hashAlmacenado));
            Debug.WriteLine("Hash calculado: " + Convert.ToBase64String(hashCalculado));

            return hashCalculado.SequenceEqual(hashAlmacenado);
        }


        // 🔹 DELETE (GET)
        public IActionResult Delete(int id)
        {
            int? rolId = HttpContext.Session.GetInt32("RolId");
            var permisos = PermisoService.ObtenerPermisos(_context, rolId ?? 0);        //desede aca

            if (!PermisoService.Tiene(permisos, "ELIMINAR"))
                return Forbid();

            ViewBag.Permisos = permisos;

            var c = _context.CREDENCIAL.Find(id);
            if (c == null) return NotFound();

            var model = new DeleteViewModel
            {
                CREDENCIAL_ID = c.CREDENCIAL_ID,
                USUARIO_NOMBRE = c.USUARIO_NOMBRE,
                PASSWORD_ULTIMO_CAMBIO = c.PASSWORD_ULTIMO_CAMBIO,
                MFA_ENABLED = c.MFA_ENABLED,
                MFA_ULTIMO_USO = c.MFA_ULTIMO_USO,
                ULTIMA_IP = c.ULTIMA_IP,
                ULTIMO_USER_AGENT = c.ULTIMO_USER_AGENT
            };

            return View(model);
        }


        // 🔹 DELETE (POST)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            int? rolId = HttpContext.Session.GetInt32("RolId");
            var permisos = PermisoService.ObtenerPermisos(_context, rolId ?? 0);

            if (!PermisoService.Tiene(permisos, "ELIMINAR"))
                return Forbid();

            var c = _context.CREDENCIAL.Find(id);
            if (c == null || c.ELIMINADO) return NotFound();

            c.ELIMINADO = true;
            c.ELIMINADO_POR = User.Identity?.Name ?? "Sistema";
            c.FECHA_ELIMINACION = DateTime.Now;

            _context.SaveChanges();
            TempData["Alerta"] = "Credencial eliminada correctamente.";
            return RedirectToAction("Index");
        }


        // 🔹 DETAILS (GET)
        public IActionResult Details(int id)
        {
            int? rolId = HttpContext.Session.GetInt32("RolId");
            var permisos = PermisoService.ObtenerPermisos(_context, rolId ?? 0);

            if (!PermisoService.Tiene(permisos, "VER"))
                return Forbid();

            var c = _context.CREDENCIAL.Find(id);
            if (c == null)
                return NotFound();

            var model = new DetailsViewModel
            {
                CREDENCIAL_ID = c.CREDENCIAL_ID,
                USUARIO_NOMBRE = c.USUARIO_NOMBRE,
                USUARIO_ID = c.USUARIO_ID,
                PASSWORD_ULTIMO_CAMBIO = c.PASSWORD_ULTIMO_CAMBIO,
                MFA_ENABLED = c.MFA_ENABLED,
                MFA_ULTIMO_USO = c.MFA_ULTIMO_USO,
                ULTIMA_IP = c.ULTIMA_IP,
                ULTIMO_USER_AGENT = c.ULTIMO_USER_AGENT
            };

            return View(model);
        }

    }
}
