using Microsoft.AspNetCore.Mvc;
using MovieStore.Models.Identity;
using MovieStore.Repositories.Abstract;

namespace MovieStore.Controllers
{
    public class UserAuthenticationController : Controller
    {
        IUserAuthenticationService authService;

        public UserAuthenticationController(IUserAuthenticationService authService)
        {
            this.authService = authService;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegistrationModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            model.Role = "User";
            var status = await authService.RegisterAsync(model);
            TempData["msg"] = status.Message;
            return RedirectToAction("Register");
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid) {
                return View(model);
            }

            var status = await authService.LoginAsync(model);

            if (status.StatusCode == 1)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["msg"] = status.Message;
                return RedirectToAction("Login");
            }
        }

        public async Task<IActionResult> Logout()
        {
            await authService.LogoutAsync();
            return RedirectToAction("Index", "Home");
        }



        /*public async Task<IActionResult> CreateAdmin()
        {
            var model = new RegistrationModel
            {
                Name = "Admin",
                Email = "Admin@gmail.com",
                Username = "Admin",
                Password = "Admin@123",
                PasswordConfirm = "Admin@123",
                Role = "Admin",
            };

            var status = await authService.RegisterAsync(model);
            return Ok(status.Message);
        }*/
    }
}
