using GamingShop.Data;
using MediatR;
using System.Collections.Generic;

namespace GamingShop.Web.API.MediatR.Queries.Sales
{
    public class GetUserSalesQuery : IRequest<IEnumerable<SaleModel>>
    {
        public GetUserSalesQuery(string userID)
        {
            UserID = userID;
        }

        public string UserID { get; private set; }
    }
}
