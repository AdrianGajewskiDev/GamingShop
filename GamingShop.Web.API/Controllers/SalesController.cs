using GamingShop.Data.Models;
using GamingShop.Service;
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

        public SalesController(ApplicationDbContext context, IOptions<ApplicationOptions> options)
        {
            _context = context;
            _options = options.Value;
        }

        [HttpPost("AddGame")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> AddGame(NewGameModel game)
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

            return Ok();
        }

        [HttpPost("AddImage")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> PostImage(IFormFile image)
        {
            var userID = User.FindFirst(c => c.Type == "UserID").Value;

            var path = _options.WebRootPath + "\\Images";

            var uniqueName = $"{userID}_{image.FileName}";

            var fileName = uniqueName + Path.GetExtension(image.FileName);

            var filePath = Path.Combine(path, fileName);

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
                Path = filePath

            };

            return Ok();
        }
    }
}
