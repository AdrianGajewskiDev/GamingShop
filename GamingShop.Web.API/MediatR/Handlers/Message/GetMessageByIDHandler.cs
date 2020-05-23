using AutoMapper;
using GamingShop.Service.Services;
using GamingShop.Web.API.MediatR.Queries.Message;
using GamingShop.Web.API.Models.Response;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace GamingShop.Web.API.MediatR.Handlers.Message
{
    public class GetMessageByIDHandler : IRequestHandler<GetMessageByIDQuery, MessageDetailsResponseModel>
    {
        private readonly IMessage _messageService;
        private readonly IMapper _mapper;

        public GetMessageByIDHandler(IMessage messageService, IMapper mapper)
        {
            _messageService = messageService;
            _mapper = mapper;
        }

        public async Task<MessageDetailsResponseModel> Handle(GetMessageByIDQuery request, CancellationToken cancellationToken)
        {
            var result = await _messageService.GetByIDAsync(request.ID);

            if (result == null)
                return null;

            var response = _mapper.Map<MessageDetailsResponseModel>(result);

            return response;

        }
    }
}
