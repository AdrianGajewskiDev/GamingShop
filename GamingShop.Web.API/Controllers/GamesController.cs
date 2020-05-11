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
using GamingShop.Web.API.Models;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using GamingShop.Web.API.MediatR.Queries;
using MediatR;

namespace GamingShop.Web.API.Controllers
{
    /// <summary>
    /// The controller to handle actions related to games
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IGame _gamesService;
        private readonly IImage _imageService;
        private readonly IMapper _mapper;

        private readonly IMediator _mediator; 


        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="context">A Database context</param>
        /// <param name="gameService">A Game Service</param>
        /// <param name="userManager">A User Manager</param>
        /// <param name="image">A Image Service</param>
        public GamesController(ApplicationDbContext context, IGame gameService, UserManager<ApplicationUser> userManager,IImage image,IMapper mapper, IMediator mediator)
        {
            _context = context;
            _gamesService = gameService;
            _userManager = userManager;
            _imageService = image;
            _mapper = mapper;
            _mediator = mediator;
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
                    return BadRequest("Search query is null");

                List<Game> games = new List<Game>();

                await Task.Run(() =>
                {
                    games = _gamesService.GetAllBySearchQuery(searchQuery).ToList();
                });

                List<GameIndexResponseModel> response = new List<GameIndexResponseModel>();

                foreach (var game in games)
                {
                    response.Add(_mapper.Map<GameIndexResponseModel>(game));
                }

                return response;

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
            var userID = User.FindFirst(c => c.Type == "UserID").Value;

            var dayOfLaunch = game.LaunchDate.Split("/")[0];
            var monthOfLaunch = game.LaunchDate.Split("/")[1];
            var yearOfLaunch = game.LaunchDate.Split("/")[2];

            game.DayOfLaunch = dayOfLaunch;
            game.MonthOfLaunch = monthOfLaunch;
            game.YearOfLaunch = yearOfLaunch;
            game.OwnerID = userID;
            game.ImageUrl = _imageService.GetImageNameForGame(game.ID);

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

            return Ok();
        }

        /// <summary>
        /// Deletes game from store
        /// </summary>
        /// <param name="id">An ID of the game to delete</param>
        /// <returns>Returns the 200 ok status if updating was successfull</returns>
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

                return Ok();


        }

        /// <summary>
        /// Checks if game with <paramref name="id"/> currently exists in store database
        /// </summary>
        /// <param name="id">A game id</param>
        /// <returns>Returns true if game with <paramref name="id"/> already exists in database</returns>
        private bool GameExists(int id)
        {
            return _context.Games.Any(e => e.ID == id);
        }
    }
}
