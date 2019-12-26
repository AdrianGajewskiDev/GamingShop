using GamingShop.Service;
using GamingShop.Web.Models;
using GamingShop.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GamingShop.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IApplicationUser _userService;

        public AccountController(IApplicationUser userService)
        {
            _userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            if (_userService.IsSignedIn(User))
            {
                var user = await _userService.GetUser(User);

                var userModel = new UserIndexViewModel
                {
                    ID = user.Id,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    UserName = user.UserName,
                    CartId = user.CartID
                };

                var model = new AccountIndexModel
                {
                    User = userModel
                };

                return View(model);
            }
            else
                return Redirect("https://localhost:44367/Identity/Account/Login");

        }
    }
}