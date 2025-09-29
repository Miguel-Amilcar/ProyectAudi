using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using ProyectAudi.Data;
using ProyectAudi.Models;
using ProyectAudi.Modelspartial;
using OtpNet;
using QRCoder;
using System.Security.Cryptography;
using System.Text;
using System.Diagnostics;

namespace ProyectAudi.Controllers
{
    public class CredencialesController : Controller
    {
        private readonly ProyectDbContext _context;

        public CredencialesController(ProyectDbContext context)
        {
            _context = context;
        }

        // GET: Login
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.MFAActivo = false;
            return View(new CredencialesViewModel());
        }

        // POST: Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(CredencialesViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var credencial = await _context.CREDENCIAL
                .FirstOrDefaultAsync(c => c.USUARIO_NOMBRE == model.Usuario);

            if (credencial == null || !ValidarPassword(model.Contrasena, credencial))
            {
                ModelState.AddModelError("", "Usuario o contraseña incorrectos.");
                return View(model);
            }

            HttpContext.Session.SetString("UsuarioNombre", credencial.USUARIO_NOMBRE);
            HttpContext.Session.SetInt32("UsuarioId", credencial.USUARIO_ID);

            if (!credencial.MFA_ENABLED)
                return RedirectToAction("ActivarMFA");

            ViewBag.MFAActivo = true;
            return RedirectToAction("ValidarMFA");
        }

        // GET: Activar MFA
        [HttpGet]
        public async Task<IActionResult> ActivarMFA()
        {
            var usuarioNombre = HttpContext.Session.GetString("UsuarioNombre");
            var credencial = await _context.CREDENCIAL.FirstOrDefaultAsync(c => c.USUARIO_NOMBRE == usuarioNombre);
            if (credencial == null) return RedirectToAction("Index");

            var secret = KeyGeneration.GenerateRandomKey(20);
            var base32Secret = Base32Encoding.ToString(secret);
            var uri = $"otpauth://totp/ProyectoAudi:{credencial.USUARIO_NOMBRE}?secret={base32Secret}&issuer=ProyectoAudi";

            using var qrGenerator = new QRCodeGenerator();
            using var qrCodeData = qrGenerator.CreateQrCode(uri, QRCodeGenerator.ECCLevel.Q);
            using var qrCode = new Base64QRCode(qrCodeData);
            ViewBag.QrUri = "data:image/png;base64," + qrCode.GetGraphic(4);

            return View(new ActivarMfaViewModel { SecretoBase32 = base32Secret });
        }

        // POST: Confirmar MFA
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmarMFA(ActivarMfaViewModel model)
        {
            if (!ModelState.IsValid)
                return View("ActivarMFA", model);

            var usuarioNombre = HttpContext.Session.GetString("UsuarioNombre");
            var credencial = await _context.CREDENCIAL.FirstOrDefaultAsync(c => c.USUARIO_NOMBRE == usuarioNombre);
            if (credencial == null) return RedirectToAction("Index");

            var totp = new Totp(Base32Encoding.ToBytes(model.SecretoBase32));
            if (!totp.VerifyTotp(model.CodigoVerificacion, out _, VerificationWindow.RfcSpecifiedNetworkDelay))
            {
                ModelState.AddModelError("", "Código inválido. Intenta nuevamente.");
                ViewBag.QrUri = $"otpauth://totp/ProyectoAudi:{credencial.USUARIO_NOMBRE}?secret={model.SecretoBase32}&issuer=ProyectoAudi";
                return View("ActivarMFA", model);
            }

            credencial.MFA_SECRET_BASE32 = model.SecretoBase32;
            credencial.MFA_ENABLED = true;
            credencial.MFA_ULTIMO_USO = DateTime.Now;
            await _context.SaveChangesAsync();

            TempData["Mensaje"] = "MFA activado correctamente. Inicia sesión nuevamente.";
            return RedirectToAction("Index");
        }

        // GET: Validar MFA
        [HttpGet]
        public IActionResult ValidarMFA()
        {
            return View(new VerificarMfaViewModel());
        }

        // POST: Validar MFA
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ValidarMFA(VerificarMfaViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var usuarioNombre = HttpContext.Session.GetString("UsuarioNombre");
            var credencial = await _context.CREDENCIAL.FirstOrDefaultAsync(c => c.USUARIO_NOMBRE == usuarioNombre);
            if (credencial == null || string.IsNullOrEmpty(credencial.MFA_SECRET_BASE32))
                return RedirectToAction("Index");

            var totp = new Totp(Base32Encoding.ToBytes(credencial.MFA_SECRET_BASE32));
            if (!totp.VerifyTotp(model.CodigoMFA, out _, VerificationWindow.RfcSpecifiedNetworkDelay))
            {
                ModelState.AddModelError("", "Código MFA inválido.");
                return View(model);
            }

            credencial.MFA_ULTIMO_USO = DateTime.Now;
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");

        }

        // GET: Home
        [HttpGet]
        public IActionResult Home()
        {
            var usuario = HttpContext.Session.GetString("UsuarioNombre");
            if (string.IsNullOrEmpty(usuario))
                return RedirectToAction("Index");

            return View(new HomeViewModel
            {
                UsuarioNombre = usuario,
                MFA_Activo = true
            });
        }

        // Helpers
        private static bool ValidarPassword(string password, CREDENCIAL credencial)
        {
            var combinado = Encoding.UTF8.GetBytes(password).Concat(credencial.USUARIO_SALT).ToArray();
            var hash = SHA256.HashData(combinado);
            return hash.SequenceEqual(credencial.USUARIO_CONTRASENA_HASH);
        }
    }
}
