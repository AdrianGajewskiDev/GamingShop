using GamingShop.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GamingShop.Service
{
    public interface IOrder
    {
        IEnumerable<Order> GetAllByCartID(int cartID);
        IEnumerable<Game> GetGamesFromOrder(int orderID);
        Task MarkGameAsSold(Game game);
        Task MarkGameAsSold(IEnumerable<Game> games);
    }
}
