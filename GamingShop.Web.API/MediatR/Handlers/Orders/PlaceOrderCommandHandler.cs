using AutoMapper;
using GamingShop.Data.DbContext;
using GamingShop.Data.Models;
using GamingShop.Service;
using GamingShop.Web.API.MediatR.Commands.Order;
using GamingShop.Web.Data;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GamingShop.Web.API.MediatR.Handlers.Orders
{
    public class PlaceOrderCommandHandler : IRequestHandler<PlaceOrderCommand, bool>
    {
        private readonly ICart _cartService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private ApplicationDbContext _dbContext;
        private readonly ApplicationDbContextFactory _dbContextFactory;
        private readonly IEmailSender _emailSender;
        private readonly IOrder _orderService;
        public PlaceOrderCommandHandler(ICart cart, UserManager<ApplicationUser> userManager, IMapper mapper, ApplicationDbContextFactory factory, IEmailSender emailSender, IOrder orderService)
        {
            _cartService = cart;
            _userManager = userManager;
            _mapper = mapper;
            _dbContextFactory = factory;
            _dbContext = _dbContextFactory.CreateDbContext();
            _emailSender = emailSender;
            _orderService = orderService;

        }

        public async Task<bool> Handle(PlaceOrderCommand request, CancellationToken cancellationToken)
        {

            IEnumerable<Game> cartItems = _cartService.GetGames(request.UserCartID);

            var user = await _userManager.FindByIdAsync(request.UserID);
            var itemsOwner = await _userManager.FindByIdAsync(cartItems.First().OwnerID);
            var totalPrice = CalculateTotalPrice(cartItems);

           request.OrderModel.Placed = DateTime.UtcNow;
           request.OrderModel.TotalPrice = totalPrice;
           request.OrderModel.Email = (string.IsNullOrEmpty(request.OrderModel.AlternativeEmailAdress)) ? user.Email : request.OrderModel.AlternativeEmailAdress;
           request.OrderModel.PhoneNumber = (string.IsNullOrEmpty(request.OrderModel.AlternativePhoneNumber)) ? user.PhoneNumber : request.OrderModel.AlternativePhoneNumber;

            var result = _mapper.Map<Order>(request.OrderModel);
            var orderID = _dbContext.Orders.Last().ID;

            string gameTitles = string.Empty;

            foreach (var item in cartItems)
            {
                gameTitles = gameTitles.Concat(item.Title).ToString();
                _dbContext.OrderItems.Add(new OrderItem
                {
                    GameID = item.ID,
                    CartID = user.CartID,
                    OrderID = orderID
                });
            }

            var message = new GamingShop.Data.Models.Message
            {
                Content = $"Ordered items: {gameTitles}",
                RecipientEmail = itemsOwner.Email,
                RecipientID = itemsOwner.Id,
                SenderID = user.Id,
                Sent = DateTime.UtcNow,
                Subject = $"A {user.UserName} ordered your game(s)"
            };
            _dbContext.Messages.Add(message);

            await _emailSender.SendOrderDetailsEmail(request.OrderModel.Email, "Order", cartItems, new Address { Street = request.OrderModel.Street, City = request.OrderModel.City, Country = request.OrderModel.Country, PhoneNumber = request.OrderModel.PhoneNumber }, totalPrice);

            await _emailSender.SendEmail(message);

            await _cartService.ClearCart(request.UserCartID);

            await _orderService.MarkGameAsSold(cartItems);

            if (await _dbContext.SaveChangesAsync() > 0)
                return true;

            return false;

        }

        decimal CalculateTotalPrice(IEnumerable<Game> games)
        {
            decimal price = 0;
            foreach (var item in games)
            {
                price += item.Price;
            }

            return price;
        }
    }
}
