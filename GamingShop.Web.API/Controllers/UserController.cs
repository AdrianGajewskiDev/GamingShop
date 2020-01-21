using GamingShop.Data.Models;
using GamingShop.Service;
using GamingShop.Web.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace GamingShop.Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _dbContext;
        private readonly IEmailSender _emailSender;
        public UserController(UserManager<ApplicationUser> service, ApplicationDbContext context, IEmailSender emailSender)
        {
            _userManager = service;
            _dbContext = context;
            _emailSender = emailSender;
        }

        [HttpPost("register")]
        public async Task<ActionResult<ApplicationUser>> RegisterUser(ApplicationUser user)
        {
            var newUser = new ApplicationUser
            {
                UserName = user.UserName,
                Email = user.Email,
                Password = user.Password
            };

            var result = await _userManager.CreateAsync(newUser, newUser.Password);
            if (result.Succeeded)
            {
                var cart = _dbContext.Carts.Add(new Cart());
                await _dbContext.SaveChangesAsync();

                newUser.CartID = cart.Entity.ID;
                await _dbContext.SaveChangesAsync();

                return newUser;
            }
            else
                return NotFound();
        }
    }
}
