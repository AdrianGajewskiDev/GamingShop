using GamingShop.Data.Models;
using GamingShop.Service;
using GamingShop.Web.API.Models;
using GamingShop.Web.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
        private readonly JWTToken _tokenWriter;
        public UserController(UserManager<ApplicationUser> service, ApplicationDbContext context, IEmailSender emailSender, JWTToken tokenWriter)
        {
            _userManager = service;
            _dbContext = context;
            _emailSender = emailSender;
            _tokenWriter = tokenWriter;
        }

        [HttpPost("register")]
        public async Task<ActionResult<ApplicationUser>> RegisterUser(RegisterModel registerModel)
        {
            var newUser = new ApplicationUser
            {
                UserName = registerModel.Username,
                Email = registerModel.Email,
                PhoneNumber = registerModel.PhoneNumber,
                Password = registerModel.Password
            };

            var result = await _userManager.CreateAsync(newUser, newUser.Password);
            if (result.Succeeded)
            {

                var token = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);

                var callbackUrl = Url.Action("ConfirmEmail", "UserProfile", new { userID = newUser.Id, token = token }, Request.Scheme);

                var cart = _dbContext.Carts.Add(new Cart());
                await _dbContext.SaveChangesAsync();

                newUser.CartID = cart.Entity.ID;
                await _dbContext.SaveChangesAsync();
                await _emailSender.SendEmailAsync(newUser.Email, "Confirmation Email",
                        $"Please confirm your account by <a href='{callbackUrl}'>clicking here</a>.");

                return newUser;
            }
            else
                return BadRequest();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);

            if(user != null && await _userManager.CheckPasswordAsync(user,model.Password))
            {
                
                var token = _tokenWriter.CreateToken("UserID", user.Id, 5d);

                return Ok(new { token });


            }
            else
                return BadRequest(new { message = "Incorect username or password!"});
        }

    }
}
