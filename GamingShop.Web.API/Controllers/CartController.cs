using GamingShop.Data.Models;
using GamingShop.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
        private readonly ICart _cartService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IGame _gameService;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="cartService">A Cart Service</param>
        /// <param name="userManager">A User manager</param>
        /// <param name="gameService">A Game Service</param>
        public CartController(ICart cartService, UserManager<ApplicationUser> userManager, IGame gameService)
        {
            _cartService = cartService;
            _userManager = userManager;
            _gameService = gameService;
        }

        /// <summary>
        /// A action to get current items in cart
        /// </summary>
        /// <returns>Returns all items in cart</returns>
        [HttpGet("GetItemsInCart")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IEnumerable<Game>> GetItemsInCart()
        {
            var userID = User.Claims.First(x => x.Type == "UserID").Value;

            var user = await _userManager.FindByIdAsync(userID);

            var games = _cartService.GetGames(user.CartID);

            return games;
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
            var game = _gameService.GetByID(ID);
            var user = await _userManager.FindByIdAsync(userID);

            _cartService.AddToCart(user.CartID, game);

            return Ok();
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
            var user = await _userManager.FindByIdAsync(userID);
            var game = _cartService.GetGames(user.CartID).Where(g => g.ID == ID).First();

            _cartService.RemoveFormCart(user.CartID, game);

            return Ok();
        }

        /// <summary>
        /// Returns user cart id
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetCardID")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<int> GetCardID()
        {
            var userID = User.Claims.First(x => x.Type == "UserID").Value;
            var user = await _userManager.FindByIdAsync(userID);

            return user.CartID;
        }
    }
}