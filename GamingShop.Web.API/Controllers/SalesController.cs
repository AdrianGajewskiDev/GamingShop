using GamingShop.Data;
using GamingShop.Data.Models;
using GamingShop.Service;
using GamingShop.Service.Services;
using GamingShop.Web.API.Models;
using GamingShop.Web.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GamingShop.Web.API.Controllers
{
    [ApiController]
    [Route("api/Sales")]
    public class SalesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ApplicationOptions _options;
        private readonly IImage _imageService;
        private readonly ISale _saleService;

        public SalesController(ApplicationDbContext context, IOptions<ApplicationOptions> options,
            IImage image, ISale sale)
        {
            _context = context;
            _options = options.Value;
            _imageService = image;
            _saleService = sale;
        }

        [HttpPost("AddGame")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<int> AddGame(NewGameModel game)
        {
            var userID = User.FindFirst(c => c.Type == "UserID").Value;

            var dayOfLaunch = game.LaunchDate.Split("/")[0];
            var monthOfLaunch = game.LaunchDate.Split("/")[1];
            var yearOfLaunch = game.LaunchDate.Split("/")[2];

            Game newGame = new Game
            {
                DayOfLaunch = dayOfLaunch,
                Description = game.Description,
                ImageUrl = game.ImageUrl,
                MonthOfLaunch = monthOfLaunch,
                Pegi = game.Pegi,
                Platform = game.Platform,
                Price = game.Price,
                Producent = game.Producent,
                Title = game.Title,
                Type = game.Type,
                YearOfLaunch = yearOfLaunch,
                OwnerID = userID,
                Posted = DateTime.UtcNow,
                Sold = false
            };

            await _context.Games.AddAsync(newGame);

            await _context.SaveChangesAsync();

            //return game id to client to pass it to Upload image function
            return newGame.ID;
        }

        [HttpPost("AddGameImage/{id}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> PostGameImage(IFormFile image, int id)
        {
            var userID = User.FindFirst(c => c.Type == "UserID").Value;

            await _imageService.UploadImageAsync(id,image, ImageType.GameCover);

            return Ok();
        }

        [HttpPost("AddUserProfileImage")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> AddUserProfileImage(IFormFile image)
        {
            var userID = User.FindFirst(c => c.Type == "UserID").Value;

            await _imageService.UploadImageAsync(userID, image, ImageType.UserProfile);

            return Ok();
        }

        [HttpGet("UserSales")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IEnumerable<SaleModel>> GetUserSale()
        {
            var userID = User.FindFirst(c => c.Type == "UserID").Value;

            List<SaleModel> sales = new List<SaleModel>();

            await Task.Run(() =>
            {
                sales = _saleService.GetUserSales(userID).ToList();
            });

            return sales;
        }
    }
}
