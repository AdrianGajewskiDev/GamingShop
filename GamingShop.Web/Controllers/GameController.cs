using System.Linq;
using GamingShop.Service;
using GamingShop.Web.Models;
using GamingShop.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GamingShop.Web.Controllers
{
    public class GameController : Controller
    {
        private readonly IGame _gameService;

        public GameController(IGame game)
        {
            _gameService = game;
        }

        public IActionResult Details(int id)
        {
            var item = _gameService.GetByID(id);

            var game = new GameDetailViewModel
            {
                BestSeller = item.BestSeller,
                Description = item.Description,
                ID = item.ID, 
                ImageUrl = item.ImageUrl,
                LaunchDate = _gameService.GetDateOfLaunch(item.ID),
                Pegi = item.Pegi,
                Platform = item.Platform,
                Price = item.Price,
                Producent = item.Producent,
                Title = item.Title,
                Type = item.Type

            };

            var model = new GameDetailModel
            {
                Game = game
            };

            return View(model);
        }

        public IActionResult FilteredGames(string searchQuery)
        {
            var games = _gameService.GetAllBySearchQuery(searchQuery).Select(game => new GameIndexViewModel
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
    }
}