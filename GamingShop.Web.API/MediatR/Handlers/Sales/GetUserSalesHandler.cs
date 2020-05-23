using GamingShop.Data;
using GamingShop.Service.Services;
using GamingShop.Web.API.MediatR.Queries.Sales;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GamingShop.Web.API.MediatR.Handlers.Sales
{
    public class GetUserSalesHandler : IRequestHandler<GetUserSalesQuery, IEnumerable<SaleModel>>
    {
        private readonly ISale _saleService;

        public GetUserSalesHandler(ISale saleService)
        {
            _saleService = saleService;
        }

        public async Task<IEnumerable<SaleModel>> Handle(GetUserSalesQuery request, CancellationToken cancellationToken)
        {
            List<SaleModel> sales = new List<SaleModel>();

            await Task.Run(() =>
            {
                sales = _saleService.GetUserSales(request.UserID).ToList();
            });

            return sales;
        }
    }
}
