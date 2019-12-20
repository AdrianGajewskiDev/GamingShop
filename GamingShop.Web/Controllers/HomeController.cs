using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GamingShop.Web.Models;
using GamingShop.Service;
using GamingShop.Web.ViewModels;

namespace GamingShop.Web.Controllers
{
    public class HomeController : Controller
    {

        private readonly IGame _gameService;

        public HomeController(IGame game)
        {
            _gameService = game;
        }

        public IActionResult Index()
        {

            var games = _gameService.GetAll().Select(game => new HomeIndexViewModel 
            {
                ImageUrl = game.ImageUrl, 
                ID = game.ID,
                Platform = game.Platform,
                Price = game.Price,
                Producent = game.Producent,
                Title = game.Title
            });

            var model = new HomeIndexModel
            {
                Games = games
            };

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
