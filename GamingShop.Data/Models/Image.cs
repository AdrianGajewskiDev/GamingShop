namespace GamingShop.Data.Models
{
    public class Image
    {
        public int ID { get; set; }
        public string UserID { get; set; }
        public int GameID { get; set; }
        public string UniqueName { get; set; }
        public string Path { get; set; }
    }
}
