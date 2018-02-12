using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API.Services.EmailService
{
    public class LocalEmail : IMailService
    {
        private string mailTo = Startup.Configuration["MailSettings:MailTo"];
        private string mailFrom = Startup.Configuration["MailSettings:MailFrom"];

        public async Task SendEmailAsync(string subject, string message)
        {
            Debug.WriteLine($"Mail from {mailFrom} to {mailTo}, using LocalMail");
            Debug.WriteLine($"Subject: {subject}");
            Debug.WriteLine($"Message: {message}");

            await Task.CompletedTask;
        }
    }
}
