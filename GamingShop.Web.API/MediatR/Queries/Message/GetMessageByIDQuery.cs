using GamingShop.Web.API.Models.Response;
using MediatR;

namespace GamingShop.Web.API.MediatR.Queries.Message
{
    public class GetMessageByIDQuery : IRequest<MessageDetailsResponseModel>
    {
        public int ID { get; private set; }

        public GetMessageByIDQuery(int id)
        {
            ID = id;
        }
    }
}
