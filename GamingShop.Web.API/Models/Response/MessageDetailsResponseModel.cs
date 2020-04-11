namespace GamingShop.Web.API.Models.Response
{
    public class MessageDetailsResponseModel
    {
        public int ID { get; set; }
        public string SenderID { get; set; }
        public string SenderEmail { get; set; }
        public string RecipientID { get; set; }
        public string RecipientEmail { get; set; }
        public string Content { get; set; }
        public string Subject { get; set; }
        public bool Read { get; set; }
        public string Sent { get; set; }
    }
}
