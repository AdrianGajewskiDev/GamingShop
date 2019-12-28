using GamingShop.Data.Models;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GamingShop.Service
{
    public interface IApplicationUser
    {
        Task<ApplicationUser> GetUser(ClaimsPrincipal claims);
        string GetUserID(ClaimsPrincipal claims);
        void UpdateUsername(ClaimsPrincipal user, string newUsername);
        void UpdateEmail(ClaimsPrincipal user,string newemailrname);
        void UpdatePhoneNumber(ClaimsPrincipal user, string newPhoneNumber);
        bool IsSignedIn(ClaimsPrincipal claims);
    }
}
