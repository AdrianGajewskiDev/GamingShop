namespace GamingShop.Web.API.Models
{
    public class NewMessage
    {
        public int GameID { get; set; }
        public string SenderID { get; set; }
        public string Content { get; set; }
        public string Subject { get; set; }
    }
}
