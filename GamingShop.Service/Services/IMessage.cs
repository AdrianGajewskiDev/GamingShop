using GamingShop.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GamingShop.Service.Services
{
    public interface IMessage
    {
        IEnumerable<Message> GetAllSentByUser(string userID);
        IEnumerable<Message> GetAllSentToUser(string userID);
        Task<Message> GetByIDAsync(int id);
    }
}
