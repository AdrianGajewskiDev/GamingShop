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
        /// <summary>
        /// All Passed
        /// </summary>

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

        [Test]
        public void Return_Game_Corresponding_To_ID()
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

                var games = gameService.GetByID(4);


                Assert.AreEqual("The Witcher", games.Title);
            }
        }

        [Test]
        public void Return_Game_Launch_Date()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: "SearchTesting").Options;

            using (var ctx = new ApplicationDbContext(options))
            {
                ctx.Games.Add(new Data.Models.Game
                {
                    ID = 1,
                    Platform = "Xbox_One",
                    Title = "Assassin Creed",
                    Producent = "Ubisoft",
                    DayOfLaunch = "12",
                    YearOfLaunch = "2019",
                    MonthOfLaunch = "09"

                });

                ctx.Games.Add(new Data.Models.Game
                {
                    ID = 2,
                    Platform = "PS4",
                    Title = "Assassin Creed",
                    Producent = "Ubisoft",
                    DayOfLaunch = "22",
                    YearOfLaunch = "2017",
                    MonthOfLaunch = "06"
                });

                ctx.Games.Add(new Data.Models.Game
                {
                    ID = 3,
                    Platform = "PC",
                    Title = "Call of Duty",
                    Producent = "Treyach",
                    DayOfLaunch = "17",
                    YearOfLaunch = "2014",
                    MonthOfLaunch = "03"
                });

                ctx.Games.Add(new Data.Models.Game
                {
                    ID = 4,
                    Platform = "Xbox_One",
                    Title = "The Witcher",
                    Producent = "CDP",
                    DayOfLaunch = "19",
                    YearOfLaunch = "2012",
                    MonthOfLaunch = "01"
                });

                ctx.SaveChanges();
            }

            using (var ctg = new ApplicationDbContext(options))
            {
                var gameService = new GameService(ctg);

                var item = gameService.GetDateOfLaunch(4);


                Assert.AreEqual("19/01/2012", item);
            }
        }

        [Test]
        public void Return_All_Games_By_Type ()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: "SearchTesting").Options;

            using (var ctx = new ApplicationDbContext(options))
            {
                ctx.Games.Add(new Data.Models.Game
                {
                    ID = 1,
                    Platform = "Xbox_One",
                    Title = "Assassin Creed",
                    Producent = "Ubisoft",
                    DayOfLaunch = "12",
                    YearOfLaunch = "2019",
                    MonthOfLaunch = "09",
                    Type = "RPG"

                });

                ctx.Games.Add(new Data.Models.Game
                {
                    ID = 2,
                    Platform = "PS4",
                    Title = "Assassin Creed",
                    Producent = "Ubisoft",
                    DayOfLaunch = "22",
                    YearOfLaunch = "2017",
                    MonthOfLaunch = "06",
                    Type = "Adventure"

                });

                ctx.Games.Add(new Data.Models.Game
                {
                    ID = 3,
                    Platform = "PC",
                    Title = "Call of Duty",
                    Producent = "Treyach",
                    DayOfLaunch = "17",
                    YearOfLaunch = "2014",
                    MonthOfLaunch = "03",
                    Type = "FPS"
                });

                ctx.Games.Add(new Data.Models.Game
                {
                    ID = 4,
                    Platform = "Xbox_One",
                    Title = "The Witcher",
                    Producent = "CDP",
                    DayOfLaunch = "19",
                    YearOfLaunch = "2012",
                    MonthOfLaunch = "01",
                    Type = "MMO"
                });

                ctx.SaveChanges();
            }

            using (var ctg = new ApplicationDbContext(options))
            {
                var gameService = new GameService(ctg);

                var item = gameService.GetAllByType("RPG");


                Assert.AreEqual(1, item.Count());
            }
        }

    }
}
