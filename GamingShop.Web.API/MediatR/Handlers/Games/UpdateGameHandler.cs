using GamingShop.Data.DbContext;
using GamingShop.Service;
using GamingShop.Service.Services;
using GamingShop.Web.API.MediatR.Queries.Games;
using GamingShop.Web.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace GamingShop.Web.API.MediatR.Handlers.Games
{
    public class UpdateGameHandler : IRequestHandler<UpdateGameQuery, bool>
    {
        private ApplicationDbContext _context;
        private readonly ApplicationDbContextFactory _contextFactory;
        private readonly IImage _imageService;
        private readonly IGame _gameService;
        public UpdateGameHandler(ApplicationDbContextFactory contextFactory, IImage imageService, IGame gameService)
        {
            _contextFactory = contextFactory;
            _imageService = imageService;
            _gameService = gameService;
        }

        public async Task<bool> Handle(UpdateGameQuery request, CancellationToken cancellationToken)
        {
            using(_context = _contextFactory.CreateDbContext())
            {
                var game = _gameService.GetByID(request.UpdateGameModel.ID);

                //TODO:Create mapper between UpdateGameModel to GameModel

                var dayOfLaunch = request.UpdateGameModel.LaunchDate.Split("/")[0];
                var monthOfLaunch = request.UpdateGameModel.LaunchDate.Split("/")[1];
                var yearOfLaunch = request.UpdateGameModel.LaunchDate.Split("/")[2];

                game.DayOfLaunch = dayOfLaunch;
                game.MonthOfLaunch = monthOfLaunch;
                game.YearOfLaunch = yearOfLaunch;
                game.OwnerID = request.UserID;
                game.ImageUrl = _imageService.GetImageNameForGame(request.UpdateGameModel.ID);
                game.Price = request.UpdateGameModel.Price;
                game.Producent = request.UpdateGameModel.Producent;
                game.Platform = request.UpdateGameModel.Platform;

                _context.Entry(game).State = EntityState.Modified;

                var result = await _context.SaveChangesAsync();

                if (result > 0)
                    return true;

                return false;
            }
        

        }
    }
}
