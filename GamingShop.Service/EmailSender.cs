﻿using System.Threading.Tasks;
using System;
using System.Net.Mail;
using System.Net;
using GamingShop.Data.MyData;
using System.Collections.Generic;
using GamingShop.Data.Models;

namespace GamingShop.Service
{
    public class EmailSender : IEmailSender
    {
        public async Task SendEmailAsync(string toEmail, string subject, string htmlMessage)
        {
            try
            {
                SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                client.EnableSsl = true;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(EmailCredentials.GetEmail(),EmailCredentials.GetPassword());

                MailMessage msg = new MailMessage();
                msg.To.Add(toEmail);
                msg.From = new MailAddress(EmailCredentials.GetEmail());
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