using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using System;

namespace GamingShop.Web.API.Models
{
    public class NewGameModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Producent { get; set; }
        public decimal Price { get; set; }
        public int Pegi { get; set; }
        public string LaunchDate { get; set; }
        public string Platform { get; set; }
        public string Type { get; set; }
        public string ImageUrl { get; set; }

    }
}
