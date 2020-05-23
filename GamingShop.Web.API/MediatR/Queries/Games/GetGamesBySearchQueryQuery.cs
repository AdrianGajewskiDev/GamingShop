using GamingShop.Web.API.Models.Response;
using MediatR;
using System.Collections.Generic;

namespace GamingShop.Web.API.MediatR.Queries
{
    public class GetGamesBySearchQueryQuery : IRequest<List<GameIndexResponseModel>>
    {
        public string SearchQuery { get; private set; }

        public GetGamesBySearchQueryQuery(string query)
        {
            this.SearchQuery = query;
        }
    }
}
