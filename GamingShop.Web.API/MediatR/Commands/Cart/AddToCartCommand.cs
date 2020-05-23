using MediatR;

namespace GamingShop.Web.API.MediatR.Commands.Cart
{
    public class AddToCartCommand : IRequest<bool>
    {
        public int GameID { get; private set; }
        public string UserID { get; private set; }

        public AddToCartCommand(int gameID, string userID)
        {
            GameID = gameID;
            UserID = userID;
        }
    }
}
