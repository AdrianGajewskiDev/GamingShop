using GamingShop.Data.Models;
using GamingShop.Web.ViewModels;
using System.Collections.Generic;

namespace GamingShop.Web.Models
{
    public class OrderIndexModel
    {
        public IEnumerable<Game> Games { get; set; }
        public int ID { get; set; }
        public int CartID { get; set; }
        public int UserID { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
