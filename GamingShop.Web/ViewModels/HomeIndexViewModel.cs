using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GamingShop.Web.ViewModels
{
    public class HomeIndexViewModel
    {
        public int ID { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public string Title { get; set; }
        public string Producent { get; set; }
        public string Platform { get; set; }

    }
}
