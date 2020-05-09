using AutoMapper;
using GamingShop.Data.Models;
using GamingShop.Service;
using GamingShop.Web.API.Models;
using GamingShop.Web.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GamingShop.Web.API.Controllers
{
    /// <summary>
    /// A controller to handle user account related requests
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _dbContext;
        private readonly IEmailSender _emailSender;
        private readonly JWTToken _tokenWriter;
        private readonly IMapper _mapper;
        private readonly ApplicationOptions _options;

        /// <summary>
        /// A default constructor 
        /// </summary>
        /// <param name="service">A user manager</param>
        /// <param name="context">A Database context</param>
        /// <param name="emailSender">A Email Sender</param>
        /// <param name="tokenWriter">A Jason Web Token Writer</param>
        /// <param name="options">A Application Settings</param>
        public UserController(UserManager<ApplicationUser> service, ApplicationDbContext context, IEmailSender emailSender,
            JWTToken tokenWriter, IOptions<ApplicationOptions> options, IMapper mapper)
        {
            _userManager = service;
            _dbContext = context;
            _emailSender = emailSender;
            _tokenWriter = tokenWriter;
            _mapper = mapper;
            _options = options.Value;

        }

        /// <summary>
        /// Register new user
        /// </summary>
        /// <param name="registerModel">User credentials</param>
        /// <returns>Adds new user to database and returns his details if registering was successful</returns>
        [HttpPost("register")]
        public async Task<ActionResult<ApplicationUser>> RegisterUser(RegisterModel registerModel)
        {

                var newUser = _mapper.Map<ApplicationUser>(registerModel);

                var result = await _userManager.CreateAsync(newUser, newUser.Password);
                if (result.Succeeded)
                {

                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);

                    var callbackUrl = Url.Action("ConfirmEmail", "UserProfile", new { userID = newUser.Id, token = token }, Request.Scheme);

                    var cart = _dbContext.Carts.Add(new Cart());
                    await _dbContext.SaveChangesAsync();

                    newUser.CartID = cart.Entity.ID;
                    await _dbContext.SaveChangesAsync();
                    await _emailSender.SendVerificationEmailAsync(newUser, "Confirmation Email",
                            callbackUrl);

                    return newUser;
                }
                else
                     return BadRequest("Failed to register new user");
        }

        /// <summary>
        /// Checks if <paramref name="model"/> is valid and the sign user in
        /// </summary>
        /// <param name="model">A user login credentials</param>
        /// <returns>Returns JWT containing UserID</returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
                var user = await _userManager.FindByNameAsync(model.Username);
                var userID = user.Id;

                if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
                {
                    var token = _tokenWriter.CreateToken("UserID", user.Id, 5d);

                    return Ok(new { token, userID });
                }
                else
                    return BadRequest(new { message = "Incorect username or password!" });
        }

        /// <summary>
        /// Sends a verification message to <paramref name="email"/> email
        /// </summary>
        /// <param name="email">A email to with send the verification message</param>
        /// <returns>Returs 200 ok result</returns>
        [HttpPost("ForgetPassword/{email}")]
        public async Task<IActionResult> ForgetPassword(string email)
        {

                var user = await _userManager.FindByEmailAsync(email);

                if (user == null)
                    return BadRequest("Provided email does not exist in our database");

                var generatedToken = await _userManager.GeneratePasswordResetTokenAsync(user);

                var jwtToken = _tokenWriter.CreateToken("Token", generatedToken, 1d);

                var redirectLink = $"{_options.ClientURL}resetPassword/{user.Id}/{jwtToken}";
                var message = new Message
                {
                    Content = $"<a href={redirectLink}>Click here to reset your password</a>",
                    Subject = "Reset Password",
                    Sent = DateTime.UtcNow,
                    SenderID = user.Id,
                    RecipientEmail = email,
                    RecipientID = user.Id
                };

                _dbContext.Messages.Add(message);

                await _dbContext.SaveChangesAsync();
                await _emailSender.SendEmail(message);

                return Ok();
        }

        /// <summary>
        /// A method that is called from <see cref="ForgetPassword(string)" verification link/>
        /// and reset user password
        /// </summary>
        /// <param name="model">A new password model</param>
        /// <returns>Returs 200 ok result if password was successfully changed</returns>
        [HttpPost("ForgetPasswordCallback")]
        public async Task<IActionResult> ForgetPasswordCallback([FromBody] ResetPasswordModel model)
        {
                var user = await _userManager.FindByIdAsync(model.UserID);

                var token = _tokenWriter.DecodeToken(model.JWTToken).First(c => c.Type == "Token").Value;

                var result = await _userManager.ResetPasswordAsync(user, token, model.Password);

                if (result.Succeeded)
                {
                    await _dbContext.SaveChangesAsync();

                    return Ok();
                }

                return BadRequest("Something went wrong while reseting your password, try again");
        }

        /// <summary>
        /// Gets user username 
        /// </summary>
        /// <param name="id">A ID of the user</param>
        /// <returns>Returns user username</returns>
        [HttpGet("getUsername/{id}")]
        public async Task<string> GetUsername(string id)
       {
            var user = await _userManager.FindByIdAsync(id);

            var result = (user == null) ? "Unknown" : user.UserName;
            return result;
        }
    }
}
