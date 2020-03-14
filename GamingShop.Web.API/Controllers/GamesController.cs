using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GamingShop.Data.Models;
using GamingShop.Web.Data;
using GamingShop.Service;
using GamingShop.Web.API.Models.Response;
using Microsoft.AspNetCore.Identity;
using GamingShop.Service.Services;

namespace GamingShop.Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IGame _gamesService;
        private readonly IImage _imageService;

        public GamesController(ApplicationDbContext context, IGame gameService, UserManager<ApplicationUser> userManager,IImage image)
        {
            _context = context;
            _gamesService = gameService;
            _userManager = userManager;
            _imageService = image;
        }

        // GET: api/Games
        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<GameIndexResponseModel>>> GetGames()
        {
            List<Game> games = new List<Game>();

            await Task.Run(() => 
            {
                games = _gamesService.GetAllAvailable().ToList();
            });

            var response = games.Select(game => new GameIndexResponseModel 
            {
                ID = game.ID,
                ImageUrl =  _imageService.GetImageNameForGame(game.ID),
                Platform = game.Platform,
                Price = game.Price,
                Producent = game.Producent,
                Title = game.Title
            });

            return response.ToArray();
        }


        [HttpGet("Search/{searchQuery}")]
        public async Task<ActionResult<IEnumerable<GameIndexResponseModel>>> GetBySearchQuery(string searchQuery)
        {
            if (searchQuery == string.Empty)
                return NotFound();

            List<Game> games = new List<Game>();

            await Task.Run(() =>
            {
                 games = _gamesService.GetAllBySearchQuery(searchQuery).ToList();
            }); 


            var response = games.Select(game => new GameIndexResponseModel
            {
                ID = game.ID,
                ImageUrl = _imageService.GetImageNameForGame(game.ID),
                Platform = game.Platform,
                Price = game.Price,
                Producent = game.Producent,
                Title = game.Title
            });

            return response.ToArray();
        }

        // GET: api/Games/5
        [HttpGet("GetGame/{id}")]
        public async Task<ActionResult<GameDetailsResponseModel>> GetGame(int id)
        {
            var game = _gamesService.GetByID(id);

            if (game == null)
            {
                return NotFound();
            }

            string ownerUsername = string.Empty;

            if (!string.IsNullOrEmpty(game.OwnerID))
            {
                var owner = await _userManager.FindByIdAsync(game.OwnerID);
                ownerUsername = owner.UserName;

            }
            else
            {
                ownerUsername = "Unknown";
            }

            var respone = new GameDetailsResponseModel
            {
                BestSeller = game.BestSeller,
                Description = game.Description,
                ImageUrl = _imageService.GetImageNameForGame(game.ID),
                LaunchDate = $"{game.DayOfLaunch}/{game.MonthOfLaunch}/{game.YearOfLaunch}",
                OwnerUsername = ownerUsername,
                Pegi = game.Pegi,
                Platform = game.Platform,
                Price = game.Price,
                Producent = game.Producent,
                Title = game.Title,
                Type = game.Type,
                Sold = game.Sold
            };

            return respone;
        }

        // PUT: api/Games/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGame(int id, Game game)
        {
            if (id != game.ID)
            {
                return BadRequest();
            }

            _context.Entry(game).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GameExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("DeleteGame/{id}")]
        public async Task<ActionResult<Game>> DeleteGame(int id)
        {
            var game = await _context.Games.FindAsync(id);
            if (game == null)
            {
                return NotFound();
            }

            _context.Games.Remove(game);
            await _context.SaveChangesAsync();

            return game;
        }

        private bool GameExists(int id)
        {
            return _context.Games.Any(e => e.ID == id);
        }
    }
}
