using System.Collections.Generic;

namespace GamingShop.Web.Models
{
    public class AccountLatestOrdersModel
    {
        public IEnumerable<OrderIndexModel> Orders { get; set; }
    }
}
