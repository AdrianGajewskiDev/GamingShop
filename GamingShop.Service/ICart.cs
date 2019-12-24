using GamingShop.Data.Models;
using System.Collections.Generic;

namespace GamingShop.Service
{
    public interface ICart
    {
        Cart GetById(int id);
        IEnumerable<Game> GetGames(int cartId);
        void AddToCart(int id, Game item);
        void RemoveFormCart(int id, Game item);
    }
}
