using AutoMapper;
using GamingShop.Data.Models;
using GamingShop.Service;
using GamingShop.Web.API.Models;
using System;

namespace GamingShop.Web.API.Profiles
{
    public class OrderProfile : Profile
    {
        private readonly IOrder _orderService;

        public OrderProfile(IOrder orderService)
        {
            _orderService = orderService;

            CreateMap<OrderModel, Order>();
            CreateMap<Order, LatestOrderModel>().ForMember(mem => mem.Games, opt => opt.MapFrom(src => orderService.GetAllByCartID(src.CartID)))
                .ForMember(mem => mem.Price, opt => opt.MapFrom(src => src.TotalPrice))
                .ForMember(mem => mem.Placed, opt => opt.MapFrom(src => src.Placed.ToShortDateString()));
        }
    }
}
