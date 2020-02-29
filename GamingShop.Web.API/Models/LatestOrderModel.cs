using GamingShop.Data.Models;
using System.Collections.Generic;

namespace GamingShop.Web.API.Models
{
    public class LatestOrderModel
    {
        public string Placed { get; set; }
        public decimal Price { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public IEnumerable<Game> Games { get; set; }
    }
}
