using System.Security.Claims;
using System.Threading.Tasks;
using GamingShop.Data.Models;
using GamingShop.Web.Data;
using Microsoft.AspNetCore.Identity;

namespace GamingShop.Service
{
    public class ApplicationUserService : IApplicationUser
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _dbContext;

        public ApplicationUserService(UserManager<ApplicationUser> userManager,
                                      ApplicationDbContext context)
        {
            _userManager = userManager;
            _dbContext = context;
        }

        public async Task<ApplicationUser> GetUser(ClaimsPrincipal claims)
        {
            var user = await  _userManager.GetUserAsync(claims);

            return user;
        }

        public string GetUserID(ClaimsPrincipal claims)
        {
            return _userManager.GetUserId(claims);
        }

        public void UpdateUserData(ref ApplicationUser user, ApplicationUser newData)
        {
            user.Email = newData.Email;
            user.PhoneNumber = newData.PhoneNumber;
            user.UserName = newData.UserName;
            _dbContext.SaveChanges();
        }

    }
}
