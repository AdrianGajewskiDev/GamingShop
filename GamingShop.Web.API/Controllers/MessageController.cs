using GamingShop.Data.Models;
using GamingShop.Service.Services;
using GamingShop.Web.API.MediatR.Commands.Message;
using GamingShop.Web.API.MediatR.Queries.Message;
using GamingShop.Web.API.Models;
using GamingShop.Web.API.Models.Response;
using GamingShop.Web.API.Pagination;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace GamingShop.Web.API.Controllers
{
    /// <summary>
    /// A controller to handle messages between users
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : Controller
    {
        private readonly IMessage _messageService;
        private readonly IMediator _mediator;

        /// <summary>
        /// Default constructor
        /// </summary>
        public MessageController(IMediator mediator, IMessage service)
        {
            _messageService = service;
            _mediator = mediator;
        }

        /// <summary>
        /// Sends a message to specified user
        /// </summary>
        /// <param name="message">A message model containig all message details</param>
        /// <returns>200 ok result if message was sent successfully</returns>
        [HttpPost("[action]")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> SendMessage([FromBody]NewMessage message)
        {
            var cmd = new SendMessageCommand(message);
            var response = await _mediator.Send(cmd);

            if (response == true)
                return Ok("Message has been sent successfully");

            return BadRequest("Something bad has happened while trying to send message");

        }

        /// <summary>
        /// Gets all messages for specified user
        /// </summary>
        /// <returns>All messages that belongs to the user</returns>
        [HttpGet("GetMessages/{by}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public ActionResult<PaginatedResponseModel<Message>> GetMessages([FromQuery]PaginationParams param, string by)
        {
            var userID = User.FindFirst(x => x.Type == "UserID").Value;

            PaginatedResponse<Message> messages;

            switch (by)
            {
                case "by":
                {
                        messages = new PaginatedResponse<Message>(_messageService.GetAllSentByUser(userID).OrderByDescending(x => x.Sent), param.PageSize, param.PageIndex);   
                }break;
                case "to":
                    {
                        messages = new PaginatedResponse<Message>(_messageService.GetAllSentToUser(userID).OrderByDescending(x => x.Sent), param.PageSize, param.PageIndex);
                    }
                    break;
                default:
                    messages = new PaginatedResponse<Message>(_messageService.GetAllSentByUser(userID).OrderByDescending(x => x.Sent), param.PageSize, param.PageIndex);
                    break;
            }

            var response = new PaginatedResponseModel<Message> 
            {
                Items = messages.GetResult(),
                ResponseInfo = messages.GetResponseInfo()
            };

            return response;
        }

        /// <summary>
        /// Gets a message details
        /// </summary>
        /// <param name="id">An id of the message to get</param>
        /// <returns>Returns message details</returns>
        [HttpGet("ByID/{id}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<MessageDetailsResponseModel>> GetByMessageByID(int id)
        {
            var query = new GetMessageByIDQuery(id);
            var response = await _mediator.Send(query);

            if(response != null)
                return Ok(response);

            return NotFound($"Cannot find message with id of{id}");
        }

    }
}
