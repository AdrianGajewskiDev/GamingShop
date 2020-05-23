using GamingShop.Data.Models;
using System.Net;

namespace GamingShop.Web.API.Models.Response
{
    public class RegisterResponseModel
    {
        public ApplicationUser User { get; set; }
        public string Link { get; set; }
        public HttpStatusCode HttpStatusCode { get; set; }
    }
}
