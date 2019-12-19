using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GamingShop.Web.ViewModels
{
    public class GameIndexViewModel
    {
        public int ID { get; set; }
        public int Pegi { get; set; }
        public decimal Price { get; set; }
        public bool BestSeller { get; set; }
        public string ImageUrl { get; set; }
        public string DateOfLaunch { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Producent { get; set; }
        public string Platform { get; set; }
        public string Type { get; set; }

    }
}
