using GamingShop.Data.Models;
using GamingShop.Web.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GamingShop.Service
{
    public class CartService : ICart
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IGame _gameService;

        public CartService(ApplicationDbContext context, IGame service)
        {
            _dbContext = context;
            _gameService = service;

        }
    
        public  void AddToCart(int id, Game item)
        {
            var cart = GetById(id);

            var cartItem = new CartItem { CartID = cart.ID, GameID = item.ID };

            _dbContext.CartItems.Add(cartItem);

            _dbContext.SaveChanges();

        }

        public async Task ClearCart(int id)
        {
            var cart = GetById(id);

            var cartItems = _dbContext.CartItems.Where(x => x.CartID == cart.ID);

            foreach (var item in cartItems)
            {
                _dbContext.CartItems.Remove(item);

            }
            await _dbContext.SaveChangesAsync();
        }

        public Cart GetById(int id)
        {
            return _dbContext.Carts.Where(x => x.ID == id).FirstOrDefault();
        }

        public IEnumerable<Game> GetGames(int cartId)
        {
            var items = _dbContext.CartItems.Where(x => x.CartID == cartId);

            List<Game> games = new List<Game>();

            foreach (var item in items)
            {
                var game = _gameService.GetByID(item.GameID);
                games.Add(game);
            }

            return games;
        }

        public void RemoveFormCart(int id, Game item)
        {
            var cart = GetById(id);

            var cartItem = _dbContext.CartItems.Where(x => x.CartID == cart.ID).FirstOrDefault();

            _dbContext.CartItems.Remove(cartItem);
            _dbContext.SaveChanges();
        }
    }
}
