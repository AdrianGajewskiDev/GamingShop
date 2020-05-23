using GamingShop.Web.API.Models;
using MediatR;
using System.Collections.Generic;

namespace GamingShop.Web.API.MediatR.Queries.Order
{
    public class GetLatestOrdersQuery : IRequest<IEnumerable<LatestOrderModel>>
    {
        public string UserID { get; private set; }

        public GetLatestOrdersQuery(string userID)
        {
            UserID = userID;
        }
    }
}
