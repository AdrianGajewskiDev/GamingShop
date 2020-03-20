using GamingShop.Data;
using GamingShop.Service.Services;
using GamingShop.Web.Data;
using System.Collections.Generic;
using System.Linq;

namespace GamingShop.Service.Implementation
{
    public class SalesService : ISale
    {
        private readonly ApplicationDbContext _context;
        private readonly IImage _imageService;

        public SalesService(ApplicationDbContext context, IImage imageService)
        {
            _context = context;
            _imageService = imageService;
        }

        public IEnumerable<SaleModel> GetUserSales(string userID)
        {
            var games = _context.Games.Where(x => x.OwnerID == userID && x.Sold == false).ToList();

            List<SaleModel> sales = new List<SaleModel>();

            foreach (var item in games)
            {
                item.ImageUrl = _imageService.GetImageNameForGame(item.ID);
                sales.Add(new SaleModel
                {
                   Created = item.Posted.ToString("dd/MM/yyyy"),
                   GameItem = item,
                   Price  = item.Price
                });
            }

            return sales;
        }
    }
}
