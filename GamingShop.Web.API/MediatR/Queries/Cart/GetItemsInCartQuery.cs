using GamingShop.Data.Models;
using MediatR;
using System.Collections.Generic;


namespace GamingShop.Web.API.MediatR.Queries.Cart
{
    public class GetItemsInCartQuery : IRequest<IEnumerable<Game>>
    {
        public string UserID { get; private set; }

        public GetItemsInCartQuery(string userID)
        {
            UserID = userID;
        }
    }
}
