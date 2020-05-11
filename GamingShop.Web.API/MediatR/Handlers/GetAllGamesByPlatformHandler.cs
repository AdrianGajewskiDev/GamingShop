using AutoMapper;
using GamingShop.Service;
using GamingShop.Web.API.Controllers;
using GamingShop.Web.API.MediatR.Queries;
using GamingShop.Web.API.Models.Response;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GamingShop.Web.API.MediatR.Handlers
{
    public class GetAllGamesByPlatformHandler : IRequestHandler<GetGamesByPlatformQuery, GamesResponseModel>
    {
        private readonly IGame _gamesService;
        private readonly IMapper _mapper;

        public GetAllGamesByPlatformHandler(IGame gameService, IMapper mapper)
        {
            _gamesService = gameService;
            _mapper = mapper;
        }

        public async Task<GamesResponseModel> Handle(GetGamesByPlatformQuery request, CancellationToken cancellationToken)
        {
            var pcGames = _gamesService.GetAllByPlatform(Platform.PC).Select(game => _mapper.Map<GameIndexResponseModel>(game));
            var xboxOneGames = _gamesService.GetAllByPlatform(Platform.XboxOne).Select(game => _mapper.Map<GameIndexResponseModel>(game));
            var ps4Games = _gamesService.GetAllByPlatform(Platform.Playstation_4).Select(game => _mapper.Map<GameIndexResponseModel>(game));

            var response = new GamesResponseModel
            {
                PCGames = pcGames,
                XboxOneGames = xboxOneGames,
                PlaystationGames = ps4Games
            };

            return response;
        }
    }
}
