using GamingShop.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GamingShop.Service
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string toEmail, string subject, string htmlMessage);
        Task SendOrderDetailsEmail(string toEmail, string subject, IEnumerable<Game> items, Address adress, decimal price);
    }
}
