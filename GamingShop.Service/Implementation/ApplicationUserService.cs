using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using GamingShop.Data.DbContext;
using GamingShop.Data.Models;
using GamingShop.Web.Data;
using Microsoft.AspNetCore.Identity;

namespace GamingShop.Service
{
    public class ApplicationUserService : IApplicationUser
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private ApplicationDbContext _dbContext;

        public ApplicationUserService(UserManager<ApplicationUser> userManager,
         ApplicationDbContext dbContext, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _dbContext = dbContext;

        }

        public ApplicationUser GetByID(string ID)
        {
            return _dbContext.Users.Where(user => user.Id == ID).FirstOrDefault();
        }

        public async Task<ApplicationUser> GetUser(ClaimsPrincipal claims)
        {
            var user = await _userManager.GetUserAsync(claims);

            return user;
        }

        public string GetUserID(ClaimsPrincipal claims)
        {
            return _userManager.GetUserId(claims);
        }

        public bool IsSignedIn(ClaimsPrincipal claims)
        {
            return _signInManager.IsSignedIn(claims);
        }

        public async void UpdateEmail(ClaimsPrincipal user, string newemail)
        {
            var currentUser = await GetUser(user);

            currentUser.Email = newemail;

            _dbContext.SaveChanges();
        }

        public async void UpdatePhoneNumber(ClaimsPrincipal user, string newPhoneNumber)
        {
            var currentUser = await GetUser(user);

            currentUser.PhoneNumber = newPhoneNumber;

            _dbContext.SaveChanges();
        }

        public async void UpdateUsername(ClaimsPrincipal user, string newUsername)
        {
            var currentUser = await GetUser(user);

            currentUser.UserName = newUsername;

            _dbContext.SaveChanges();
        }
    }
}
