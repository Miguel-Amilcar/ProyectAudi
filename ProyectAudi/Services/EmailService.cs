using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

public class EmailService
{
    private readonly IConfiguration _config;

    public EmailService(IConfiguration config)
    {
        _config = config;
    }

    public async Task EnviarTokenAsync(string destinatario, string token)
    {
        var settings = _config.GetSection("EmailSettings");
        var smtp = new SmtpClient(settings["Host"], int.Parse(settings["Port"]))
        {
            Credentials = new NetworkCredential(settings["User"], settings["Password"]),
            EnableSsl = bool.Parse(settings["EnableSsl"])
        };

        var mensaje = new MailMessage
        {
            From = new MailAddress(settings["User"], "Soporte ProyectAudi"),
            Subject = "Tu token de recuperación",
            Body = $"Tu código de recuperación es: {token}\nEste código expira en 30 minutos.",
            IsBodyHtml = false
        };

        mensaje.To.Add(destinatario);
        await smtp.SendMailAsync(mensaje);
    }
}
