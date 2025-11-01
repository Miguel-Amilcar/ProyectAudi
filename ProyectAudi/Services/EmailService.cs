using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MimeKit;
using ProyectAudi.Modelspartial.Correos;
using ProyectAudi.Services;


public class EmailService
{
    private readonly IConfiguration _config;

    public EmailService(IConfiguration config)
    {
        _config = config;
    }
    public async Task EnviarTokenAsync(string destinatario, string token)
    {
        // Renderiza la plantilla Razor con el token
        var renderer = new RazorViewRenderer();
        var htmlBody = await renderer.RenderAsync("Email/Recuperacion.cshtml", token);

        // Configura el cliente SMTP
        var settings = _config.GetSection("EmailSettings");
        var smtp = new SmtpClient(settings["Host"], int.Parse(settings["Port"]))
        {
            Credentials = new NetworkCredential(settings["User"], settings["Password"]),
            EnableSsl = bool.Parse(settings["EnableSsl"])
        };

        // Construye el mensaje
        var mensaje = new MailMessage
        {
            From = new MailAddress(settings["User"], "Soporte ProyectAudi"),
            Subject = "Tu token de recuperación",
            Body = htmlBody,
            IsBodyHtml = true
        };

        mensaje.To.Add(destinatario);

        // Envía el correo
        await smtp.SendMailAsync(mensaje);

    }


    public async Task EnviarCorreoPersonalizadoAsync(RedactarCorreoViewModel model)
    {
        // Renderiza la plantilla Razor con el modelo
        var renderer = new RazorViewRenderer();
        var htmlBody = await renderer.RenderAsync("Email/CorreoPersonalizado.cshtml", model);

        // Configura el cliente SMTP
        var settings = _config.GetSection("EmailSettings");
        var smtp = new SmtpClient(settings["Host"], int.Parse(settings["Port"]))
        {
            Credentials = new NetworkCredential(settings["User"], settings["Password"]),
            EnableSsl = bool.Parse(settings["EnableSsl"])
        };

        // Construye el mensaje
        var mensaje = new MailMessage
        {
            From = new MailAddress(settings["User"], "Soporte ProyectAudi"),
            Subject = model.Asunto,
            Body = htmlBody,
            IsBodyHtml = true
        };

        mensaje.To.Add(model.Destinatario);

        // Envía el correo
        await smtp.SendMailAsync(mensaje);
    }



}
