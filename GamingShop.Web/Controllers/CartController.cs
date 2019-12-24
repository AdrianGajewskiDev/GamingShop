using GamingShop.Service;
using GamingShop.Web.Models;
using GamingShop.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GamingShop.Web.Controllers
{
    public class CartController : Controller
    {
        private readonly ICart _cartService;

        public CartController(ICart cart)
        {
            _cartService = cart;
        }

        public IActionResult Index(int id)
        {
            var games = _cartService.GetGames(id).Select(game => new GameIndexViewModel
            {
                Platform = game.Platform,
                Price = game.Price,
                Producent = game.Producent,
                Title = game.Title,
                ImageUrl = game.ImageUrl
            });

            var model = new CartIndexModel
            {
                Games = games,
                TotalPrice = CalculateTotalPrice(games)
            };


            return View(model);
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