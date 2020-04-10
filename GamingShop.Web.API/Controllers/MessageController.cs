using GamingShop.Data.Models;
using GamingShop.Service;
using GamingShop.Service.Services;
using GamingShop.Web.API.Models;
using GamingShop.Web.API.Models.Response;
using GamingShop.Web.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
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
        private readonly ApplicationDbContext _dbContext;
        private readonly IMessage _messageService;
        private readonly IGame _gameService;
        private readonly UserManager<ApplicationUser> _userManager;

        /// <summary>
        /// Default constructor
        /// </summary>
        public MessageController(ApplicationDbContext context, IMessage service, 
            IGame gameService, UserManager<ApplicationUser> userManager)
        {
            _dbContext = context;
            _messageService = service;
            _gameService = gameService;
            _userManager = userManager;
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
            try
            {
                if (message == null)
                    return BadRequest("Invalid message model");

                var recipientID = _gameService.GetByID(message.GameID).OwnerID;
                var recipient = await _userManager.FindByIdAsync(recipientID);
                var sender = await _userManager.FindByIdAsync(message.SenderID);

                var msg = new Message
                {
                    Content = message.Content,
                    Read = false,
                    RecipientEmail = recipient.Email,
                    RecipientID = recipientID,
                    SenderID = message.SenderID,
                    Sent = DateTime.UtcNow,
                    Subject = message.Subject,
                    SenderEmail = sender.Email

                };


                _dbContext.Messages.Add(msg);

                await _dbContext.SaveChangesAsync();

                return Ok();


            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Something bad happened on server: {ex.Message}");
            }

        }

        /// <summary>
        /// Gets all messages for specified user
        /// </summary>
        /// <returns>All messages that belongs to the user</returns>
        [HttpGet("GetMessages")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public ActionResult<MessageResponseModel> GetMessages()
        {
            try
            {
                var userID = User.FindFirst(x => x.Type == "UserID").Value;

                var messagesSentByUser = _messageService.GetAllSentByUser(userID);
                var messagesSentToUser = _messageService.GetAllSentToUser(userID);
                var newMessages = messagesSentToUser.Where(msg => msg.Read == false);

                var response = new MessageResponseModel
                {
                    MessagesSentByUser = messagesSentByUser,
                    MessagesSentToUser = messagesSentToUser,
                    NewMessages = newMessages,
                    NewMessagesAvailable = newMessages.Count()
                };

                return response;
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Something bad happened on server: {ex.Message}");
            }

        }



    }
}
