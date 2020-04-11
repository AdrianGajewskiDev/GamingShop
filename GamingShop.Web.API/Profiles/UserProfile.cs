using AutoMapper;
using GamingShop.Data.Models;
using GamingShop.Service.Services;
using GamingShop.Web.API.Models;
using GamingShop.Web.API.Models.Response;

namespace GamingShop.Web.API.Profiles
{
    public class UserProfile : Profile
    {
        private IImage _imageService { get; }

        public UserProfile(IImage imageService)
        {
            _imageService = imageService;

            CreateMap<ApplicationUser, ApplicationUserResponseModel>().ForMember(member => member.ImageUrl, opt => opt.MapFrom(src => _imageService.GetImagePathForUser(src.Id)));
            CreateMap<RegisterModel, ApplicationUser>();
        }

    }
}
