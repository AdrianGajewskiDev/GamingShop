using System;
using System.Collections.Generic;
using System.Linq;
using GamingShop.Data.DbContext;
using GamingShop.Data.Models;
using GamingShop.Web.Data;

namespace GamingShop.Service
{
    public class GameService : IGame
    {
        private ApplicationDbContext _dbContext;
        private ApplicationDbContextFactory _dbFactory;

        public GameService(ApplicationDbContextFactory contextfactory)
        {
            _dbFactory = contextfactory;
        }

        public IEnumerable<Game> GetAll()
        {
            using (_dbContext = _dbFactory.CreateDbContext())
            {
                return _dbContext.Games;
            }
        }

        public IEnumerable<Game> GetAllAvailable()
        {
            using (_dbContext = _dbFactory.CreateDbContext())
            {
                var games = GetAll().Where(x => x.Sold == false);

                return games;
            }

        }

        public IEnumerable<Game> GetAllByPlatform(Platform platform)
        {

            using (_dbContext = _dbFactory.CreateDbContext())
            {

                IList<Game> games = new List<Game>();

                switch (platform)
                {
                    case Platform.XboxOne:
                        {
                            var platf = "Xbox One";
                            games = _dbContext.Games.Where(x => x.Platform == platf && x.Sold == false).ToList();
                        }
                        break;
                    case Platform.Playstation_4:
                        {
                            var platf = "Playstation 4";
                            games = _dbContext.Games.Where(x => x.Platform == platf && x.Sold == false).ToList();
                        }
                        break;
                    case Platform.Xbox360:
                        {
                            var platf = "Xbox 360";
                            games = _dbContext.Games.Where(x => x.Platform == platf && x.Sold == false)
                                .OrderByDescending(x => x.Posted)
                                .Take(3).ToList();
                        }
                        break;
                    case Platform.Playstation_3:
                        {
                            var platf = "PS3";
                            games = _dbContext.Games.Where(x => x.Platform == platf && x.Sold == false).ToList();
                        }
                        break;
                    case Platform.PC:
                        {
                            var platf = "PC";
                            games = _dbContext.Games.Where(x => x.Platform == platf && x.Sold == false).ToList();
                        }
                        break;
                }

                return games;
            }


        }

        public IEnumerable<Game> GetAllBySearchQuery(string searchQuery)
        {
            using (_dbContext = _dbFactory.CreateDbContext())
            {


                var query = searchQuery.ToLower();

                IEnumerable<Game> result = GetAll().Where(x => x.Sold == false && x.Title.ToLower().Contains(query) | x.Platform.ToLower().Contains(query) | x.Producent.ToLower().Contains(query));

                return result;
            }
        }

        public IEnumerable<Game> GetAllByType(string type)
        {
            using (_dbContext = _dbFactory.CreateDbContext())
            {
                var t = type.ToLower();

                return GetAllAvailable().Where(x => x.Type.ToLower() == t);
            }
        }

        public Game GetByID(int id)
        {
            using (_dbContext = _dbFactory.CreateDbContext())
            {
                return _dbContext.Games.Where(x => x.ID == id).FirstOrDefault();
            }
        }

        public Game GetByTitle(string title)
        {
            using (_dbContext = _dbFactory.CreateDbContext())
            {
                var t = title.ToLower();

                return _dbContext.Games.Where(x => x.Title.ToLower() == t).FirstOrDefault();
            }
        }

        public string GetDateOfLaunch(int id)
        {
            using (_dbContext = _dbFactory.CreateDbContext())
            {
                var game = GetByID(id);

                return $"{game.DayOfLaunch}/{game.MonthOfLaunch}/{game.YearOfLaunch}";
            }
        }
    }
}
