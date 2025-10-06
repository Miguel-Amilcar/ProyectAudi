using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using ProyectAudi.Data;
using ProyectAudi.Models;
using ProyectAudi.Modelspartial;

namespace ProyectAudi.Controllers
{
    public class RecuperacionController : Controller
    {
        private readonly ProyectDbContext _context;
        private readonly EmailService _email;

        public RecuperacionController(ProyectDbContext context, EmailService email)
        {
            _context = context;
            _email = email;
        }



        // GET: Recuperacion/SolicitarToken
        public IActionResult SolicitarToken()
        {
            var bloqueo = HttpContext.Session.GetString("RecuperacionBloqueada");
            if (bloqueo != null && DateTime.Parse(bloqueo) > DateTime.Now)
            {
                TempData["BloqueoRecuperacion"] = "La recuperación está desactivada hasta " + DateTime.Parse(bloqueo).ToString("g");
                return RedirectToAction("Index", "Credenciales", new { area = "Modelspartial" });
            }

            return View();
        }

        // POST: Recuperacion/EnviarToken
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EnviarToken(SolicitarTokenViewModel model)

        {
            if (!ModelState.IsValid)
                return View("SolicitarToken", model);

            var usuario = await _context.USUARIO.FirstOrDefaultAsync(u => u.USUARIO_CORREO == model.Correo);
            if (usuario == null)
            {
                ModelState.AddModelError("Correo", "Este correo no está registrado.");
                return View("SolicitarToken", model);
            }

            var token = new Random().Next(100000, 999999).ToString();

            var tokenExistente = await _context.TOKEN_RECUPERACION
                .FirstOrDefaultAsync(t => t.USUARIO_ID == usuario.USUARIO_ID);

            if (tokenExistente != null)
            {
                tokenExistente.TOKEN_VALOR = token;
                tokenExistente.FECHA_CREACION = DateTime.Now;
                tokenExistente.FECHA_EXPIRACION = DateTime.Now.AddMinutes(30);
                tokenExistente.USADO = false;
                tokenExistente.FECHA_USO = null;
                tokenExistente.CONSUMIDO_POR = null;
            }
            else
            {
                _context.TOKEN_RECUPERACION.Add(new TOKEN_RECUPERACION
                {
                    USUARIO_ID = usuario.USUARIO_ID,
                    TOKEN_VALOR = token,
                    FECHA_CREACION = DateTime.Now,
                    FECHA_EXPIRACION = DateTime.Now.AddMinutes(30),
                    USADO = false,
                    CREADO_POR = "RecuperacionController"
                });
            }

            await _context.SaveChangesAsync();
            await _email.EnviarTokenAsync(model.Correo, token);
            // Simulación de envío de correo
            Console.WriteLine($"Token enviado a {model.Correo}: {token}");

            TempData["Correo"] = model.Correo;
            return RedirectToAction("IngresarToken");

        }

        // GET: Recuperacion/IngresarToken
        public IActionResult IngresarToken()
        {
            return View();
        }

        // POST: Recuperacion/ValidarTokenYActualizarPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ValidarTokenYActualizarPassword(IngresarTokenViewModel model)
        {
            if (!ModelState.IsValid)
                return View("IngresarToken", model);

            var bloqueo = HttpContext.Session.GetString("RecuperacionBloqueada");
            if (bloqueo != null && DateTime.Parse(bloqueo) > DateTime.Now)
            {
                TempData["BloqueoRecuperacion"] = "La recuperación está desactivada hasta " + DateTime.Parse(bloqueo).ToString("g");
                return RedirectToAction("Index", "Credenciales", new { area = "Modelspartial" });
            }

            var token = await _context.TOKEN_RECUPERACION
                .Include(t => t.USUARIO)
                .FirstOrDefaultAsync(t => t.TOKEN_VALOR == model.Token && !t.USADO);

            if (token == null || token.FECHA_EXPIRACION < DateTime.Now)
            {
                int intentosFallidos = HttpContext.Session.GetInt32("IntentosToken") ?? 0;
                intentosFallidos++;
                HttpContext.Session.SetInt32("IntentosToken", intentosFallidos);

                if (intentosFallidos >= 3)
                {
                    HttpContext.Session.Remove("IntentosToken");
                    HttpContext.Session.SetString("RecuperacionBloqueada", DateTime.Now.AddMinutes(30).ToString("o"));
                    TempData["BloqueoRecuperacion"] = "Has excedido el número de intentos. La recuperación estará desactivada por 30 minutos.";
                    return RedirectToAction("Index", "Credenciales", new { area = "Modelspartial" });
                }

                ModelState.AddModelError("Token", "Token inválido o expirado.");
                return View("IngresarToken", model);
            }

            HttpContext.Session.Remove("IntentosToken");

            var salt = GenerarSalt();
            var hash = HashearPassword(model.NuevaContrasena, salt);

            var credencial = await _context.CREDENCIAL
                .FirstOrDefaultAsync(c => c.USUARIO_ID == token.USUARIO_ID);

            if (credencial != null)
            {
                credencial.USUARIO_CONTRASENA_HASH = hash;
                credencial.USUARIO_SALT = salt;
                credencial.PASSWORD_ULTIMO_CAMBIO = DateTime.Now;
                credencial.USUARIO_CAMBIOINICIAL = true;
            }

            token.USADO = true;
            token.FECHA_USO = DateTime.Now;
            token.IP_USO = HttpContext.Connection.RemoteIpAddress?.ToString();
            token.USER_AGENT = Request.Headers["User-Agent"].ToString();
            token.CONSUMIDO_POR = "RecuperacionController";

            await _context.SaveChangesAsync();

            TempData["Mensaje"] = "Contraseña actualizada correctamente. Puedes iniciar sesión.";
            return RedirectToAction("Index", "Credenciales", new { area = "Modelspartial" });

        }

        private static byte[] GenerarSalt()
        {
            var salt = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(salt);
            return salt;
        }

        private static byte[] HashearPassword(string password, byte[] salt)
        {
            using var sha = SHA512.Create();
            var combined = Encoding.UTF8.GetBytes(password).Concat(salt).ToArray();
            return sha.ComputeHash(combined);
        }
    }
}
