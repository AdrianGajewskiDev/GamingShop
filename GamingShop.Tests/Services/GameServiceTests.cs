using GamingShop.Data.DbContext;
using GamingShop.Data.Models;
using GamingShop.Service;
using Microsoft.EntityFrameworkCore.Internal;
using NUnit.Framework;
using System;
using System.Linq;

namespace GamingShop.Tests.Services
{
    [TestFixture]
    public class Game_Service_Should
    {
        private const string connectionString = "Server=(localdb)\\mssqllocaldb;Database=GamingShop;Trusted_Connection=True;MultipleActiveResultSets=true";
        private ApplicationDbContextFactory _dbContextFactory = new ApplicationDbContextFactory(connectionString);

        [Test]
        public void Return_All_Available_Games_In_Database()
        {
            var database = _dbContextFactory.CreateDbContext();

            IGame _gameService = new GameService(database);

            var result = _gameService.GetAllAvailable();

            int expected = 10;

            Assert.IsNotNull(result);
            Assert.IsTrue(!result.Any(res => res.Sold == true));
            Assert.AreEqual(result.Count(), expected);
        }

        [TestCase(Platform.PC, 4)]
        [TestCase(Platform.XboxOne, 3)]
        [TestCase(Platform.Playstation_4, 3)]
        public void Return_All_Games_By_Platform(Platform platform, int expected)
        {
            var database = _dbContextFactory.CreateDbContext();

            IGame _gameService = new GameService(database);

            var result = _gameService.GetAllByPlatform(platform);

            Assert.IsNotNull(result);
            Assert.IsTrue(!result.Any(res => res.Sold == true));
            Assert.AreEqual(result.Count(), expected);
        }

        [TestCase(GameType.FPS, 1)]
        [TestCase(GameType.RPG, 2)]
        [TestCase(GameType.Survival_horror, 2)]
        [TestCase(GameType.Survival, 0)]
        [TestCase(GameType.TPS, 1)]
        [TestCase(GameType.Action_Adventure, 2)]
        public void Return_All_Games_Type(string type, int expected)
        {
            var database = _dbContextFactory.CreateDbContext();

            IGame _gameService = new GameService(database);

            var result = _gameService.GetAllByType(type);

            Assert.IsNotNull(result);
            Assert.IsTrue(!result.Any(res => res.Type != type));
            Assert.AreEqual(result.Count(), expected);
        }
 
        [TestCase(42)]
        [TestCase(43)]
        [TestCase(51)]
        [TestCase(52)]
        [TestCase(68)]
        [TestCase(69)]
        public void Return_Game_By_ID(int id)
        {
            var database = _dbContextFactory.CreateDbContext();

            IGame _gameService = new GameService(database);

            var result = _gameService.GetByID(id);

            Assert.IsInstanceOf(typeof(Game), result);
            Assert.IsTrue(database.Games.Where(game => game.ID == id).Count() == 1);
        }

    }
}
