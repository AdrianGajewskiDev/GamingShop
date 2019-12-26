using GamingShop.Data.Models;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GamingShop.Service
{
    public interface IApplicationUser
    {
        Task<ApplicationUser> GetUser(ClaimsPrincipal claims);
        string GetUserID(ClaimsPrincipal claims);
        void UpdateUserData(ref ApplicationUser user, ApplicationUser newData);
    }
}
