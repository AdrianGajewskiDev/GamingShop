namespace GamingShop.Data.Models
{
    public class OrderItem
    {
        public int ID { get; set; }
        public int OrderID { get; set; }
        public int GameID { get; set; }
        public int CartID { get; set; }
    }
}
