using GamingShop.Web.API.Models;
using GamingShop.Web.API.Models.Response;
using MediatR;

namespace GamingShop.Web.API.MediatR.Commands.Sales
{
    public class AddGameCommand : IRequest<AddGameResponseModel>
    {
        public NewGameModel NewGameModel { get; private set; }
        public string UserID { get; set; }

        public AddGameCommand(NewGameModel model,string userID)
        {
            NewGameModel = model;
            UserID = userID;
        }
    }
}
