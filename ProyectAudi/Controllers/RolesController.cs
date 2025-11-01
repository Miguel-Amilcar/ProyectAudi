using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectAudi.Data;
using ProyectAudi.Models;
using ProyectAudi.Services;
using ProyectAudi.ViewModels.Roles;

namespace ProyectAudi.Controllers
{
    public class RolesController : Controller
    {
        private readonly ProyectDbContext _context;

        public RolesController(ProyectDbContext context)
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

            var lista = _context.ROL
                .Select(r => new IndexViewModel
                {
                    ROL_ID = r.ROL_ID,
                    ROL_NOMBRE = r.ROL_NOMBRE,
                    ROL_DESCRIPCION = r.ROL_DESCRIPCION,
                    FECHA_CREACION = r.FECHA_CREACION,
                    CantidadUsuarios = r.USUARIOS.Count
                })
                .ToList();

            return View(lista);
        }

        // 🔹 CREATE (GET)
        public IActionResult Create()
        {
            int? rolId = HttpContext.Session.GetInt32("RolId");
            var permisos = PermisoService.ObtenerPermisos(_context, rolId ?? 0);

            if (!PermisoService.Tiene(permisos, "CREAR"))
                return Forbid();

            ViewBag.Permisos = permisos;
            return View(new CreateViewModel());
        }

        // 🔹 CREATE (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateViewModel model)
        {
            int? rolId = HttpContext.Session.GetInt32("RolId");
            var permisos = PermisoService.ObtenerPermisos(_context, rolId ?? 0);

            if (!PermisoService.Tiene(permisos, "CREAR"))
                return Forbid();

            if (!ModelState.IsValid)
                return View(model);

            var usuarioActual = HttpContext.Session.GetString("UsuarioCorreo") ?? "Sistema";

            var nuevoRol = new ROL
            {
                ROL_NOMBRE = model.ROL_NOMBRE,
                ROL_DESCRIPCION = model.ROL_DESCRIPCION,
                CREADO_POR = usuarioActual,
                FECHA_CREACION = DateTime.UtcNow
            };

            _context.ROL.Add(nuevoRol);
            _context.SaveChanges();

            TempData["Alerta"] = "Rol creado correctamente.";
            return RedirectToAction("Index");
        }

        // 🔹 EDIT (GET)
        public IActionResult Edit(int id)
        {
            int? rolId = HttpContext.Session.GetInt32("RolId");
            var permisos = PermisoService.ObtenerPermisos(_context, rolId ?? 0);

            if (!PermisoService.Tiene(permisos, "EDITAR"))
                return Forbid();

            ViewBag.Permisos = permisos;

            var r = _context.ROL.Find(id);
            if (r == null) return NotFound();

            var model = new EditViewModel
            {
                ROL_ID = r.ROL_ID,
                ROL_NOMBRE = r.ROL_NOMBRE,
                ROL_DESCRIPCION = r.ROL_DESCRIPCION
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
                return View("Edit", model);

            var r = _context.ROL.Find(model.ROL_ID);
            if (r == null)
            {
                ModelState.AddModelError(nameof(model.ROL_NOMBRE), "No se encontró el rol.");
                return View("Edit", model);
            }

            r.ROL_NOMBRE = model.ROL_NOMBRE;
            r.ROL_DESCRIPCION = model.ROL_DESCRIPCION;

            _context.SaveChanges();
            TempData["Alerta"] = "Rol actualizado correctamente.";
            return RedirectToAction("Index");
        }

        // 🔹 DETAILS (GET)
        public IActionResult Details(int id)
        {
            int? rolId = HttpContext.Session.GetInt32("RolId");
            var permisos = PermisoService.ObtenerPermisos(_context, rolId ?? 0);

            if (!PermisoService.Tiene(permisos, "VER"))
                return Forbid();

            var r = _context.ROL
                .Include(r => r.USUARIOS)
                .Include(r => r.ROL_PERMISOS).ThenInclude(rp => rp.PERMISO)
                .FirstOrDefault(r => r.ROL_ID == id);

            if (r == null)
                return NotFound();

            var model = new DetailsViewModel
            {
                ROL_ID = r.ROL_ID,
                ROL_NOMBRE = r.ROL_NOMBRE,
                ROL_DESCRIPCION = r.ROL_DESCRIPCION,
                FECHA_CREACION = r.FECHA_CREACION,
                CREADO_POR = r.CREADO_POR,
                UsuariosAsociados = r.USUARIOS.Select(u =>
                    string.Join(" ", new[] {
                        u.PRIMERNOMBRE,
                        u.SEGUNDONOMBRE,
                        u.TERCERNOMBRE,
                        u.PRIMERAPELLIDO,
                        u.SEGUNDOAPELLIDO,
                        u.APELLIDOCASADA
                    }.Where(n => !string.IsNullOrWhiteSpace(n)))
                ).ToList(),
                PermisosAsignados = r.ROL_PERMISOS.Select(p => p.PERMISO.NOMBRE_PERMISO).ToList()

            };

            return View(model);
        }

        // 🔹 DELETE (GET)
        public IActionResult Delete(int id)
        {
            int? rolId = HttpContext.Session.GetInt32("RolId");
            var permisos = PermisoService.ObtenerPermisos(_context, rolId ?? 0);

            if (!PermisoService.Tiene(permisos, "ELIMINAR"))
                return Forbid();

            ViewBag.Permisos = permisos;

            var r = _context.ROL
                .Include(r => r.USUARIOS)
                .Include(r => r.ROL_PERMISOS)
                .FirstOrDefault(r => r.ROL_ID == id);

            if (r == null)
                return NotFound();

            var model = new DeleteViewModel
            {
                ROL_ID = r.ROL_ID,
                ROL_NOMBRE = r.ROL_NOMBRE,
                ROL_DESCRIPCION = r.ROL_DESCRIPCION,
                CantidadUsuarios = r.USUARIOS.Count,
                CantidadPermisos = r.ROL_PERMISOS.Count
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

            var r = _context.ROL.Find(id);
            if (r == null)
                return NotFound();

            _context.ROL.Remove(r);
            _context.SaveChanges();

            TempData["Alerta"] = "Rol eliminado correctamente.";
            return RedirectToAction("Index");
        }
    }
}
