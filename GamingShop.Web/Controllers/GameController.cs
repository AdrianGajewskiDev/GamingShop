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
    }
}