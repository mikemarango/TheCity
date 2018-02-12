using System.Threading.Tasks;

namespace CityInfo.API.Services.EmailService
{
    public interface IMailService
    {
        Task SendEmailAsync(string subject, string message);
    }
}