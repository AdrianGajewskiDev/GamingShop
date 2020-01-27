using GamingShop.Data.Models;
using GamingShop.Service;
using GamingShop.Web.API.Models;
using GamingShop.Web.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
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
        private readonly ApplicationOptions _options;
        public UserController(UserManager<ApplicationUser> service, ApplicationDbContext context, IEmailSender emailSender, IOptions<ApplicationOptions> options)
        {
            _userManager = service;
            _dbContext = context;
            _emailSender = emailSender;
            _options = options.Value;
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

            var callbackUrl = $"http://localhost:55367/api/UserProfile/ConfirmEmail/{newUser.Id}";



            var result = await _userManager.CreateAsync(newUser, newUser.Password);
            if (result.Succeeded)
            {

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
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim("UserID", user.Id.ToString())
                    }),
                    Expires = DateTime.UtcNow.AddHours(5d),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Secret_Key)), SecurityAlgorithms.HmacSha256Signature)
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var securityToken = tokenHandler.CreateToken(tokenDescriptor);
                var token = tokenHandler.WriteToken(securityToken);

                return Ok(new { token });


            }
            else
                return BadRequest(new { message = "Incorect username or password!"});
        }

    }
}
