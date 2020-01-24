using GamingShop.Data.Models;
using GamingShop.Web.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GamingShop.Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _dbContext;
        public UserProfileController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _dbContext = context;
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
    }
}
