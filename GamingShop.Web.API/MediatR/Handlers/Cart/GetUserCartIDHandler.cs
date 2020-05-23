using GamingShop.Data.Models;
using GamingShop.Web.API.MediatR.Queries.Cart;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Threading;
using System.Threading.Tasks;

namespace GamingShop.Web.API.MediatR.Handlers.Cart
{
    public class GetUserCartIDHandler : IRequestHandler<GetUserCartIDQuery, int>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public GetUserCartIDHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<int> Handle(GetUserCartIDQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserID);

            return user.CartID;
        }
    }
}
