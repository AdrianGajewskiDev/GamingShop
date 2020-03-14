using GamingShop.Data.Models;
using System;

namespace GamingShop.Data
{
    public class SaleModel
    {
        public Game GameItem { get; set; }
        public DateTime Created { get; set; }
        public decimal Price { get; set; }
    }
}
