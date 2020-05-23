using GamingShop.Data.Models;
using GamingShop.Web.API.Models;
using MediatR;

namespace GamingShop.Web.API.MediatR.Queries.Games
{
    public class UpdateGameQuery : IRequest<bool>
    {
        public UpdateGameModel UpdateGameModel { get; private set; }
        public string UserID { get; private set; }

        public UpdateGameQuery(UpdateGameModel model, string userID)
        {
            UpdateGameModel = model;
            UserID = userID;
        }
    }
}
