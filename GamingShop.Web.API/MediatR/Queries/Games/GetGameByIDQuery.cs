using GamingShop.Web.API.Models.Response;
using MediatR;

namespace GamingShop.Web.API.MediatR.Queries
{
    public class GetGameByIDQuery : IRequest<GameDetailsResponseModel>
    {
        public int GameID { get; private set; }

        public GetGameByIDQuery(int gameId)
        {
            GameID = gameId;
        }
    }
}
