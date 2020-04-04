using System;

namespace GamingShop.Data.Models
{
    public class Message
    {
        public int ID { get; set; }
        public string SenderID { get; set; }
        public string RecipientID { get; set; }
        public string RecipientEmail { get; set; }
        public string Content { get; set; }
        public string Subject { get; set; }
        public DateTime Sent { get; set; }
    }
}
