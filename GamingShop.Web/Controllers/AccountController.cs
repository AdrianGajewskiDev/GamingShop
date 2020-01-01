using GamingShop.Data.Models;
using GamingShop.Service;
using GamingShop.Web.Models;
using GamingShop.Web.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace GamingShop.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IApplicationUser _userService;
        private readonly IOrder _orderService;
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountController(IApplicationUser userService, UserManager<ApplicationUser> userManager, IOrder orderService)
        {
            _userService = userService;
            _orderService = orderService;
            _userManager = userManager;
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
                    CartId = user.CartID,
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

        public async Task<IActionResult> ConfirmEmail(string userID)
        {
            var user = _userService.GetByID(userID);

            user.EmailConfirmed = true;

            return RedirectToAction("Index");
        }


        [HttpPost]
        public async Task<IActionResult> UpdateUserProfile()
        {
            var username = Request.Form["username"];
            var phonenumber = Request.Form["phonenumber"];
            var email = Request.Form["email"];
            
            var user = await _userService.GetUser(User);

            if (user == null)
            {
                return StatusCode(404);
            }

            if (IsUsernameDiffrent(username))
            {
                user.UserName = username;

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Account");
                }

                return View();
            }
            else if (IsEmailDiffrent(email))
            {

                user.Email = email;

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Account");
                }

                return View();

            }
            else if (IsPhoneNumberDiffrent(phonenumber))
            {
                user.PhoneNumber = phonenumber;

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Account");
                }

                return View();

            }
            return View();

        }

        public IActionResult LatestOrders(int cartID)
        {
            var orders = _orderService.GetAllByCartID(cartID).Select(order => new OrderIndexModel 
            {
                CartID = order.CartID,
                City = order.City,
                Country = order.Country,
                ID = order.ID,
                Email = order.Email,
                Games = _orderService.GetGamesFromOrder(order.ID),
                PhoneNumber = order.PhoneNumber,
                Street = order.Street,
                TotalPrice = order.TotalPrice,
                UserID = order.UserID,
                Placed = order.Placed
            });


            var model = new AccountLatestOrdersModel 
            {
                Orders = orders
            };

            return View(model);
        }

        private bool IsPhoneNumberDiffrent(string phonenumber)
        {
            var user =  _userService.GetUser(User);

            if (user.Result.PhoneNumber == phonenumber)
                return false;

            return true;
        }

        private bool IsEmailDiffrent(string email)
        {
            var user = _userService.GetUser(User);

            if (user.Result.Email == email)
                return false;

            return true;
        }

        private bool IsUsernameDiffrent(string username)
        {
            var user = _userService.GetUser(User);

            if (user.Result.UserName == username)
                return false;

            return true;
        }
    }
}