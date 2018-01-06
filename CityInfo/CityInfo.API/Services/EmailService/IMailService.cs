namespace CityInfo.API.Services.EmailService
{
    public interface IMailService
    {
        void Send(string subject, string message);
    }
}