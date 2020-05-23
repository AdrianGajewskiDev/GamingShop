using GamingShop.Data.Models;
using GamingShop.Service;
using GamingShop.Web.API.MediatR.Commands.Cart;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GamingShop.Web.API.MediatR.Handlers.Cart
{
    public class RemoveFromCartCommandHandler : IRequestHandler<RemoveFromCartCommand, bool>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICart _cartService;

        public RemoveFromCartCommandHandler(UserManager<ApplicationUser> userManager, ICart cartService)
        {
            _userManager = userManager;
            _cartService = cartService;
        }

        public async Task<bool> Handle(RemoveFromCartCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserID);

            try
            {
                var game = _cartService.GetGames(user.CartID).Where(g => g.ID == request.GameID).First();

                _cartService.RemoveFormCart(user.CartID, game);

                return true;
            }
            catch(Exception)
            {
                return false;
            }
      
        }
    }
}
