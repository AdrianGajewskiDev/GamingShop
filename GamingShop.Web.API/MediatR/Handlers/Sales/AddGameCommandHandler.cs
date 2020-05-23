using AutoMapper;
using GamingShop.Data.DbContext;
using GamingShop.Data.Models;
using GamingShop.Web.API.MediatR.Commands.Sales;
using GamingShop.Web.API.Models.Response;
using GamingShop.Web.Data;
using MediatR;
using Microsoft.AspNetCore.Routing;
using System.Threading;
using System.Threading.Tasks;

namespace GamingShop.Web.API.MediatR.Handlers
{
    public class AddGameCommandHandler : IRequestHandler<AddGameCommand, AddGameResponseModel>
    {
        private readonly IMapper _mapper;
        private readonly LinkGenerator _generator;
        private readonly ApplicationDbContextFactory _dbContextFactory;
        private ApplicationDbContext _context;

        public AddGameCommandHandler(IMapper mapper, LinkGenerator generator, ApplicationDbContextFactory factory)
        {
            _mapper = mapper;
            _generator = generator;
            _dbContextFactory = factory;
        }

        public async Task<AddGameResponseModel> Handle(AddGameCommand request, CancellationToken cancellationToken)
        {
            var result = _mapper.Map<Game>(request.NewGameModel);

            result.DayOfLaunch = request.NewGameModel.LaunchDate.Split("/")[0];
            result.MonthOfLaunch = request.NewGameModel.LaunchDate.Split("/")[1];
            result.YearOfLaunch = request.NewGameModel.LaunchDate.Split("/")[2];
            result.OwnerID = request.UserID;

            var link = _generator.GetPathByAction("AddGame", "Sales");

            using (_context = _dbContextFactory.CreateDbContext())
            {
                await _context.Games.AddAsync(result);

                if (await _context.SaveChangesAsync() > 0)
                    return new AddGameResponseModel
                    {
                        GameID = result.ID,
                        Link = link
                    };
            }

            return null;

        }
    }
}
