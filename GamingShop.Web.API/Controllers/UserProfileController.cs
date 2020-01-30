using GamingShop.Data.Models;
using GamingShop.Web.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace GamingShop.Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _dbContext;
        private ApplicationOptions _options;
        public UserProfileController(UserManager<ApplicationUser> userManager, ApplicationDbContext context, IOptions<ApplicationOptions> options)
        {
            _userManager = userManager;
            _dbContext = context;
            _options = options.Value;
        }


        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<ApplicationUser>> GetUserProfile()
        {
            string userID = User.Claims.First(c => c.Type == "UserID").Value;

            var user = await _userManager.FindByIdAsync(userID);

            if (user != null)
            {
                return user;
            }
            else
                return BadRequest(new { message = "Cannot get user profile" });
        }

        [HttpPost("UpdateUsername/{username}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> UpdateUsername(string username)
        {

            string userID = User.Claims.First(c => c.Type == "UserID").Value;

            var user = await _userManager.FindByIdAsync(userID);

            user.UserName = username;
            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return new NoContentResult();
            }

            return new BadRequestResult();
        }

        [HttpPost("UpdateEmail/{email}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> UpdateEmail(string email)
        {

            string userID = User.Claims.First(c => c.Type == "UserID").Value;

            var user = await _userManager.FindByIdAsync(userID);

            user.Email = email;
            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return new NoContentResult();
            }

            return new BadRequestResult();
        }
        [HttpPost("UpdatePhoneNumber/{phoneNumber}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> UpdatePhoneNumber(string phoneNumber)
        {

            string userID = User.Claims.First(c => c.Type == "UserID").Value;

            var user = await _userManager.FindByIdAsync(userID);

            user.PhoneNumber = phoneNumber;
            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return new NoContentResult();
            }

            return new BadRequestResult();
        }


        [Route("ConfirmEmail/{userID}")]
        public async Task<IActionResult> ConfirmEmail(string userID, string token)
        {
            var user = await _userManager.FindByIdAsync(userID);

            await _userManager.ConfirmEmailAsync(user, token);

            await _dbContext.SaveChangesAsync();

            OpenUrl(_options.ClientURL + "/EmailConfirmation");
            return new NoContentResult();
        }

        private void OpenUrl(string url)
        {
            try
            {
                Process.Start(url);
            }
            catch
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    url = url.Replace("&", "^&");
                    Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    Process.Start("xdg-open", url);
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    Process.Start("open", url);
                }
                else
                {
                    throw;
                }
            }
        }
    }
}
