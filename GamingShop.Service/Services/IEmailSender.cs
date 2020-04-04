using GamingShop.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GamingShop.Service
{
    public interface IEmailSender
    {
        Task SendVerificationEmailAsync(ApplicationUser user, string subject, string link, string plainTextContent = null);
        Task SendOrderDetailsEmail(string toEmail, string subject, IEnumerable<Game> items, Address adress, decimal price);
        Task SendEmail(Message message);
    }
}
