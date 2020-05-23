using MediatR;

namespace GamingShop.Web.API.MediatR.Commands.Cart
{
    public class RemoveFromCartCommand : IRequest<bool>
    {
        public string UserID { get; private set; }
        public int GameID { get; private set; }

        public RemoveFromCartCommand(string userID, int iD)
        {
            UserID = userID;
            GameID = iD;
        }
    }
}
