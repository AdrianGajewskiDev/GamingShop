using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using GamingShop.Data.Models;
using GamingShop.Service;
using GamingShop.Web.API.Models;
using GamingShop.Web.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GamingShop.Web.API.Controllers
{
    /// <summary>
    /// The controller to handle order requests
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ICart _cartService;
        private readonly IGame _gameService;
        private IEmailSender _emailSender;
        private IOrder _orderService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _dbContext;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="cartService">A cart service</param>
        /// <param name="gameService">A game service </param>
        /// <param name="dbContext">A Database context</param>
        /// <param name="manager">A user manager</param>
        /// <param name="sender">A email sender</param>
        /// <param name="orderService">A order service</param>
        public OrderController(ICart cartService, IGame gameService,
            ApplicationDbContext dbContext, UserManager<ApplicationUser> manager,
            IEmailSender sender, IOrder orderService)
        {
            _cartService = cartService;
            _gameService = gameService;
            _dbContext = dbContext;
            _userManager = manager;
            _emailSender = sender;
            _orderService = orderService;
        }
        
        /// <summary>
        /// Places new order
        /// </summary>
        /// <param name="id">An ID of the user cart</param>
        /// <param name="model">A model containing order details</param>
        /// <returns>Returns 200 ok status if order was successfully placed</returns>
        [HttpPut("PlaceOrder/{id}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> PlaceOrder(int id, [FromBody] OrderModel model)
        {
            IEnumerable<Game> cartItems = _cartService.GetGames(id);

            var userID = User.Claims.First(x => x.Type == "UserID").Value;

            var user = await _userManager.FindByIdAsync(userID);

            var email = (string.IsNullOrEmpty(model.AlternativeEmailAdress)) ? user.Email : model.AlternativeEmailAdress;

            var phoneNumber = (string.IsNullOrEmpty(model.AlternativePhoneNumber)) ? user.PhoneNumber : model.AlternativePhoneNumber;

            var totalPrice = CalculateTotalPrice(cartItems);

            var result = await _dbContext.Orders.AddAsync(new Order
            {
                CartID = id,
                City = model.City,
                Country = model.Country,
                Email = email,
                PhoneNumber = phoneNumber,
                Street = model.Street,
                TotalPrice = totalPrice,
                Placed = DateTime.Now,
            });

            await _dbContext.SaveChangesAsync();

            var orderID = _dbContext.Orders.Last().ID;

            foreach (var item in cartItems)
            {
                _dbContext.OrderItems.Add(new OrderItem
                {
                    GameID = item.ID,
                    CartID = user.CartID,
                    OrderID = orderID
                });
            }

            await _emailSender.SendOrderDetailsEmail(email, "Order",cartItems, new Address { Street = model.Street, City = model.City, Country = model.Country, PhoneNumber = phoneNumber }, totalPrice);

            await _cartService.ClearCart(id);

            await _orderService.MarkGameAsSold(cartItems);

            return Ok();
        }

        /// <summary>
        /// Gets all user latest orders
        /// </summary>
        /// <returns>Returns an Array of <see cref="LatestOrderModel" /> items</returns>
        [HttpGet("LatestOrders")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IEnumerable<LatestOrderModel>> GetLatestOrders()
        {
            var userID = User.Claims.First(c => c.Type == "UserID").Value;
            var user = await _userManager.FindByIdAsync(userID);
            var cardID = user.CartID;
            var latestOrders = _orderService.GetAllByCartID(cardID);

            var orders = latestOrders.Select(order => new LatestOrderModel 
            {
                Placed = order.Placed.ToShortDateString(),
                City =  order.City,
                Street = order.Street,
                Games = _orderService.GetGamesFromOrder(order.ID),
                Price = order.TotalPrice
            });

            return orders.ToArray();
        }

        /// <summary>
        /// Method to calculate total price
        /// </summary>
        /// <param name="games">A list of games in user cart</param>
        /// <returns>Total price of all items in cart</returns>
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