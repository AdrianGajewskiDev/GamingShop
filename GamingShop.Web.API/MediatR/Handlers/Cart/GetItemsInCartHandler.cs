using GamingShop.Data.Models;
using GamingShop.Service;
using GamingShop.Web.API.MediatR.Queries.Cart;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GamingShop.Web.API.MediatR.Handlers
{
    public class GetItemsInCartHandler : IRequestHandler<GetItemsInCartQuery, IEnumerable<Game>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICart _cartService;

        public GetItemsInCartHandler(UserManager<ApplicationUser> userManager, ICart cartService)
        {
            _userManager = userManager;
            _cartService = cartService;
        }

        public async Task<IEnumerable<Game>> Handle(GetItemsInCartQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserID);
            var games = _cartService.GetGames(user.CartID);

            return games;
        }
    }
}
