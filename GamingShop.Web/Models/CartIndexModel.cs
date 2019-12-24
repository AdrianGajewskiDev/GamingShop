using GamingShop.Web.ViewModels;
using System.Collections.Generic;

namespace GamingShop.Web.Models
{
    public class CartIndexModel
    {
        public IEnumerable<GameIndexViewModel> Games { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
