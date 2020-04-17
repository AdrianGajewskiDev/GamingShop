using GamingShop.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GamingShop.Service
{
    public interface IGame
    {
        IEnumerable<Game> GetAll();
        IEnumerable<Game> GetAllAvailable();
        IEnumerable<Game> GetAllByPlatform(Platform platform);
        IEnumerable<Game> GetAllBySearchQuery(string searchQuery);
        IEnumerable<Game> GetAllByType(string type);
        Game GetByID(int id);
        Game GetByTitle(string title);
        string GetDateOfLaunch(int id);
    }
}
