using System.Collections.Generic;
using System.Linq;
using GamingShop.Data.Models;
using GamingShop.Web.Data;
using Microsoft.EntityFrameworkCore;

namespace GamingShop.Service
{
    public class OrderService : IOrder
    {
        private readonly ApplicationDbContext _context;

        public OrderService(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Order> GetAllByCartID(int cartID)
        {
            return _context.Orders.Where(order => order.CartID == cartID);
        }

        public IEnumerable<Game> GetGamesFromOrder(int orderID)
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
