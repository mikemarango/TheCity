using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API.Services.EmailService
{
    public class CloudEmail : IMailService
    {
        private string mailTo = Startup.Configuration["MailSettings:MailTo"];
        private string mailFrom = Startup.Configuration["MailSettings:MailFrom"];


        public async Task SendEmailAsync(string subject, string message)
        {
            // send mail - output to debug window
            Debug.WriteLine($"Mail from {mailFrom} to {mailTo}, using CloudEmail");
            Debug.WriteLine($"Subject: {subject}");
            Debug.WriteLine($"Message: {message}");

            await Task.CompletedTask;
        }
    }
}
