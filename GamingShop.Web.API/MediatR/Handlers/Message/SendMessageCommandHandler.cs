using AutoMapper;
using GamingShop.Data.DbContext;
using GamingShop.Data.Models;
using GamingShop.Service;
using GamingShop.Web.API.MediatR.Commands.Message;
using GamingShop.Web.Data;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GamingShop.Web.API.MediatR.Handlers.Message
{
    public class SendMessageCommandHandler : IRequestHandler<SendMessageCommand, bool>
    {
        private ApplicationDbContext _dbContext;
        private readonly ApplicationDbContextFactory _dbContextFactory;
        private readonly IGame _gameService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public SendMessageCommandHandler( ApplicationDbContextFactory contextFactory,
        IGame gameService, UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _dbContextFactory = contextFactory;
            _gameService = gameService;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<bool> Handle(SendMessageCommand request, CancellationToken cancellationToken)
        {
            using(_dbContext = _dbContextFactory.CreateDbContext())
            {
                GamingShop.Data.Models.Message msg;

                if (request.Message.Replying == false)
                {
                    var recipientID = _gameService.GetByID(request.Message.GameID).OwnerID;
                    var recipient = await _userManager.FindByIdAsync(recipientID);
                    var sender = await _userManager.FindByIdAsync(request.Message.SenderID);

                    msg = new GamingShop.Data.Models.Message
                    {
                        Content = request.Message.Content,
                        Read = false,
                        RecipientEmail = recipient.Email,
                        RecipientID = recipientID,
                        SenderID = request.Message.SenderID,
                        Sent = DateTime.UtcNow,
                        Subject = request.Message.Subject,
                        SenderEmail = sender.Email

                    };
                }
                else
                {
                    msg = _mapper.Map<GamingShop.Data.Models.Message>(request.Message);
                }

                _dbContext.Messages.Add(msg);

                if (await _dbContext.SaveChangesAsync() > 0)
                    return true;

                return false;
            }
          
        }
    }
}
