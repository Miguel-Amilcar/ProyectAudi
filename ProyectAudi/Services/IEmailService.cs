using System.Threading.Tasks;

namespace ProyectAudi.Services
{
    public interface IEmailService
    {
        Task SendAsync(string to, string subject, string body);
    }
}
