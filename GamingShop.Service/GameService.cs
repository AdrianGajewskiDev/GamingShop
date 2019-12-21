using System;
using System.Collections.Generic;
using System.Linq;
using GamingShop.Data.Models;
using GamingShop.Web.Data;

namespace GamingShop.Service
{
    public class GameService : IGame
    {
        private readonly ApplicationDbContext _dbContext;

        public GameService(ApplicationDbContext contex)
        {
            _dbContext = contex;
        }

        public IEnumerable<Game> GetAll()
        {
            return _dbContext.Games;
        }

        public IEnumerable<Game> GetAllByPlatform(string platform)
        {
            var p = platform.ToLower();

            return _dbContext.Games.Where(x => x.Platform.ToLower() == p);
        }

        public IEnumerable<Game> GetAllBySearchQuery(string searchQuery)
        {
            var query = searchQuery.ToLower();

            IEnumerable<Game> result = GetAll().Where(x => x.Title.ToLower().Contains(query) | x.Platform.ToLower().Contains(query) | x.Producent.ToLower().Contains(query));

            return result;
        }

        public IEnumerable<Game> GetAllByType(string type)
        {
            var t = type.ToLower();

            return _dbContext.Games.Where(x => x.Type == t);
        }

        public Game GetByID(int id)
        {
            return _dbContext.Games.Where(x => x.ID == id).FirstOrDefault();
        }

        public Game GetByTitle(string title)
        {
            var t = title.ToLower();

            return _dbContext.Games.Where(x => x.Title.ToLower() == t).FirstOrDefault();
        }

        public string GetDateOfLaunch(int id)
        {
            var game = GetByID(id);

            return $"{game.DayOfLaunch}/{game.MonthOfLaunch}/{game.YearOfLaunch}";
        }
    }
}
