using GamingShop.Data.Models;
using GamingShop.Service;
using GamingShop.Service.Services;
using GamingShop.Web.API.Models;
using GamingShop.Web.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.IO;
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

        public SalesController(ApplicationDbContext context, IOptions<ApplicationOptions> options,
            IImage image)
        {
            _context = context;
            _options = options.Value;
            _imageService = image;
        }

        [HttpPost("AddGame")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<int> AddGame(NewGameModel game)
        {
            var userID = User.FindFirst(c => c.Type == "UserID").Value;

            Game newGame = new Game
            {
                DayOfLaunch = game.LaunchDate.Day.ToString(),
                Description = game.Description,
                ImageUrl = game.ImageUrl,
                MonthOfLaunch = game.LaunchDate.Month.ToString(),
                Pegi = game.Pegi,
                Platform = game.Platform,
                Price = game.Price,
                Producent = game.Producent,
                Title = game.Title,
                Type = game.Type,
                YearOfLaunch = game.LaunchDate.Year.ToString(),
                OwnerID = userID
            };

            await _context.Games.AddAsync(newGame);

            await _context.SaveChangesAsync();

            //return game id to client to pass it to Upload image function
            return newGame.ID;
        }

        [HttpPost("AddImage/{id}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> PostImage(IFormFile image, int id)
        {
            var userID = User.FindFirst(c => c.Type == "UserID").Value;

            var path = _options.ImagesPath;

            var uniqueName = $"{id}_{image.FileName}";

            var filePath = Path.Combine(path, uniqueName);

            if (Directory.Exists(path))
            {
                using (var sr = new FileStream(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(sr);
                }
            }
            else
            {
                return BadRequest("Directory does not exist");
            }

            var img = new Image
            {
                UserID = userID,
                UniqueName = uniqueName,
                Path = filePath,
                GameID = id

            };

            await _imageService.UploadImageAsync(img);

            return Ok();
        }
    }
}
