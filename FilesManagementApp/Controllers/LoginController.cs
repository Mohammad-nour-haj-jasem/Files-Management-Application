using FilesManagementApp.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FilesManagementApp.Controllers
{
    public class LoginController : Controller
    {
        private readonly List<string> allowedRandomUsernames = new List<string>
    {
        "mostapha@gmail.com",
        "’MohammadNour@gmail.com",
        "SamerAboSamra@gmail.com"
        // يمكنك إضافة المزيد من الحسابات العشوائية هنا
    };

        [Route("/Login")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("/Login")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            string randomUsername = GetRandomUsername();
            if (!string.IsNullOrEmpty(randomUsername))
            {
                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, randomUsername),
                new Claim(ClaimTypes.Role,"User"),
            };
                var claimIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimIdentity));

                return RedirectToAction("Index", "FileUploades");
            }
            return View();
        }

        [Route("/Logout")]
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            if (User.Identity.IsAuthenticated)
            {
                await HttpContext.SignOutAsync();
            }
            return RedirectToAction("Index", "Login");
        }

        private string GetRandomUsername()
        {
            Random random = new Random();
            int index = random.Next(allowedRandomUsernames.Count);
            return allowedRandomUsernames[index];
        }
    }

}