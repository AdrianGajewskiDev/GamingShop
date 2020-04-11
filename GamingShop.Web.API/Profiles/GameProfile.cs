using AutoMapper;
using GamingShop.Data.Models;
using GamingShop.Service.Services;
using GamingShop.Web.API.Models;
using GamingShop.Web.API.Models.Response;
using System;

namespace GamingShop.Web.API.Profiles
{
    public class GameProfile : Profile
    {
        private readonly IImage _imageService;


        public GameProfile(IImage imageService)
        {
            _imageService = imageService;

            this.CreateMap<NewGameModel, Game>();
            this.CreateMap<Game, GameIndexResponseModel>().ForMember(mem => mem.ImageUrl, opt => opt.MapFrom(src => _imageService.GetImageNameForGame(src.ID)));
            this.CreateMap<Game, GameDetailsResponseModel>().ForMember(mem => mem.ImageUrl, opt => opt.MapFrom(src => _imageService.GetImageNameForGame(src.ID)))
                .ForMember(mem => mem.OwnerUsername, opt => opt.Ignore())
                .ForMember(mem => mem.LaunchDate, opt => opt.MapFrom(src => $"{src.DayOfLaunch}/{src.MonthOfLaunch}/{src.YearOfLaunch}"));

        }

    }
}
