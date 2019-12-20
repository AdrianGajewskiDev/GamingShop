namespace GamingShop.Web.ViewModels
{
    public class GameDetailViewModel
    {
        public int ID { get; set; }
        public int Pegi { get; set; }
        public decimal Price { get; set; }
        public bool BestSeller { get; set; }
        public string ImageUrl { get; set; }
        public string LaunchDate { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Producent { get; set; }
        public string Platform { get; set; }
        public string Type { get; set; }
    }
}
