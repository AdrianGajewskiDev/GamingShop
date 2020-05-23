using GamingShop.Web.API.Models;
using MediatR;

namespace GamingShop.Web.API.MediatR.Commands.Order
{
    public class PlaceOrderCommand : IRequest<bool>
    {
        public int UserCartID { get; private set; }
        public string UserID { get; private set; }
        public OrderModel OrderModel { get; private set; }

        public PlaceOrderCommand(int cartID, string userID, OrderModel model)
        {
            UserCartID = cartID;
            OrderModel = model;
            UserID = userID;
        }
    }
}
