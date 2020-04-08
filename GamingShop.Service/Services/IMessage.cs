using GamingShop.Data.Models;
using System.Collections.Generic;

namespace GamingShop.Service.Services
{
    public interface IMessage
    {
        IEnumerable<Message> GetAllSentByUser(string userID);
        IEnumerable<Message> GetAllSentToUser(string userID);
    }
}
