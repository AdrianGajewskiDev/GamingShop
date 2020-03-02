using GamingShop.Data.Models;
using System.Collections.Generic;

namespace GamingShop.Service
{
    public interface IOrder
    {
        IEnumerable<Order> GetAllByCartID(int cartID);
        IEnumerable<Game> GetGamesFromOrder(int orderID);
    }
}
