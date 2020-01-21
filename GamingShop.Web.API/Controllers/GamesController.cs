using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GamingShop.Data.Models;
using GamingShop.Web.Data;
using GamingShop.Service;

namespace GamingShop.Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IGame _gamesService;

        public GamesController(ApplicationDbContext context, IGame gameService)
        {
            _context = context;
            _gamesService = gameService;
        }

        // GET: api/Games
        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<Game>>> GetGames()
        {
            return await _context.Games.ToListAsync();
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
        public async Task<ActionResult<Game>> GetGame(int id)
        {
            var game = _gamesService.GetByID(id);

            if (game == null)
            {
                return NotFound();
            }

            return game;
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

        // POST: api/Games/AddGame
        [HttpPost("AddGame")]
        public async Task<ActionResult<Game>> PostGame(Game game)
        {
            _context.Games.Add(game);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGame", new { id = game.ID }, game);
        }

        // DELETE: api/Games/5
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
