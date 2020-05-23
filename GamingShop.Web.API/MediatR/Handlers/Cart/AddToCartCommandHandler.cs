using GamingShop.Data.Models;
using GamingShop.Service;
using GamingShop.Web.API.MediatR.Commands.Cart;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GamingShop.Web.API.MediatR.Handlers.Cart
{
    public class AddToCartCommandHandler : IRequestHandler<AddToCartCommand, bool>
    {
        private readonly IGame _gameService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICart _cartService;
        public AddToCartCommandHandler(IGame gameService, UserManager<ApplicationUser> userManager, ICart cartService)
        {
            _gameService = gameService;
            _userManager = userManager;
            _cartService = cartService;
        }

        public async Task<bool> Handle(AddToCartCommand request, CancellationToken cancellationToken)
        {
            var game = _gameService.GetByID(request.GameID);

            try
            {
                var user = await _userManager.FindByIdAsync(request.UserID);
                _cartService.AddToCart(user.CartID, game);
                return true;
            }
            catch(Exception)
            {
                return false;
            }
         
        }
    }
}
