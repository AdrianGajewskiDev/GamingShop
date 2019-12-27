using GamingShop.Service;
using GamingShop.Web.Data;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Linq;

namespace GamingShop.Tests
{
    [TestFixture]
    class Cart_Service_Should
    {
        [Test]
        public void Return_Games_Amount_Corresponding_To_Cart()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: "SearchTesting").Options;

            using (var ctx = new ApplicationDbContext(options))
            {

                ctx.Games.Add(new Data.Models.Game
                {
                    ID = 2,
                    Platform = "Xbox_One",
                    Title = "Assassin Creed",
                    Producent = "Ubisoft"
                });


                ctx.Carts.Add(new Data.Models.Cart
                {
                    ID = 1
                });

                ctx.CartItems.Add(new Data.Models.CartItem
                {
                    ID = 1,
                });


                ctx.SaveChanges();
            }

            using (var ctg = new ApplicationDbContext(options))
            {
                var gameService = new GameService(ctg);
                var service = new CartService(ctg, gameService);

                var result = service.GetGames(1);

                Assert.AreEqual(0, result.Count());
            }
        }

        [Test]
        public void Return_Cart_Corresponding_To_ID()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: "SearchTesting").Options;

            using (var ctx = new ApplicationDbContext(options))
            {

                ctx.Carts.Add(new Data.Models.Cart
                {
                    ID = 1
                });

                ctx.Carts.Add(new Data.Models.Cart
                {
                    ID = 2
                });

                ctx.SaveChanges();
            }

            using (var ctg = new ApplicationDbContext(options))
            {
                var gameService = new GameService(ctg);
                var service = new CartService(ctg, gameService);

                var result = service.GetById(1);

                Assert.AreEqual(1, result.ID);
            }
        }
    }
}
