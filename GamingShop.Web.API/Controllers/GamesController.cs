using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GamingShop.Data.Models;
using GamingShop.Web.Data;
using GamingShop.Web.API.Models.Response;
using GamingShop.Web.API.Models;
using Microsoft.AspNetCore.Authorization;
using GamingShop.Web.API.MediatR.Queries;
using MediatR;
using GamingShop.Web.API.MediatR.Queries.Games;
using GamingShop.Data.DbContext;

namespace GamingShop.Web.API.Controllers
{
    /// <summary>
    /// The controller to handle actions related to games
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private ApplicationDbContext _context;
        private readonly IMediator _mediator;
        private readonly ApplicationDbContextFactory _applicationDbContextFactory;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="context">A Database context</param>
        public GamesController(ApplicationDbContext context, IMediator mediator, ApplicationDbContextFactory applicationDbContextFactory)
        {
            _mediator = mediator;
            _applicationDbContextFactory = applicationDbContextFactory;
        }

        /// <summary>
        /// Gets all available games in store database
        /// </summary>
        /// <returns>Array of <see cref="GameIndexResponseModel"/></returns>
        [HttpGet("GetAll")]
        public async Task<ActionResult<GamesResponseModel>> GetGames()
        {
            var query = new GetGamesByPlatformQuery();
            var response = await _mediator.Send(query);

            if (response == null)
                return NotFound("Cannot get any available game");

            return Ok(response);
        }

        /// <summary>
        /// Gets games corresponding to search query
        /// </summary>
        /// <param name="searchQuery">A search query</param>
        /// <returns>Array of games where title, producent or platform matches the <paramref name="searchQuery"/></returns>
        [HttpGet("Search/{searchQuery}")]
        public async Task<ActionResult<IEnumerable<GameIndexResponseModel>>> GetBySearchQuery(string searchQuery)
        {
            if (searchQuery == string.Empty)
                return BadRequest("Search query is empty");

            var query = new GetGamesBySearchQueryQuery(searchQuery);
            var response = await _mediator.Send(query);

            if (response == null)
                return NotFound($"Cannot find game with {searchQuery} query" );

            return Ok(response);

        }

        /// <summary>
        /// Gets game corresponding to <paramref name="id"/>
        /// </summary>
        /// <param name="id">An ID of the game to get</param>
        /// <returns>Returns the <see cref="GameDetailsResponseModel"/> game details</returns>
        [HttpGet("GetGame/{id}")]
        public async Task<ActionResult<GameDetailsResponseModel>> GetGame(int id)
        {
            var query = new GetGameByIDQuery(id);
            var response = await _mediator.Send(query);

            if (response == null)
                return NotFound($"Game with id of {id} does not exist!!!");

            return response;

        }

        /// <summary>
        /// Updates the game
        /// </summary>
        /// <param name="id">An ID of the game to update</param>
        /// <param name="game">An new values for game to update</param>
        /// <returns>Returns the 200 ok status if updating was successful</returns>
        [HttpPut("UpdateGame/{id}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> PutGame(int id, [FromBody] UpdateGameModel game)
        {
            if (id != game.ID)
            {
                return BadRequest();
            }

            string userID = User.FindFirst(c => c.Type == "UserID").Value;

            var query = new UpdateGameQuery(game, userID);
            var result = await _mediator.Send(query);

            if (result == true)
                return Ok("Successfully updated");

            return BadRequest("Something went wrong while trying to update your game");
        }

        /// <summary>
        /// Deletes game from store
        /// </summary>
        /// <param name="id">An ID of the game to delete</param>
        /// <returns>Returns the 200 ok status if updating was successfull</returns>
        [HttpDelete("DeleteGame/{id}")]
        public async Task<ActionResult<Game>> DeleteGame(int id)
        {
            using(_context = _applicationDbContextFactory.CreateDbContext())
            {
                var game = await _context.Games.FindAsync(id);
                if (game == null)
                {
                    return NotFound();
                }

                _context.Games.Remove(game);
                await _context.SaveChangesAsync();

                return Ok();
            }
        }

        /// <summary>
        /// Checks if game with <paramref name="id"/> currently exists in store database
        /// </summary>
        /// <param name="id">A game id</param>
        /// <returns>Returns true if game with <paramref name="id"/> already exists in database</returns>
        private bool GameExists(int id)
        {
            using(_context = _applicationDbContextFactory.CreateDbContext())
            {
                return _context.Games.Any(e => e.ID == id);

            }
        }
    }
}
