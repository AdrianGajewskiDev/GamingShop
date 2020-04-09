using AutoMapper;
using GamingShop.Data.Models;
using GamingShop.Web.API.Models;
using System;

namespace GamingShop.Web.API.Profiles
{
    public class GameProfile : Profile
    {

        public GameProfile()
        {
            this.CreateMap<NewGameModel, Game>();
        }


    }
}
