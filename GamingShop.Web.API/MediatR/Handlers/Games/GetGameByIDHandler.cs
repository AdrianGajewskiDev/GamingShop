using AutoMapper;
using GamingShop.Data.Models;
using GamingShop.Service;
using GamingShop.Web.API.MediatR.Queries;
using GamingShop.Web.API.Models.Response;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Threading;
using System.Threading.Tasks;

namespace GamingShop.Web.API.MediatR.Handlers
{
    public class GetGameByIDHandler : IRequestHandler<GetGameByIDQuery, GameDetailsResponseModel>
    {
        private readonly IGame _gamesService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public GetGameByIDHandler(IGame gamesService, UserManager<ApplicationUser> userManager, IMapper mapper) 
        {
            _gamesService = gamesService;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<GameDetailsResponseModel> Handle(GetGameByIDQuery request, CancellationToken cancellationToken)
        {
            var game = _gamesService.GetByID(request.GameID);

            if (game == null)
            {
                return null;
            }

            string ownerUsername = string.Empty;

            if (!string.IsNullOrEmpty(game.OwnerID))
            {
                var owner = await _userManager.FindByIdAsync(game.OwnerID);
                ownerUsername = owner.UserName;
            }
            else
            {
                ownerUsername = "Unknown";
            }

            var response = _mapper.Map<GameDetailsResponseModel>(game);
            response.OwnerUsername = ownerUsername;

            return response;
        }
    }
}
