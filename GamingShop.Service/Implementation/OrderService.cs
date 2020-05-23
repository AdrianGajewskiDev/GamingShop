using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamingShop.Data.DbContext;
using GamingShop.Data.Models;
using GamingShop.Web.Data;

namespace GamingShop.Service
{
    public class OrderService : IOrder
    {
        private ApplicationDbContext _context;
        private readonly ApplicationDbContextFactory _contextFactory;

        public OrderService(ApplicationDbContextFactory context)
        {
            _contextFactory = context;
        }

        public async Task MarkGameAsSold(Game game)
        {
            using (_context = _contextFactory.CreateDbContext())
            {
                game.Sold = true;
                await _context.SaveChangesAsync();
            }

        }

        public async Task MarkGameAsSold(IEnumerable<Game> games)
        {
            using (_context = _contextFactory.CreateDbContext())
            {
                foreach (var game in games)
                {
                    await MarkGameAsSold(game);
                }
            }
        }

        public IEnumerable<Order> GetAllByCartID(int cartID)
        {
            using (_context = _contextFactory.CreateDbContext())
            {
                return _context.Orders.Where(order => order.CartID == cartID);
            }
        }

        public IEnumerable<Game> GetGamesFromOrder(int orderID)
        {
            using (_context = _contextFactory.CreateDbContext())
            {
                var items = _context.OrderItems.Where(x => x.OrderID == orderID);

                List<Game> games = new List<Game>();

                foreach (var item in items)
                {
                    games.Add(_context.Games.Where(x => x.ID == item.GameID).FirstOrDefault());
                }

                return games;
            }
        }
    }
}
