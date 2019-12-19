using Microsoft.AspNetCore.Identity;

namespace GamingShop.Data.Models
{
    public class ApplicationUser : IdentityUser<int>
    {
        public int CartID { get; set; }
    }
}
