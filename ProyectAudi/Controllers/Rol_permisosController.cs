using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProyectAudi.Data;
using ProyectAudi.Models;
using ProyectAudi.ViewModels;
using System.Linq;

namespace ProyectAudi.Controllers
{
    public class Rol_permisosController : Controller
    {
        private readonly ProyectDbContext _context;

        public Rol_permisosController(ProyectDbContext context)
        {
            _context = context;
        }

        // 🔹 Redirección automática al selector de roles
        public IActionResult Index()
        {
            return RedirectToAction("SeleccionarRol");
        }

        // 🔹 Vista para seleccionar un rol
        public IActionResult SeleccionarRol()
        {
            int? rolActual = HttpContext.Session.GetInt32("RolId");
            if (rolActual != 1)
            {
                return Forbid(); // Solo SuperAdmin puede acceder
            }

            var roles = _context.ROL
                .Select(r => new SelectListItem
                {
                    Value = r.ROL_ID.ToString(),
                    Text = r.ROL_NOMBRE
                }).ToList();

            roles.Insert(0, new SelectListItem { Value = "", Text = " Elija un rol ", Selected = true });

            ViewBag.Roles = roles;
            return View();
        }

        // 🔹 Mostrar permisos del rol seleccionado
        public IActionResult EditarPermisos(int id)
        {
            int? rolActual = HttpContext.Session.GetInt32("RolId");
            if (rolActual != 1)
            {
                return Forbid(); // Solo SuperAdmin puede editar permisos
            }

            var rol = _context.ROL.Find(id);
            if (rol == null) return NotFound();

            var permisos = _context.PERMISO
                .Select(p => new PermisoAsignableViewModel
                {
                    PermisoId = p.PERMISO_ID,
                    NombrePermiso = p.NOMBRE_PERMISO,
                    Asignado = _context.ROL_PERMISO.Any(rp => rp.ROL_ID == id && rp.PERMISO_ID == p.PERMISO_ID)
                }).ToList();

            var model = new RolPermisoViewModel
            {
                RolId = rol.ROL_ID,
                RolNombre = rol.ROL_NOMBRE,
                Permisos = permisos
            };

            return View(model);
        }

        // 🔹 Guardar cambios de permisos
        [HttpPost]
        public IActionResult EditarPermisos(RolPermisoViewModel model)
        {
            int? rolActual = HttpContext.Session.GetInt32("RolId");
            if (rolActual != 1)
            {
                return Forbid(); // Solo SuperAdmin puede guardar cambios
            }

            var rol = _context.ROL.Find(model.RolId);
            if (rol == null) return NotFound();

            var permisosActuales = _context.ROL_PERMISO
                .Where(rp => rp.ROL_ID == model.RolId)
                .Select(rp => rp.PERMISO_ID)
                .ToHashSet();

            var nuevosPermisos = model.Permisos
                .Where(p => p.Asignado)
                .Select(p => p.PermisoId)
                .ToHashSet();

            bool hayCambios = !permisosActuales.SetEquals(nuevosPermisos);

            if (hayCambios)
            {
                var registrosActuales = _context.ROL_PERMISO
                    .Where(rp => rp.ROL_ID == model.RolId)
                    .ToList();

                _context.ROL_PERMISO.RemoveRange(registrosActuales);

                foreach (var permisoId in nuevosPermisos)
                {
                    _context.ROL_PERMISO.Add(new ROL_PERMISO
                    {
                        ROL_ID = model.RolId,
                        PERMISO_ID = permisoId
                    });
                }

                _context.SaveChanges();
                TempData["Mensaje"] = "Permisos actualizados correctamente.";
            }
            else
            {
                TempData["Mensaje"] = "No se realizaron modificaciones en los permisos.";
            }

            return RedirectToAction("EditarPermisos", new { id = model.RolId });
        }
    }
}
