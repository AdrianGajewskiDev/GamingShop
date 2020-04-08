using GamingShop.Data.Models;
using GamingShop.Service.Services;
using GamingShop.Web.Data;
using System.Collections.Generic;
using System.Linq;

namespace GamingShop.Service.Implementation
{
    public class MessageService : IMessage
    {
        private readonly ApplicationDbContext _dbContext;

        public MessageService(ApplicationDbContext context)
        {
            _dbContext = context;
        }

        public IEnumerable<Message> GetAllSentByUser(string userID)
        {
            var messages = _dbContext.Messages.Where(msg => msg.SenderID == userID);

            return messages;
        }

        public IEnumerable<Message> GetAllSentToUser(string userID)
        {
            var messages = _dbContext.Messages.Where(msg => msg.RecipientID == userID);

            return messages;
        }
    }
}
