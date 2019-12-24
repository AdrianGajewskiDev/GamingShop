using GamingShop.Data.Models;
using GamingShop.Service;
using GamingShop.Web.Models;
using GamingShop.Web.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GamingShop.Web.Controllers
{
    public class CartController : Controller
    {
        private readonly ICart _cartService;
        private readonly UserManager<ApplicationUser> _userManager;


        public CartController(ICart cart, UserManager<ApplicationUser> manager)
        {
            _cartService = cart;
            _userManager = manager;
        }

        public IActionResult Index(int id)
        {
            var games = _cartService.GetGames(id).Select(game => new GameIndexViewModel
            {
                Platform = game.Platform,
                Price = game.Price,
                Producent = game.Producent,
                Title = game.Title,
                ImageUrl = game.ImageUrl,
                ID = game.ID
            });

            var model = new CartIndexModel
            {
                Games = games,
                TotalPrice = CalculateTotalPrice(games)
            };


            return View(model);
        }

        public async Task<IActionResult> RemoveFromCart(int id)
        {

            var user = await _userManager.GetUserAsync(User);

            var game = _cartService.GetGames(user.CartID).Where(g => g.ID == id).First();

            _cartService.RemoveFormCart(user.CartID, game);

            return RedirectToAction("Index", new { id = user.CartID }) ;
        }

        private decimal CalculateTotalPrice(IEnumerable<GameIndexViewModel> games)
        {
            decimal price = 0;

            foreach (var game in games)
            {
                price += game.Price;
            }

            return price;
        }

    }
}