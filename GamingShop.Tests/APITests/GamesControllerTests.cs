//using AutoMapper;
//using GamingShop.Data.DbContext;
//using GamingShop.Data.Models;
//using GamingShop.Service;
//using GamingShop.Service.Implementation;
//using GamingShop.Service.Services;
//using GamingShop.Web.API.Controllers;
//using GamingShop.Web.API.Profiles;
//using GamingShop.Web.Data;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.Extensions.Options;
//using Moq;
//using NUnit.Framework;
//using System.Linq;
//using System.Threading.Tasks;

//namespace GamingShop.Tests.APITests
//{
//    [TestFixture]
//    public class GamesControllerTests
//    {
//        #region services
//        private ApplicationDbContext _context;
//        private UserManager<ApplicationUser> _userManager;
//        private IGame _gamesService;
//        private IImage _imageService;
//        private IMapper _mapper;
//        #endregion

//        private const string ConnectionString = "Server=(localdb)\\mssqllocaldb;Database=GamingShop;Trusted_Connection=True;MultipleActiveResultSets=true";

//        [Test]
//        public void Games_Controller_Should_Return_All_Games_From_Database()
//        {
//            IOptions<ApplicationOptions> options = new Mock<IOptions<ApplicationOptions>>().Object;
//            _context = new ApplicationDbContextFactory(ConnectionString).CreateDbContext();
//            _gamesService = new GameService(_context);
//            _imageService = new ImageService(_context, options);

//            var mappingConfig = new MapperConfiguration(mc =>
//            {
//                mc.AddProfile(new GameProfile(_imageService));
//            });

//            _mapper = mappingConfig.CreateMapper();


//            var controller = new GamesController(_context, _gamesService, null, _imageService, _mapper);
//            var games = controller.GetGames();

//            var expected = _context.Games.Where(g => g.Sold == false).Count();

//            var result = games.Value.PCGames.Count() + games.Value.PlaystationGames.Count() + games.Value.XboxOneGames.Count();

//            Assert.AreEqual(expected, result);
//        }

//        [TestCase("Xbox One")]
//        [TestCase("Playstion 4")]
//        [TestCase("PC")]
//        [TestCase("The Witcher 3")]
//        [TestCase("Fifa 20")]
//        public async Task Games_Controller_Should_Return_All_Games_Coresponding_To_Search_Query(string query)
//        {
//            IOptions<ApplicationOptions> options = new Mock<IOptions<ApplicationOptions>>().Object;
//            _context = new ApplicationDbContextFactory(ConnectionString).CreateDbContext();
//            _gamesService = new GameService(_context);
//            _imageService = new ImageService(_context, options);

//            var mappingConfig = new MapperConfiguration(mc =>
//            {
//                mc.AddProfile(new GameProfile(_imageService));
//            });

//            _mapper = mappingConfig.CreateMapper();


//            var controller = new GamesController(_context, _gamesService, null, _imageService, _mapper);
//            var games = await controller.GetBySearchQuery(query);

//            var expected = _gamesService.GetAllBySearchQuery(query).Count();
//            var result = games.Value.Count();

//            Assert.AreEqual(expected, result);
//        }

//    }
//}
