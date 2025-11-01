using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProyectAudi.Data;
using ProyectAudi.Models;
using ProyectAudi.ViewModels;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ProyectAudi.Services;
using ProyectAudi.Modelspartial;
using ProyectAudi.ViewModels.Usuario;


namespace ProyectAudi.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly ProyectDbContext _context;
        private readonly IVirusScannerService _virusScanner;



        public UsuariosController(ProyectDbContext context, IVirusScannerService virusScanner)
        {
            _context = context;
            _virusScanner = virusScanner;
        }

        private void CargarRoles()
        {
            var roles = _context.ROL
                .Select(r => new SelectListItem
                {
                    Value = r.ROL_ID.ToString(),
                    Text = r.ROL_NOMBRE
                }).ToList();

            roles.Insert(0, new SelectListItem
            {
                Value = "",
                Text = " Elija ",
                Selected = true
            });

            ViewBag.Roles = roles;
        }

        public async Task<IActionResult> Index()
        {
            int? rolId = HttpContext.Session.GetInt32("RolId");                     // desde acaa
            var permisos = PermisoService.ObtenerPermisos(_context, rolId ?? 0);
            ViewBag.Permisos = permisos;                                            //hasta aca


            var usuarios = await _context.USUARIO
                .Include(u => u.ROL)
                .Where(u => !u.ELIMINADO)
                .Select(u => new IndexViewModel
                {
                    USUARIO_ID = u.USUARIO_ID,
                    CUI = u.CUI,
                    NombreCompleto = $"{u.PRIMERNOMBRE} {u.SEGUNDONOMBRE} {u.TERCERNOMBRE} {u.PRIMERAPELLIDO} {u.SEGUNDOAPELLIDO} {u.APELLIDOCASADA}".Trim(),
                    USUARIO_CORREO = u.USUARIO_CORREO,
                    TELEFONO = u.TELEFONO,
                    RolNombre = u.ROL.ROL_NOMBRE,
                    ESTADO_TINY = u.ESTADO_TINY
                }).ToListAsync();
            // Obtener rol del usuario actual


            return View(usuarios);
        }

        [HttpGet]
        public IActionResult Create()
        {
            int? rolId = HttpContext.Session.GetInt32("RolId");
            var permisos = PermisoService.ObtenerPermisos(_context, rolId ?? 0);

            if (!PermisoService.Tiene(permisos, "CREAR"))
                return Forbid();

            ViewBag.Permisos = permisos;
            ViewBag.EsSuperAdmin = rolId == 1;

            if (rolId == 1)
            {
                ViewBag.Roles = _context.ROL
                    .Select(r => new SelectListItem
                    {
                        Value = r.ROL_ID.ToString(),
                        Text = r.ROL_NOMBRE
                    }).ToList();
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateViewModel model)
        {
            int? rolId = HttpContext.Session.GetInt32("RolId");
            var permisos = PermisoService.ObtenerPermisos(_context, rolId ?? 0);

            if (!PermisoService.Tiene(permisos, "CREAR"))
                return Forbid();

            // Validar edad
            if (model.FECHA_NACIMIENTO.HasValue)
            {
                var edad = DateTime.Today.Year - model.FECHA_NACIMIENTO.Value.Year;
                if (model.FECHA_NACIMIENTO.Value.Date > DateTime.Today.AddYears(-edad)) edad--;
                if (edad < 18 || edad > 65)
                    ModelState.AddModelError("FECHA_NACIMIENTO", "La edad debe estar entre 18 y 65 años.");
            }

            // Validar duplicados
            if (_context.USUARIO.Any(u => u.CUI == model.CUI))
                ModelState.AddModelError("CUI", "Ya existe un usuario con este CUI.");

            if (_context.USUARIO.Any(u => u.USUARIO_CORREO == model.USUARIO_CORREO))
                ModelState.AddModelError("USUARIO_CORREO", "Ya existe un usuario con este correo.");

            // Validar rol solo si es SuperAdmin
            if (rolId == 1)
            {
                if (!_context.ROL.Any(r => r.ROL_ID == model.ROL_ID))
                    ModelState.AddModelError("ROL_ID", "El rol seleccionado no es válido.");
            }
            else
            {
                model.ROL_ID = 0; // Seguridad: evitar asignación si no es SuperAdmin
            }

            // Validar tamaño de archivos
            if (model.FotografiaFile?.Length > 5_000_000)
                ModelState.AddModelError("FotografiaFile", "La imagen no debe superar los 5 MB.");

            if (model.DpiFile?.Length > 10_000_000)
                ModelState.AddModelError("DpiFile", "El archivo PDF no debe superar los 10 MB.");

            // Validar tipo MIME
            if (model.FotografiaFile != null && !model.FotografiaFile.ContentType.StartsWith("image/"))
                ModelState.AddModelError("FotografiaFile", "El archivo debe ser una imagen válida.");

            if (model.DpiFile != null && model.DpiFile.ContentType != "application/pdf")
                ModelState.AddModelError("DpiFile", "El archivo debe ser un PDF válido.");

            // Escaneo de archivos
            if (model.FotografiaFile != null && !await _virusScanner.ArchivoEsSeguroAsync(model.FotografiaFile))
                ModelState.AddModelError("FotografiaFile", "La imagen contiene amenazas.");

            if (model.DpiFile != null && !await _virusScanner.ArchivoEsSeguroAsync(model.DpiFile))
                ModelState.AddModelError("DpiFile", "El archivo PDF contiene amenazas.");

            if (!ModelState.IsValid)
            {
                ViewBag.Permisos = permisos;
                ViewBag.EsSuperAdmin = rolId == 1;

                if (rolId == 1)
                {
                    ViewBag.Roles = _context.ROL
                        .Select(r => new SelectListItem
                        {
                            Value = r.ROL_ID.ToString(),
                            Text = r.ROL_NOMBRE
                        }).ToList();
                }

                return View(model);
            }

            var usuario = new USUARIO
            {
                CUI = model.CUI,
                PRIMERNOMBRE = model.PRIMERNOMBRE,
                SEGUNDONOMBRE = model.SEGUNDONOMBRE,
                TERCERNOMBRE = model.TERCERNOMBRE,
                PRIMERAPELLIDO = model.PRIMERAPELLIDO,
                SEGUNDOAPELLIDO = model.SEGUNDOAPELLIDO,
                APELLIDOCASADA = model.APELLIDOCASADA,
                FECHA_NACIMIENTO = model.FECHA_NACIMIENTO,
                TELEFONO = model.TELEFONO,
                DIRECCION = model.DIRECCION,
                PERSONA_NIT = model.PERSONA_NIT,
                PERSONA_DIRECCION = model.PERSONA_DIRECCION,
                PERSONA_TELEFONOCASA = model.PERSONA_TELEFONOCASA,
                PERSONA_TELEFONOMOVIL = model.PERSONA_TELEFONOMOVIL,
                USUARIO_CORREO = model.USUARIO_CORREO,
                ROL_ID = model.ROL_ID.Value,
                ESTADO_TINY = 1,
                CREADO_POR = User.Identity?.Name ?? "Sistema",
                FECHA_CREACION = DateTime.Now,
                ELIMINADO = false,
                FOTOGRAFIA = await ConvertToBytesAsync(model.FotografiaFile),
                DPI_PDF = await ConvertToBytesAsync(model.DpiFile)
            };

            try
            {
                _context.USUARIO.Add(usuario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Ocurrió un error al guardar el usuario.");
                ViewBag.Permisos = permisos;
                ViewBag.EsSuperAdmin = rolId == 1;

                if (rolId == 1)
                {
                    ViewBag.Roles = _context.ROL
                        .Select(r => new SelectListItem
                        {
                            Value = r.ROL_ID.ToString(),
                            Text = r.ROL_NOMBRE
                        }).ToList();
                }

                return View(model);
            }
        }



        // Método auxiliar para convertir archivos a byte[]
        private async Task<byte[]> ConvertToBytesAsync(IFormFile? file)
        {
            if (file == null) return null!;
            using var ms = new MemoryStream();
            await file.CopyToAsync(ms);
            return ms.ToArray();
        }

        public async Task<IActionResult> Details(int id)
        {
            int? rolId = HttpContext.Session.GetInt32("RolId");
            var permisos = PermisoService.ObtenerPermisos(_context, rolId ?? 0);

            if (!PermisoService.Tiene(permisos, "VER"))
                return Forbid();

            ViewBag.Permisos = permisos; // ✅ Para que la vista también lo reciba, osea las validaciones del rol

            var usuario = await _context.USUARIO
                .Include(u => u.ROL)
                .FirstOrDefaultAsync(u => u.USUARIO_ID == id && !u.ELIMINADO);

            if (usuario == null) return NotFound();

            var model = new DetailsViewModel
            {
                USUARIO_ID = usuario.USUARIO_ID,
                CUI = usuario.CUI,
                PRIMERNOMBRE = usuario.PRIMERNOMBRE,
                SEGUNDONOMBRE = usuario.SEGUNDONOMBRE,
                TERCERNOMBRE = usuario.TERCERNOMBRE,
                PRIMERAPELLIDO = usuario.PRIMERAPELLIDO,
                SEGUNDOAPELLIDO = usuario.SEGUNDOAPELLIDO,
                APELLIDOCASADA = usuario.APELLIDOCASADA,
                FECHA_NACIMIENTO = usuario.FECHA_NACIMIENTO,
                TELEFONO = usuario.TELEFONO,
                DIRECCION = usuario.DIRECCION,
                PERSONA_NIT = usuario.PERSONA_NIT,
                PERSONA_DIRECCION = usuario.PERSONA_DIRECCION,
                PERSONA_TELEFONOCASA = usuario.PERSONA_TELEFONOCASA,
                PERSONA_TELEFONOMOVIL = usuario.PERSONA_TELEFONOMOVIL,
                USUARIO_CORREO = usuario.USUARIO_CORREO,
                RolNombre = usuario.ROL.ROL_NOMBRE,
                ESTADO_TINY = usuario.ESTADO_TINY,
                CREADO_POR = usuario.CREADO_POR,
                FECHA_CREACION = usuario.FECHA_CREACION,
                MODIFICADO_POR = usuario.MODIFICADO_POR,
                FECHA_MODIFICACION = usuario.FECHA_MODIFICACION
            };

            return View(model);
        }
        public async Task<IActionResult> Edit(int id)
        {
            int? rolId = HttpContext.Session.GetInt32("RolId");
            var permisos = PermisoService.ObtenerPermisos(_context, rolId ?? 0);

            if (!PermisoService.Tiene(permisos, "EDITAR"))
                return Forbid();

            ViewBag.Permisos = permisos;
            ViewBag.EsSuperAdmin = rolId == 1;

            var usuario = await _context.USUARIO.FindAsync(id);
            if (usuario == null || usuario.ELIMINADO) return NotFound();

            if (rolId == 1)
            {
                ViewBag.Roles = _context.ROL
                    .Select(r => new SelectListItem
                    {
                        Value = r.ROL_ID.ToString(),
                        Text = r.ROL_NOMBRE
                    }).ToList();
            }

            var model = new EditViewModel
            {
                USUARIO_ID = usuario.USUARIO_ID,
                CUI = usuario.CUI,
                PRIMERNOMBRE = usuario.PRIMERNOMBRE,
                SEGUNDONOMBRE = usuario.SEGUNDONOMBRE,
                TERCERNOMBRE = usuario.TERCERNOMBRE,
                PRIMERAPELLIDO = usuario.PRIMERAPELLIDO,
                SEGUNDOAPELLIDO = usuario.SEGUNDOAPELLIDO,
                APELLIDOCASADA = usuario.APELLIDOCASADA,
                FECHA_NACIMIENTO = usuario.FECHA_NACIMIENTO,
                TELEFONO = usuario.TELEFONO,
                DIRECCION = usuario.DIRECCION,
                PERSONA_NIT = usuario.PERSONA_NIT,
                PERSONA_DIRECCION = usuario.PERSONA_DIRECCION,
                PERSONA_TELEFONOCASA = usuario.PERSONA_TELEFONOCASA,
                PERSONA_TELEFONOMOVIL = usuario.PERSONA_TELEFONOMOVIL,
                USUARIO_CORREO = usuario.USUARIO_CORREO,
                ROL_ID = usuario.ROL_ID
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditViewModel model)
        {
            int? rolId = HttpContext.Session.GetInt32("RolId");
            var permisos = PermisoService.ObtenerPermisos(_context, rolId ?? 0);

            if (!PermisoService.Tiene(permisos, "EDITAR"))
                return Forbid();

            ViewBag.Permisos = permisos;
            ViewBag.EsSuperAdmin = rolId == 1;

            // Validar edad
            if (model.FECHA_NACIMIENTO.HasValue)
            {
                var edad = DateTime.Today.Year - model.FECHA_NACIMIENTO.Value.Year;
                if (model.FECHA_NACIMIENTO.Value.Date > DateTime.Today.AddYears(-edad)) edad--;
                if (edad < 18 || edad > 65)
                    ModelState.AddModelError("FECHA_NACIMIENTO", "La edad debe estar entre 18 y 65 años.");
            }

            // Validar rol solo si el usuario actual es SuperAdmin
            if (rolId == 1)
            {
                if (!_context.ROL.Any(r => r.ROL_ID == model.ROL_ID))
                    ModelState.AddModelError("ROL_ID", "El rol seleccionado no es válido.");

                ViewBag.Roles = _context.ROL
                    .Select(r => new SelectListItem
                    {
                        Value = r.ROL_ID.ToString(),
                        Text = r.ROL_NOMBRE
                    }).ToList();
            }

            // Validar tamaño de archivos
            if (model.FotografiaFile?.Length > 5_000_000)
                ModelState.AddModelError("FotografiaFile", "La imagen no debe superar los 5 MB.");

            if (model.DpiFile?.Length > 10_000_000)
                ModelState.AddModelError("DpiFile", "El archivo PDF no debe superar los 10 MB.");

            // Escaneo de archivos
            if (model.FotografiaFile != null && !await _virusScanner.ArchivoEsSeguroAsync(model.FotografiaFile))
                ModelState.AddModelError("FotografiaFile", "La imagen contiene amenazas.");

            if (model.DpiFile != null && !await _virusScanner.ArchivoEsSeguroAsync(model.DpiFile))
                ModelState.AddModelError("DpiFile", "El archivo PDF contiene amenazas.");

            if (!ModelState.IsValid)
                return View(model);

            var usuario = await _context.USUARIO.FindAsync(model.USUARIO_ID);
            if (usuario == null || usuario.ELIMINADO) return NotFound();

            // Actualizar campos
            usuario.CUI = model.CUI;
            usuario.PRIMERNOMBRE = model.PRIMERNOMBRE;
            usuario.SEGUNDONOMBRE = model.SEGUNDONOMBRE;
            usuario.TERCERNOMBRE = model.TERCERNOMBRE;
            usuario.PRIMERAPELLIDO = model.PRIMERAPELLIDO;
            usuario.SEGUNDOAPELLIDO = model.SEGUNDOAPELLIDO;
            usuario.APELLIDOCASADA = model.APELLIDOCASADA;
            usuario.FECHA_NACIMIENTO = model.FECHA_NACIMIENTO;
            usuario.TELEFONO = model.TELEFONO;
            usuario.DIRECCION = model.DIRECCION;
            usuario.PERSONA_NIT = model.PERSONA_NIT;
            usuario.PERSONA_DIRECCION = model.PERSONA_DIRECCION;
            usuario.PERSONA_TELEFONOCASA = model.PERSONA_TELEFONOCASA;
            usuario.PERSONA_TELEFONOMOVIL = model.PERSONA_TELEFONOMOVIL;
            usuario.USUARIO_CORREO = model.USUARIO_CORREO;
            usuario.MODIFICADO_POR = model.MODIFICADO_POR;
            usuario.FECHA_MODIFICACION = DateTime.Now;

            // Solo SuperAdmin puede modificar el rol
            if (rolId == 1)
                usuario.ROL_ID = model.ROL_ID;

            // Guardar archivos
            if (model.FotografiaFile != null)
            {
                using var ms = new MemoryStream();
                await model.FotografiaFile.CopyToAsync(ms);
                usuario.FOTOGRAFIA = ms.ToArray();
            }

            if (model.DpiFile != null)
            {
                using var ms = new MemoryStream();
                await model.DpiFile.CopyToAsync(ms);
                usuario.DPI_PDF = ms.ToArray();
            }
            _context.Entry(usuario).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            int? rolId = HttpContext.Session.GetInt32("RolId");
            var permisos = PermisoService.ObtenerPermisos(_context, rolId ?? 0);        //desede aca

            if (!PermisoService.Tiene(permisos, "ELIMINAR"))
                return Forbid();

            ViewBag.Permisos = permisos; // ✅ Para que la vista también lo reciba

            var usuario = await _context.USUARIO
                .Include(u => u.ROL)
                .FirstOrDefaultAsync(u => u.USUARIO_ID == id && !u.ELIMINADO);

            if (usuario == null) return NotFound();

            var model = new DeleteViewModel
            {
                USUARIO_ID = usuario.USUARIO_ID,
                CUI = usuario.CUI,
                NombreCompleto = $"{usuario.PRIMERNOMBRE} {usuario.SEGUNDONOMBRE} {usuario.TERCERNOMBRE} {usuario.PRIMERAPELLIDO} {usuario.SEGUNDOAPELLIDO} {usuario.APELLIDOCASADA}".Trim(),
                USUARIO_CORREO = usuario.USUARIO_CORREO,
                TELEFONO = usuario.TELEFONO,
                DIRECCION = usuario.DIRECCION,
                RolNombre = usuario.ROL.ROL_NOMBRE,
                ESTADO_TINY = usuario.ESTADO_TINY,
                CREADO_POR = usuario.CREADO_POR,
                FECHA_CREACION = usuario.FECHA_CREACION
            };

            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            int? rolId = HttpContext.Session.GetInt32("RolId");
            var permisos = PermisoService.ObtenerPermisos(_context, rolId ?? 0);

            if (!PermisoService.Tiene(permisos, "ELIMINAR"))
                return Forbid();



            var usuario = await _context.USUARIO.FindAsync(id);
            if (usuario == null || usuario.ELIMINADO) return NotFound();

            usuario.ELIMINADO = true;
            usuario.ELIMINADO_POR = User.Identity?.Name ?? "Sistema";
            usuario.FECHA_ELIMINACION = DateTime.Now;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}