using GamingShop.Data.Models;
using GamingShop.Web.API.MediatR.Commands.Cart;
using GamingShop.Web.API.MediatR.Queries.Cart;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

using System.Threading.Tasks;

namespace GamingShop.Web.API.Controllers
{
    /// <summary>
    /// The controller to handle user cart actions
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Default constructor
        /// </summary>
        public CartController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// A action to get current items in cart
        /// </summary>
        /// <returns>Returns all items in cart</returns>
        [HttpGet("GetItemsInCart")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<IEnumerable<Game>>> GetItemsInCart()
        {
            string userID = User.Claims.First(x => x.Type == "UserID").Value;
            var query = new GetItemsInCartQuery(userID);
            var response = await _mediator.Send(query);

            if (!response.Any() || response == null)
                return NotFound($"Cart does not contain any items");

            return Ok(response);

        }

        /// <summary>
        /// Adds item to cart
        /// </summary>
        /// <param name="ID">An ID of the item to add</param>
        /// <returns>Return 200 OK result if adding was successful</returns>
        [HttpPost("AddItemToCart/{ID}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> AddToCart(int ID)
        {
            var userID = User.Claims.First(x => x.Type == "UserID").Value;
            var cmd = new AddToCartCommand(ID, userID);
            var response = await _mediator.Send(cmd);

            if (response)
                return Ok($"Item of ID {ID} has been successfully added to cart.");

            return BadRequest("Something bad has happend while trying to add new item to cart.");

        }

        /// <summary>
        /// Deletes item from cart
        /// </summary>
        /// <param name="ID">An ID of the item to remove</param>
        /// <returns>Return 200 OK result if deleting was successful</returns>
        [HttpPost("RemoveFromCart")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> RemoveFromCart([FromBody]int ID)
        {
            var userID = User.Claims.First(x => x.Type == "UserID").Value;
            var cmd = new RemoveFromCartCommand(userID, ID);
            var response = await _mediator.Send(cmd);

            if (response == true)
                return Ok($"Successfully removed game of id: {ID}");

            return BadRequest("Something bad has happend while trying to remove item from cart.");
        }

        /// <summary>
        /// Returns user cart id
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetCardID")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<int>> GetCardID()
        {
            var userID = User.Claims.First(x => x.Type == "UserID").Value;
            var query = new GetUserCartIDQuery(userID);

            var response = _mediator.Send(query);

            if (response != null)
                return Ok(response);

            return NotFound("Cannot get user cart ID");
        }
    }
}