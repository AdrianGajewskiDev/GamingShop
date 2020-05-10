using GamingShop.Data.DbContext;
using GamingShop.Service;
using GamingShop.Web.Data;
using NUnit.Framework;
using System.Linq;

namespace GamingShop.Tests.ServiceTests
{
    [TestFixture]
    public class GameServiceTests
    {
        private ApplicationDbContext _db;
        private IGame _gameService;

        private const string ConnectionString = "Server=(localdb)\\mssqllocaldb;Database=GamingShop;Trusted_Connection=True;MultipleActiveResultSets=true";

        [Test]
        public void Game_Service_Should_Return_All_Games_From_Database()
        {
            _db = new ApplicationDbContextFactory(ConnectionString).CreateDbContext();
            _gameService = new GameService(_db);

            int expected = 15;
            var result = _gameService.GetAll();

            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result.Count());
        }

        [Test]
        public void Game_Service_Should_Return_All_Available_Games_From_Database()
        {
            _db = new ApplicationDbContextFactory(ConnectionString).CreateDbContext();
            _gameService = new GameService(_db);

            int expected = 8;
            var result = _gameService.GetAllAvailable();

            Assert.IsNotNull(result);
            Assert.That(!result.Any(g => g.Sold == true));
            Assert.AreEqual(expected, result.Count());
        }

        [TestCase(Platform.PC, 4)]
        [TestCase(Platform.XboxOne, 3)]
        [TestCase(Platform.Playstation_4, 1)]
        public void Game_Service_Should_Return_All_Games_Corresponding_To_Platform(Platform platform, int expected)
        {
            _db = new ApplicationDbContextFactory(ConnectionString).CreateDbContext();
            _gameService = new GameService(_db);

            var result = _gameService.GetAllByPlatform(platform);

            string platf = default(string);

            //To match column names from database
            if (platform == Platform.Playstation_4)
                platf = "Playstation 4";
            else if (platform == Platform.XboxOne)
                platf = "Xbox One";
            else
                platf = "PC";

            Assert.That(!result.Any(g => g.Platform != platf));
            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result.Count());
        }

    }
}
