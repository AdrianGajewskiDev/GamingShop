using System.Linq;
using GamingShop.Service;
using GamingShop.Web.Data;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace GamingShop.Tests
{
    [TestFixture]
    public class Game_Service_Should
    {
        [Test]
        public void Return_Result_Corresponding_To_Search_Query()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: "SearchTesting").Options;

            using (var ctx = new ApplicationDbContext(options))
            {
                ctx.Games.Add(new Data.Models.Game
                {
                    ID = 1,
                    Platform = "Xbox_One",
                    Title = "Assassin Creed",
                    Producent = "Ubisoft"
                });

                ctx.Games.Add(new Data.Models.Game
                {
                    ID = 2,
                    Platform = "PS4",
                    Title = "Assassin Creed",
                    Producent = "Ubisoft"
                });

                ctx.Games.Add(new Data.Models.Game
                {
                    ID = 3,
                    Platform = "PC",
                    Title = "Call of Duty",
                    Producent = "Treyach"
                });

                ctx.Games.Add(new Data.Models.Game
                {
                    ID = 4,
                    Platform = "Xbox_One",
                    Title = "The Witcher",
                    Producent = "CDP"
                });

                ctx.SaveChanges();
            }

            using (var ctg = new ApplicationDbContext(options))
            {
                var gameService = new GameService(ctg);

                var games = gameService.GetAllBySearchQuery("XbOx ONe");

                var result = games.Count();

                Assert.AreEqual(2, result);
            }


        }
    }
}
