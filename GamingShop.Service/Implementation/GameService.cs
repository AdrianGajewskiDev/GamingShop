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

        public IEnumerable<Game> GetAllAvailable()
        {
            var games = GetAll().Where(x => x.Sold == false);

            return games;
        }

        public IEnumerable<Game> GetAllByPlatform(Platform platform)
        {
            IList<Game> games = new List<Game>();

            switch (platform)
            {
                case Platform.XboxOne:
                    {
                        var platf = "Xbox One";
                        games =_dbContext.Games.Where(x => x.Platform == platf && x.Sold ==false )
                            .OrderByDescending(x => x.Posted)
                            .Take(3).ToList();
                    }
                    break;
                case Platform.Playstation_4:
                    {
                        var platf = "PS4";
                        games = _dbContext.Games.Where(x => x.Platform == platf || x.Platform == "Playstation 4" && x.Sold == false)
                            .OrderByDescending(x => x.Posted)
                            .Take(3).ToList(); 
                    }
                    break;
                case Platform.Xbox360:
                     {
                        var platf = "Xbox 360";
                        games  = _dbContext.Games.Where(x => x.Platform == platf && x.Sold == false)
                            .OrderByDescending(x => x.Posted)
                            .Take(3).ToList(); 
                    }
                    break;
                case Platform.Playstation_3:
                    {
                        var platf = "PS3";
                        games = _dbContext.Games.Where(x => x.Platform == platf || x.Platform == "Playstation 3" && x.Sold == false)
                            .OrderByDescending(x => x.Posted)
                            .Take(3).ToList();
                    }
                    break;
                case Platform.PC:
                    {
                        var platf = "PC";
                        games = _dbContext.Games.Where(x => x.Platform == platf && x.Sold == false)
                            .OrderByDescending(x => x.Posted)
                            .Take(3).ToList();
                    }
                    break;
            }

            return games;

        }

        public IEnumerable<Game> GetAllBySearchQuery(string searchQuery)
        {
            var query = searchQuery.ToLower();

            IEnumerable<Game> result = GetAll().Where(x => x.Sold == false &&  x.Title.ToLower().Contains(query) | x.Platform.ToLower().Contains(query) | x.Producent.ToLower().Contains(query));

            return result;
        }

        public IEnumerable<Game> GetAllByType(string type)
        {
            var t = type.ToLower();

            return GetAllAvailable().Where(x => x.Type.ToLower() == t);
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
