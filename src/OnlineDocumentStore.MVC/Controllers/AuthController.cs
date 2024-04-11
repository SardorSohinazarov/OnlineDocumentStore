using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineDocumentStore.Application.DataTransferObjects.Auth;
using OnlineDocumentStore.Domain.Entities;
using OnlineDocumentStore.Domain.Exceptions;

namespace OnlineDocumentStore.MVC.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AuthController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
            => View();

        public async Task<IActionResult> Login()
            => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            var user = await _userManager.FindByEmailAsync(loginDTO.Email);
            if (user == null)
                throw new Exception("User not found");

            var result = await _signInManager.PasswordSignInAsync(user, loginDTO.Password, false, false);

            if (!result.Succeeded)
                throw new Exception("There is an issue with signing in process");

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Register()
            => View();

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDTO registerDTO)
        {
            var user = await _userManager.FindByEmailAsync(registerDTO.Email);

            if (registerDTO.Password != registerDTO.ConfirmPassword)
                throw new ValidationException("Passwords do not match!");

            if (user is not null)
                throw new ValidationException("You are already registred");

            user = new User()
            {
                UserName = registerDTO.Email,
                FullName = registerDTO.FullName,
                Email = registerDTO.Email,
                PhoneNumber = registerDTO.PhoneNumber,
                Salt = Guid.NewGuid().ToString(),
                ExpireDate = DateTime.Now.AddDays(2),
                RefreshToken = Guid.NewGuid().ToString(),
            };

            var identityResult = await _userManager.CreateAsync(user, registerDTO.Password);

            if (!identityResult.Succeeded)
                throw new Exception("There is an issue with signing in process");

            var result =
                await _signInManager.PasswordSignInAsync(user, registerDTO.Password, false, false);

            if (!result.Succeeded)
                throw new Exception("There is an issue with signing in process");

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Login");
        }
    }
}
