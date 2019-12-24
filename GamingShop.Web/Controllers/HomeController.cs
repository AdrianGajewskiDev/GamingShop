using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GamingShop.Web.Models;
using GamingShop.Service;
using GamingShop.Web.ViewModels;
using GamingShop.Data.Models;
using Microsoft.AspNetCore.Identity;

namespace GamingShop.Web.Controllers
{
    public class HomeController : Controller
    {

        private readonly IGame _gameService;
        private readonly ICart _cartService;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;


        public HomeController(IGame game, ICart cart, 
            SignInManager<ApplicationUser> manager, UserManager<ApplicationUser> userManager)
        {
            _gameService = game;
            _cartService = cart;
            _signInManager = manager;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var games = _gameService.GetAll().Select(game => new GameIndexViewModel
                {
                    ImageUrl = game.ImageUrl,
                    ID = game.ID,
                    Platform = game.Platform,
                    Price = game.Price,
                    Producent = game.Producent,
                    Title = game.Title
                }).ToList();

            
            var model = new HomeIndexModel
            {
                Games = games
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Search()
        {
            var query = Request.Form["searchBar"];
            return RedirectToAction("FilteredGames", "Game", new { searchQuery = query });
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> AddToCart(int game)
        {
            if (_signInManager.IsSignedIn(User))
            {
                var user = await _userManager.GetUserAsync(User);
                var item = _gameService.GetByID(game);
                _cartService.AddToCart(user.CartID, item);

                return RedirectToAction("Index");
            }
            else
                return Redirect("https://localhost:44367/Identity/Account/Login");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
