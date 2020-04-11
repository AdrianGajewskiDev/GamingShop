using AutoMapper;
using GamingShop.Data.Models;
using GamingShop.Web.API.Models.Response;

namespace GamingShop.Web.API.Profiles
{
    public class MessageProfile : Profile
    {
        public MessageProfile()
        {
            this.CreateMap<Message, MessageDetailsResponseModel>().ForMember(mem => mem.Sent, opt => opt.MapFrom(src => src.Sent.ToShortTimeString()));
            this.CreateMap<NewMessageModel, Message>().ForMember(mem => mem.Sent, opt => opt.Ignore());
        }
    }
}
