using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GamingShop.Data.Models;
using GamingShop.Web.Data;
using GamingShop.Service;
using GamingShop.Web.API.Models;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Data.SqlClient;
using GamingShop.Web.API.Models.Response;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

namespace GamingShop.Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IGame _gamesService;
        private readonly UserManager<ApplicationUser> _userManager;

        public GamesController(ApplicationDbContext context, IGame gameService, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _gamesService = gameService;
            _userManager = userManager;
        }

        // GET: api/Games
        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<Game>>> GetGames()
        {
            var games = await _context.Games.ToListAsync();

            return games;
        }


        [HttpGet("Search/{searchQuery}")]
        public async Task<ActionResult<IEnumerable<Game>>> GetBySearchQuery(string searchQuery)
        {
            if (searchQuery == string.Empty)
                return NotFound();

            var games = _gamesService.GetAllBySearchQuery(searchQuery).ToList();

            return games;
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
                ImageUrl = game.ImageUrl,
                LaunchDate = $"{game.DayOfLaunch}/{game.MonthOfLaunch}/{game.YearOfLaunch}",
                OwnerUsername = ownerUsername,
                Pegi = game.Pegi,
                Platform = game.Platform,
                Price = game.Price,
                Producent = game.Producent,
                Title = game.Title,
                Type = game.Type
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
