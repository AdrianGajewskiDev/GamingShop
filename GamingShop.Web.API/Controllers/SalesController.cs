using AutoMapper;
using GamingShop.Data;
using GamingShop.Data.Models;
using GamingShop.Service;
using GamingShop.Service.Services;
using GamingShop.Web.API.Models;
using GamingShop.Web.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GamingShop.Web.API.Controllers
{
    /// <summary>
    /// A controller to handle user sales actions
    /// </summary>
    [ApiController]
    [Route("api/Sales")]
    public class SalesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ApplicationOptions _options;
        private readonly IImage _imageService;
        private readonly ISale _saleService;
        private IMapper _mapper;
        private readonly LinkGenerator _generator;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="context">A Database context</param>
        /// <param name="options">Application Options</param>
        /// <param name="image">A Image service</param>
        /// <param name="sale">A Sale Service</param>
        public SalesController(ApplicationDbContext context, IOptions<ApplicationOptions> options,
            IImage image, ISale sale, IMapper mapper, LinkGenerator generator)
        {
            _context = context;
            _options = options.Value;
            _imageService = image;
            _saleService = sale;
            _mapper = mapper;
            _generator = generator;
        }

        /// <summary>
        /// Adds new game item to database
        /// </summary>
        /// <param name="game">A game to add</param>
        /// <returns>Returns game id to client to pass it to Upload image function</returns>
        [HttpPost("AddGame")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<int>> AddGame(NewGameModel game)
        {
            try
            {
                var userID = User.FindFirst(c => c.Type == "UserID").Value;

                var result = _mapper.Map<Game>(game);

                result.DayOfLaunch = game.LaunchDate.Split("/")[0];
                result.MonthOfLaunch = game.LaunchDate.Split("/")[1];
                result.YearOfLaunch = game.LaunchDate.Split("/")[2];
                result.OwnerID = userID;

                var link = _generator.GetPathByAction("AddGame", "Sales");
                await _context.Games.AddAsync(result);

                await _context.SaveChangesAsync();

                return Created(link, result.ID);
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Something bad happened on server: {ex.Message}");
            }
        }

        /// <summary>
        /// Adds game image to database and copy file to specified folder
        /// </summary>
        /// <param name="image">File to add</param>
        /// <param name="id">An ID of the game that file belongs to</param>
        /// <returns>200 ok status if uploading was successfull</returns>
        [HttpPost("AddGameImage/{id}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> PostGameImage(IFormFile image, int id)
        {
            try
            {
                var userID = User.FindFirst(c => c.Type == "UserID").Value;

                await _imageService.UploadImageAsync(id, image, ImageType.GameCover);

                return Ok();
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Something bad happened on server: {ex.Message}");
            }
        }

        /// <summary>
        /// Adds user profile image to database and copy file to specified folder
        /// </summary>
        /// <param name="image">File to add</param>
        /// <param name="id">An ID of the user that file belongs to</param>
        /// <returns>200 ok status if uploading was successfull</returns>
        [HttpPost("AddUserProfileImage")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> AddUserProfileImage(IFormFile image)
        {
            try
            {
                var userID = User.FindFirst(c => c.Type == "UserID").Value;

                await _imageService.UploadImageAsync(userID, image, ImageType.UserProfile);

                return Ok();
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Something bad happened on server: {ex.Message}");
            }

        }

        /// <summary>
        /// Gets all user currently available sales
        /// </summary>
        /// <returns>Returns list of user sales</returns>
        [HttpGet("UserSales")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<IEnumerable<SaleModel>>> GetUserSale()
        {
            try
            {
                var userID = User.FindFirst(c => c.Type == "UserID").Value;

                List<SaleModel> sales = new List<SaleModel>();

                await Task.Run(() =>
                {
                    sales = _saleService.GetUserSales(userID).ToList();
                });

                return sales;

            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Something bad happened on server: {ex.Message}");
            }

        }
    }
}
