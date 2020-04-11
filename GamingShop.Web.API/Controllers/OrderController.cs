using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
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
        private readonly IMapper _mapper;

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
            IEmailSender sender, IOrder orderService, IMapper mapper)
        {
            _cartService = cartService;
            _gameService = gameService;
            _dbContext = dbContext;
            _userManager = manager;
            _emailSender = sender;
            _orderService = orderService;
            _mapper = mapper;

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
                var itemsOwner = await _userManager.FindByIdAsync(cartItems.First().OwnerID);
                var totalPrice = CalculateTotalPrice(cartItems);

                model.Placed = DateTime.UtcNow;
                model.TotalPrice = totalPrice;
                model.Email = (string.IsNullOrEmpty(model.AlternativeEmailAdress)) ? user.Email : model.AlternativeEmailAdress;
                model.PhoneNumber = (string.IsNullOrEmpty(model.AlternativePhoneNumber)) ? user.PhoneNumber : model.AlternativePhoneNumber;

                var result = _mapper.Map<Order>(model);
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

                var message = new Message
                {
                    Content = $"Ordered items: {gameTitles}",
                    RecipientEmail = itemsOwner.Email,
                    RecipientID = itemsOwner.Id,
                    SenderID = user.Id,
                    Sent = DateTime.UtcNow,
                    Subject = $"A {user.UserName} ordered your game(s)"
                };
                _dbContext.Messages.Add(message);

                await _dbContext.SaveChangesAsync();

                await _emailSender.SendOrderDetailsEmail(model.Email, "Order", cartItems, new Address { Street = model.Street, City = model.City, Country = model.Country, PhoneNumber = model.PhoneNumber }, totalPrice);

                await _emailSender.SendEmail(message);

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
        public async Task<ActionResult<IEnumerable<LatestOrderModel>>> GetLatestOrders()
        {
                var userID = User.Claims.First(c => c.Type == "UserID").Value;
                var user = await _userManager.FindByIdAsync(userID);
                var cardID = user.CartID;
                var latestOrders = _orderService.GetAllByCartID(cardID);

                List<LatestOrderModel> results = new List<LatestOrderModel>();

                foreach (var order in latestOrders)
                {
                    results.Add(_mapper.Map<LatestOrderModel>(order));
                }

                return results;
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