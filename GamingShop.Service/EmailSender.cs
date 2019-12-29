using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Threading.Tasks;
using System;
using System.Net.Mail;
using System.Net;
using GamingShop.Data.MyData;

namespace GamingShop.Service
{
    public class EmailSender : IEmailSender
    {

        private readonly IHostingEnvironment _env;

        public EmailSender(IHostingEnvironment env)
        {

            _env = env;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string htmlMessage)
        {
            try
            {
                SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                client.EnableSsl = true;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(EmailCredentials.Email,EmailCredentials.Password);

                MailMessage msg = new MailMessage();
                msg.To.Add(toEmail);
                msg.From = new MailAddress(EmailCredentials.Email);
                msg.Subject = subject;
                msg.Body = htmlMessage;

                client.Send(msg);

            }
            catch (Exception ex)
            {
                // TODO: handle exception
                throw new InvalidOperationException(ex.Message);
            }
        }
    }
}
