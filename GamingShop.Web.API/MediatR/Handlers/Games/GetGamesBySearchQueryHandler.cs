using AutoMapper;
using GamingShop.Data.Models;
using GamingShop.Service;
using GamingShop.Web.API.Controllers;
using GamingShop.Web.API.MediatR.Queries;
using GamingShop.Web.API.Models.Response;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GamingShop.Web.API.MediatR.Handlers
{
    public class GetGamesBySearchQueryHandler : IRequestHandler<GetGamesBySearchQueryQuery, List<GameIndexResponseModel>>
    {
        private readonly IGame _gamesService;
        private readonly IMapper _mapper;

        public GetGamesBySearchQueryHandler(IGame gameService, IMapper mapper)
        {
            _gamesService = gameService;
            _mapper = mapper;
        }

        public async Task<List<GameIndexResponseModel>> Handle(GetGamesBySearchQueryQuery request, CancellationToken cancellationToken)
        {
            IList<Game> games = new List<Game>();

            await Task.Run(() =>
            {
                games = _gamesService.GetAllBySearchQuery(request.SearchQuery).ToList();
            });

            List<GameIndexResponseModel> response = new List<GameIndexResponseModel>();

            foreach (var game in games)
            {
                response.Add(_mapper.Map<GameIndexResponseModel>(game));
            }

            return response;
        }
    }
}
