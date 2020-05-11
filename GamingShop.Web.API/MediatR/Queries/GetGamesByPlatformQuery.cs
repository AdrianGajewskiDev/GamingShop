using GamingShop.Web.API.Models.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamingShop.Web.API.MediatR.Queries
{
    public class GetGamesByPlatformQuery : IRequest<GamesResponseModel>
    {
        
    }
}
