using AutoMapper;
using GamingShop.Data.Models;
using GamingShop.Service;
using GamingShop.Web.API.MediatR.Queries.Order;
using GamingShop.Web.API.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GamingShop.Web.API.MediatR.Handlers.Orders
{
    public class GetLatestOrdersHandler : IRequestHandler<GetLatestOrdersQuery, IEnumerable<LatestOrderModel>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IOrder _orderService;
        private readonly IMapper _mapper;

        public GetLatestOrdersHandler(IOrder orderService, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _orderService = orderService;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async  Task<IEnumerable<LatestOrderModel>> Handle(GetLatestOrdersQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserID);
            var cardID = user.CartID;
            var latestOrders = _orderService.GetAllByCartID(cardID);

            List<LatestOrderModel> results = new List<LatestOrderModel>();

            foreach (var order in latestOrders)
            {
                results.Add(_mapper.Map<LatestOrderModel>(order));
            }

            return results;
        }
    }
}
