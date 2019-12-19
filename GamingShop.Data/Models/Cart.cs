using System.Collections.Generic;

namespace GamingShop.Data.Models
{
    public class Cart
    {
        public int ID { get; set; }
        public decimal TotalPrice { get; set; }

        public virtual IEnumerable<Game> Games { get; set; }
    }
}
