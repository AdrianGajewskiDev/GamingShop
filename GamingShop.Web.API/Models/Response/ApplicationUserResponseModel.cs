using System.Security;

namespace GamingShop.Web.API.Models.Response
{
    public class ApplicationUserResponseModel
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public bool EmailConfirmed { get; set; }
    }
}

