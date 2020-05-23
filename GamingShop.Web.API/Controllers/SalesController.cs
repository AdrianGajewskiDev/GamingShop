using GamingShop.Data;
using GamingShop.Service;
using GamingShop.Web.API.MediatR.Commands.Sales;
using GamingShop.Web.API.MediatR.Queries.Sales;
using GamingShop.Web.API.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System.Collections.Generic;
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
        private readonly IMediator _mediator;

        /// <summary>
        /// Default constructor
        /// </summary>
        public SalesController(IMediator mediator)
        {
            _mediator = mediator;
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
            var userID = User.FindFirst(c => c.Type == "UserID").Value;
            var cmd = new AddGameCommand(game, userID);
            var response = await _mediator.Send(cmd);

            if (response != null)
                return Created(response.Link, response.GameID);

            return BadRequest("Something went wrong while trying to add new game to database");

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
            var userID = User.FindFirst(c => c.Type == "UserID").Value;

            var cmd = new UploadImageCommand(image, id, ImageType.GameCover);
            var response = await _mediator.Send(cmd);

            if(response == true)
                return Ok();

            return BadRequest("Something bad has happened while trying to upload image");
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
            var userID = User.FindFirst(c => c.Type == "UserID").Value;
            var cmd = new UploadImageCommand(image, userID, ImageType.UserProfile);
            var response = await _mediator.Send(cmd);

            if (response == true)
                return Ok();

            return BadRequest("Something bad has happened while trying to upload image");
        }

        /// <summary>
        /// Gets all user currently available sales
        /// </summary>
        /// <returns>Returns list of user sales</returns>
        [HttpGet("UserSales")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<IEnumerable<SaleModel>>> GetUserSale()
        {
            var userID = User.FindFirst(c => c.Type == "UserID").Value;
            var query = new GetUserSalesQuery(userID);
            var response = await _mediator.Send(query);

            if (response != null)
                return Ok(response);

            return NotFound("Cannot get any available user sale");
        }
    }
}
