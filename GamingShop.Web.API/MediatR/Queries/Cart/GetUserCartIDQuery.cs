using MediatR;

namespace GamingShop.Web.API.MediatR.Queries.Cart
{
    public class GetUserCartIDQuery : IRequest<int>
    {
        public string UserID { get; private set; }

        public GetUserCartIDQuery(string userID)
        {
            UserID = userID;
        }
    }
}
