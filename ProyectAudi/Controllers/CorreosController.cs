using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectAudi.Data;
using ProyectAudi.Models;
using ProyectAudi.Modelspartial.Correos;
using ProyectAudi.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectAudi.Controllers
{
    public class CorreosController : Controller
    {
        private readonly ProyectDbContext _context;
        private readonly EmailService _emailService;

        public CorreosController(ProyectDbContext context, EmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        // ✅ Mostrar formulario de envío con datos del destinatario
        [HttpGet]
        public IActionResult EnviarCorreo(string correoDestino, string nombreCompleto)
        {
            var modelo = new RedactarCorreoViewModel
            {
                Destinatario = correoDestino,
                NombreCompleto = nombreCompleto
            };

            return View(modelo); // Vista: EnviarCorreo.cshtml
        }

        // ✅ Procesar y enviar el correo
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EnviarCorreo(RedactarCorreoViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var correo = new CORREO_ENVIADO
            {
                DESTINATARIO = model.Destinatario,
                ASUNTO = model.Asunto,
                CUERPO = model.Cuerpo,
                FECHA_ENVIO = DateTime.Now,
                ENVIADO_POR_ID = ObtenerUsuarioIdActual()
            };

            try
            {
                // Guardar en base de datos
                _context.CORREO_ENVIADO.Add(correo);
                await _context.SaveChangesAsync();

                // Enviar correo con plantilla Razor
                await _emailService.EnviarCorreoPersonalizadoAsync(model);

                TempData["Mensaje"] = "Correo enviado correctamente.";
                return RedirectToAction("CorreosEnviados");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al enviar el correo: " + ex.Message;
                return View(model);
            }
        }

        // ✅ Mostrar historial de correos enviados
        public IActionResult CorreosEnviados()
        {
            var correos = _context.CORREO_ENVIADO
                .Include(c => c.USUARIO_REMITENTE)
                .OrderByDescending(c => c.FECHA_ENVIO)
                .ToList();

            var modelo = correos.Select(c => new CorreoEnviadoViewModel
            {
                CorreoId = c.CORREO_ID,
                Destinatario = c.DESTINATARIO,
                Asunto = c.ASUNTO,
                Cuerpo = c.CUERPO,
                FechaEnvio = c.FECHA_ENVIO,
                NombreRemitente =
                    (c.USUARIO_REMITENTE.PRIMERNOMBRE ?? "") + " " +
                    (c.USUARIO_REMITENTE.SEGUNDONOMBRE ?? "") + " " +
                    (c.USUARIO_REMITENTE.TERCERNOMBRE ?? "") + " " +
                    (c.USUARIO_REMITENTE.PRIMERAPELLIDO ?? "") + " " +
                    (c.USUARIO_REMITENTE.SEGUNDOAPELLIDO ?? "") + " " +
                    (c.USUARIO_REMITENTE.APELLIDOCASADA ?? ""),
                RolRemitente = c.USUARIO_REMITENTE.ROL?.ROL_NOMBRE ?? "Sin rol",
                FechaCreacionRemitente = c.USUARIO_REMITENTE.FECHA_CREACION
            }).ToList();

            return View(modelo);
        }

        private int ObtenerUsuarioIdActual()
        {
            return HttpContext.Session.GetInt32("UsuarioId") ?? 0;
        }

        // ✅ Listado de usuarios para enviar correos
        public IActionResult UsuariosParaCorreo()
        {
            var usuarios = _context.USUARIO
                .Where(u => !u.ELIMINADO)
                .ToList();

            var resultado = usuarios.Select(u => new UsuarioCorreoViewModel
            {
                UsuarioId = u.USUARIO_ID,
                NombreCompleto =
                    (u.PRIMERNOMBRE ?? "") + " " +
                    (u.SEGUNDONOMBRE ?? "") + " " +
                    (u.TERCERNOMBRE ?? "") + " " +
                    (u.PRIMERAPELLIDO ?? "") + " " +
                    (u.SEGUNDOAPELLIDO ?? "") + " " +
                    (u.APELLIDOCASADA ?? ""),
                Correo = u.USUARIO_CORREO,
                Rol = _context.ROL.FirstOrDefault(r => r.ROL_ID == u.ROL_ID)?.ROL_NOMBRE ?? "Sin rol",
                FechaCreacion = u.FECHA_CREACION
            }).ToList();

            return View(resultado);
        }
    }
}
