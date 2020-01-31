using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamingShop.Data.Models;
using GamingShop.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GamingShop.Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICart _cartService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IGame _gameService;
        public CartController(ICart cartService, UserManager<ApplicationUser> userManager, IGame gameService)
        {
            _cartService = cartService;
            _userManager = userManager;
            _gameService = gameService;
        }

        [HttpGet("GetItemsInCart")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IEnumerable<Game>> GetItemsInCart()
        {
            var userID = User.Claims.First(x => x.Type == "UserID").Value;

            var user = await _userManager.FindByIdAsync(userID);

            var games = _cartService.GetGames(user.CartID);

            return games;
        }

        [HttpPost("AddItemToCart")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> AddToCart([FromBody] int ID)
        {
            var userID = User.Claims.First(x => x.Type == "UserID").Value;
            var game = _gameService.GetByID(ID);
            var user = await _userManager.FindByIdAsync(userID);

            _cartService.AddToCart(user.CartID, game);
            
            return new NoContentResult();
        }

        [HttpPost("RemoveFromCart")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> RemoveFromCart([FromBody]int ID)
        {
            var userID = User.Claims.First(x => x.Type == "UserID").Value;
            var user = await _userManager.FindByIdAsync(userID);
            var game = _cartService.GetGames(user.CartID).Where(g => g.ID == ID).First();

            _cartService.RemoveFormCart(user.CartID, game);

            return new NoContentResult();
        }
    }
}