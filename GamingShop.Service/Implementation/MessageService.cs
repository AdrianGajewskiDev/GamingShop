using GamingShop.Data.DbContext;
using GamingShop.Data.Models;
using GamingShop.Service.Services;
using GamingShop.Web.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GamingShop.Service.Implementation
{
    public class MessageService : IMessage
    {
        private ApplicationDbContext _dbContext;

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

        public async Task<Message> GetByIDAsync(int id)
        {
            var result = await _dbContext.FindAsync<Message>(id);

            //Mark message as read 
            if (result.Read == false)
            {
                result.Read = true;
                await _dbContext.SaveChangesAsync();
            }

            return result;
        }
    }
}
