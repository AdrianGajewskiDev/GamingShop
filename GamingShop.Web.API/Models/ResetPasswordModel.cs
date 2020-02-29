namespace GamingShop.Web.API.Models
{
    public class ResetPasswordModel
    {
        public string UserID { get; set; }
        public string JWTToken { get; set; }
        public string Password { get; set; }
    }
}
