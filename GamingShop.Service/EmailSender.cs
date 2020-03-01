using System.Threading.Tasks;
using System;
using System.Net.Mail;
using System.Net;
using GamingShop.Data.MyData;
using System.Collections.Generic;
using GamingShop.Data.Models;
using SendGrid;
using SendGrid.Helpers.Mail;
using Microsoft.Extensions.Options;

namespace GamingShop.Service
{
    public class EmailSender : IEmailSender
    {
        private readonly ApplicationOptions _options;

        public EmailSender(IOptions<ApplicationOptions> options)
        {
            _options = options.Value;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string htmlMessage, string plainTextContent = null)
        {

            var apiKey = _options.SendGridAPIKey;
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress(EmailCredentials.GetEmail());
            var to = new EmailAddress(toEmail);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent,htmlMessage);
            var response = await client.SendEmailAsync(msg);
        }

        public async Task SendOrderDetailsEmail(string toEmail, string subject, IEnumerable<Game> items, Address adress, decimal price)
        {
            string Body = System.IO.File.ReadAllText(@"C:\Users\adria\Projects\GamingShop\GamingShop.Service\EmailTemplates\OrderEmailTemplate.html");
            Body = Body.Replace("#Country#",adress.Country);
            Body = Body.Replace("#City#", adress.City);
            Body = Body.Replace("#Street#", adress.Street);
            Body = Body.Replace("#PhoneNumber#", adress.PhoneNumber);
            Body = Body.Replace("#price#", price.ToString());
            try
            {
                SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                client.EnableSsl = true;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(EmailCredentials.GetEmail(), EmailCredentials.GetPassword());

                MailMessage msg = new MailMessage();
                msg.IsBodyHtml = true;
                msg.To.Add(toEmail);
                msg.From = new MailAddress(EmailCredentials.GetEmail());
                msg.Subject = subject;
                msg.Body = Body;
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
