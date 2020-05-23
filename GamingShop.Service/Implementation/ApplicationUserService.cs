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
        private readonly ApplicationDbContextFactory _dbContextFactory;

        public ApplicationUserService(UserManager<ApplicationUser> userManager,
                                      ApplicationDbContextFactory contextFactory,
                                      SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _dbContextFactory = contextFactory;
            _signInManager = signInManager;

        }

        public ApplicationUser GetByID(string ID)
        {
            using (_dbContext = _dbContextFactory.CreateDbContext())
            {
                return _dbContext.Users.Where(user => user.Id == ID).FirstOrDefault();

            }
        }

        public async Task<ApplicationUser> GetUser(ClaimsPrincipal claims)
        {
            using (_dbContext = _dbContextFactory.CreateDbContext())
            {

                var user = await _userManager.GetUserAsync(claims);

                return user;
            }
        }

        public string GetUserID(ClaimsPrincipal claims)
        {
            using (_dbContext = _dbContextFactory.CreateDbContext())
            {
                return _userManager.GetUserId(claims);
            }
        }

        public bool IsSignedIn(ClaimsPrincipal claims)
        {
            using (_dbContext = _dbContextFactory.CreateDbContext())
            {
                return _signInManager.IsSignedIn(claims);
            }
        }

        public async void UpdateEmail(ClaimsPrincipal user, string newemail)
        {
            using (_dbContext = _dbContextFactory.CreateDbContext())
            {

                var currentUser = await GetUser(user);

                currentUser.Email = newemail;

                _dbContext.SaveChanges();
            }
        }

        public async void UpdatePhoneNumber(ClaimsPrincipal user, string newPhoneNumber)
        {
            using (_dbContext = _dbContextFactory.CreateDbContext())
            {

                var currentUser = await GetUser(user);

                currentUser.PhoneNumber = newPhoneNumber;

                _dbContext.SaveChanges();
            }
        }

        public async void UpdateUsername(ClaimsPrincipal user, string newUsername)
        {
            using (_dbContext = _dbContextFactory.CreateDbContext())
            {

                var currentUser = await GetUser(user);

                currentUser.UserName = newUsername;

                _dbContext.SaveChanges();
            }
        }
    }
}
