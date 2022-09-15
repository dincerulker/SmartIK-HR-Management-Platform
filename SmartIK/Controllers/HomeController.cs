using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using SmartIK.Data;
using SmartIK.Models;
using System.Diagnostics;
using System.Security.Claims;

namespace SmartIK.Controllers
{

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<ApplicationUser> signInManager, IEmailSender emailSender)
        {
            _logger = logger;
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // kullanıcının ID getirir.
            var user = _db.Users.Find(userId);
            var userIsAdmin = await _userManager.IsInRoleAsync(user, "admin");
            var userIsManager = await _userManager.IsInRoleAsync(user, "Company Manager");
            var userIsEmployee = await _userManager.IsInRoleAsync(user, "Employee");

            if (user != null)
            {
                if (userIsAdmin)
                    TempData["Greetings"] = $"{user.FirstName} {user.LastName} Welcome to SMART IK Admin Panel";
                else
                    TempData["Greetings"] = $"{user.FirstName} {user.LastName} Welcome to SMART IK Company Panel";
                if (_signInManager.IsSignedIn(User))
                {
                    if (userIsManager || userIsEmployee)
                        return RedirectToAction("Index", "Home", new { area = "Company" });
                    if (userIsAdmin)
                        return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
                }
            }
            return View();
        }

        [Authorize]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}