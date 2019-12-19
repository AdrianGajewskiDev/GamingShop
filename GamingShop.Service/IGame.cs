using GamingShop.Data.Models;
using System.Collections.Generic;

namespace GamingShop.Service
{
    public interface IGame
    {
        IEnumerable<Game> GetAll();
        IEnumerable<Game> GetAllByPlatform(string platform);
        IEnumerable<Game> GetAllByType(string type);
        Game GetByID(int id);
        Game GetByTitle(string title);

        string GetDateOfLaunch(int id);
    }
}
