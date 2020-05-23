using GamingShop.Web.API.Models;
using MediatR;

namespace GamingShop.Web.API.MediatR.Commands.Message
{
    public class SendMessageCommand : IRequest<bool>
    {
        public NewMessage Message { get; private set; }

        public SendMessageCommand(NewMessage msg)
        {
            Message = msg;
        }
    }
}
