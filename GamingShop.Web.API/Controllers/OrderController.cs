using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using GamingShop.Data.Models;
using GamingShop.Service;
using GamingShop.Web.API.MediatR.Commands.Order;
using GamingShop.Web.API.MediatR.Queries.Order;
using GamingShop.Web.API.Models;
using GamingShop.Web.Data;
using MediatR;
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
        private readonly IMediator _mediator;

        /// <summary>
        /// Default constructor
        /// </summary>
        public OrderController(IMediator mediator)
        {
            _mediator = mediator;
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
            var userID = User.Claims.First(x => x.Type == "UserID").Value;

            var cmd = new PlaceOrderCommand(id,userID, model);
            var response = await _mediator.Send(cmd);

            if (response)
                return Ok("Order has been successfully placed");

            return BadRequest("Something went wrong while trying to place order");
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

            var query = new GetLatestOrdersQuery(userID);
            var response = await _mediator.Send(query);

            if (response == null)
                return NotFound("Cannot get any available latest orders");

            return Ok(response);
        }

    }
}