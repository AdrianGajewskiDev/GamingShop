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
using System.IO;
using System.Reflection;
using System.Linq;

namespace GamingShop.Service
{
    public class SendGridEmailSender : IEmailSender
    {
        private readonly ApplicationOptions _options;
        private string APIKEY;
        private EmailAddress Address = new EmailAddress(EmailCredentials.GetEmail());

        public SendGridEmailSender(IOptions<ApplicationOptions> options)
        {
            _options = options.Value;
            APIKEY = _options.SendGridAPIKey;
        }

        public async Task SendVerificationEmailAsync(ApplicationUser user, string subject, string link, string plainTextContent = null)
        {
            var client = new SendGridClient(APIKEY);
            var to = new EmailAddress(user.Email);
            var path = @"C:\Users\adria\Projects\GamingShop\GamingShop.Service\EmailTemplates\Templates\VerificationTemplate.htm";
            string templateText = default;
            using (var sr = new StreamReader(path))
            {
                templateText = sr.ReadToEnd();
                templateText =  templateText.Replace("#Name", user.UserName).Replace("#link#", link);
            }
            var msg = MailHelper.CreateSingleEmail(Address, to, subject, plainTextContent, templateText);

            var response = await client.SendEmailAsync(msg);
        }

        public async Task SendOrderDetailsEmail(string toEmail, string subject, IEnumerable<Game> items, Address adress, decimal price)
        {
            string Body = System.IO.File.ReadAllText(@"C:\Users\adria\Projects\GamingShop\GamingShop.Service\EmailTemplates\Templates\OrderEmailTemplate.htm");
            Body = Body.Replace("#Country#",adress.Country);
            Body = Body.Replace("#City#", adress.City);
            Body = Body.Replace("#Street#", adress.Street);
            Body = Body.Replace("#PhoneNumber#", adress.PhoneNumber);
            Body = Body.Replace("#price#", price.ToString());

            foreach (var game in items)
            {
                Body += $"<p>{game.Title}</p>";
            }

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

        public async Task SendEmail(Message message)
        {
            var client = new SendGridClient(APIKEY);
            var to = new EmailAddress(message.RecipientEmail);
            var msg = MailHelper.CreateSingleEmail(Address, to, message.Subject, null, message.Content);
            var response = await client.SendEmailAsync(msg);
        }
    }
}
