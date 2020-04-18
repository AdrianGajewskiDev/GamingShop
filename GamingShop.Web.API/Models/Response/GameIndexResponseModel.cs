namespace GamingShop.Web.API.Models.Response
{
    public class GameIndexResponseModel
    {
        public int ID { get; set; }
        public decimal Price { get; set; }
        public bool Bestseller { get; set; }
        public string ImageUrl { get; set; }
        public string Title { get; set; }
        public string Producent { get; set; }
        public string Platform { get; set; }
    }
}
