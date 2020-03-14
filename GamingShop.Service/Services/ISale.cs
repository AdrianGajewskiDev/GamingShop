using GamingShop.Data;
using System.Collections.Generic;

namespace GamingShop.Service.Services
{
    public interface ISale
    {
        IEnumerable<SaleModel> GetUserSales(string userID);
    }
}
