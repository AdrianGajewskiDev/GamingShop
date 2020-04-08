using GamingShop.Data.Models;
using GamingShop.Web.Data;
using System;
using System.Collections.Generic;

namespace GamingShop.Web.API.Helpers
{
    public class MessagesDatabaseSeeder
    {
        private readonly ApplicationDbContext _dbContext;

        public MessagesDatabaseSeeder(ApplicationDbContext context)
        {
            _dbContext = context;
        }

        public void AddMessages(int amount, string senderID, string recipientID)
        {
            List<Message> messages = new List<Message>();

            for (int i = 0; i <= amount; i++)
            {
                messages.Add(new Message
                {
                    Content = $"Content-{i}",
                    Read = false,
                    RecipientEmail = "",
                    RecipientID = recipientID,
                    SenderID = senderID,
                    Sent = DateTime.UtcNow,
                    Subject = $"Subject-{i}"
                }) ;
            }

            _dbContext.Messages.AddRange(messages);
            _dbContext.SaveChanges();

        }
    }
}
