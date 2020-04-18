using System.Collections.Generic;

namespace GamingShop.Web.API.Models.Response
{
    public class GamesResponseModel
    {
        public IEnumerable<GameIndexResponseModel> XboxOneGames { get; set; }
        public IEnumerable<GameIndexResponseModel> PlaystationGames { get; set; }
        public IEnumerable<GameIndexResponseModel> PCGames { get; set; }
    }
}
