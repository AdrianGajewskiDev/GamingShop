using GamingShop.Data.DbContext;
using GamingShop.Data.Models;
using GamingShop.Web.Data;
using System;
using System.Collections.Generic;

namespace GamingShop.Web.API.Helpers
{
    public class MessagesDatabaseSeeder
    {
        private ApplicationDbContext _dbContext;
        private readonly ApplicationDbContextFactory _dbContextFactory;

        public MessagesDatabaseSeeder(ApplicationDbContextFactory contextFactory)
        {
            _dbContextFactory = contextFactory;
        }

        public void AddMessages(int amount, string senderID, string recipientID, string senderEmail, string recipientEmail)
        {
            using(_dbContext = _dbContextFactory.CreateDbContext())
            {
                List<Message> messages = new List<Message>();

                for (int i = 0; i <= amount; i++)
                {
                    messages.Add(new Message
                    {
                        Content = $"Content-{i}",
                        Read = false,
                        RecipientEmail = recipientEmail,
                        RecipientID = recipientID,
                        SenderID = senderID,
                        Sent = DateTime.UtcNow,
                        Subject = $"Subject-{i}",
                        SenderEmail = senderEmail
                    });
                }

                _dbContext.Messages.AddRange(messages);
                _dbContext.SaveChanges();
            }
          

        }
    }
}
