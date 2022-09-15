using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartIK.Areas.Company.Models;
using SmartIK.Data;
using SmartIK.Enums;
using System.Security.Claims;

namespace SmartIK.Areas.Company.Controllers
{
    [Authorize]
    [Area("Company")]
    public class PermissionController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;

        public PermissionController(ApplicationDbContext db, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<ApplicationUser> signInManager, IEmailSender emailSender)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
        }

        public async Task<IActionResult> Index()
        {
            var user = GetUser();
            if (user == null) return NotFound();
            var userIsManager = await _userManager.IsInRoleAsync(user, "Company Manager");
            var userIsEmployee = await _userManager.IsInRoleAsync(user, "Employee");
            if (userIsEmployee)
                return View(_db.Permissions.Include(x => x.ApplicationUser).ThenInclude(x => x.Corparation).Where(x => x.ApplicationUserId == user.Id && x.ApplicationUser.CorparationId == user.CorparationId).ToList());
            else
                return View(_db.Permissions.Include(x => x.ApplicationUser).ThenInclude(x => x.Corparation).Where(x => x.ApplicationUser.CorparationId == user.CorparationId).ToList());
        }


        [HttpGet]
        public IActionResult PMCreate()
        {
            PermissionCreateViewModel vm = new PermissionCreateViewModel();
            vm.ApplicationUser = GetUser();
            return View(vm);
        }
        [HttpPost]
        public IActionResult PMCreate(PermissionCreateViewModel vm)
        {
            if (!ModelState.IsValid) return BadRequest();
            Permission permission = new Permission();
            var user = GetUser();
            permission.ApplicationUserId = user.Id;
            permission.ApplicationUser = GetUser();
            permission.PermissionStartDate = vm.PermissionStartDate;
            permission.PermissionName = vm.PermissionType.ToString();


            if (vm.PermissionType == Enums.PermissionType.DeathPermission)
            {
                permission.PermissionEndDate = vm.PermissionStartDate.AddDays(3);
                permission.PermissionDays = 3;
            }
            if (vm.PermissionType == Enums.PermissionType.JobSearchPermission)
            {
                permission.PermissionEndDate = vm.PermissionStartDate.AddDays(3);
                permission.PermissionDays = 3;
            }
            if (vm.PermissionType == Enums.PermissionType.WeddingPermission)
            {
                permission.PermissionEndDate = vm.PermissionStartDate.AddDays(3);
                permission.PermissionDays = 3;
            }
            if (vm.PermissionType == Enums.PermissionType.AnnualPermission)
            {
                permission.PermissionEndDate = vm.PermissionEndDate;
                permission.PermissionDays = vm.PermissionEndDate.Day - vm.PermissionStartDate.Day;
            }
            if (vm.PermissionType == Enums.PermissionType.FreePermission)
            {
                permission.PermissionEndDate = vm.PermissionEndDate;
                permission.PermissionDays = vm.PermissionEndDate.Day - vm.PermissionStartDate.Day;
            }
            if (vm.PermissionType == Enums.PermissionType.PaternityLeave && user.Gender == GenderEnum.Male)
            {
                permission.PermissionEndDate = vm.PermissionStartDate.AddDays(5);
                permission.PermissionDays = 5;
            }
            else if (vm.PermissionType == Enums.PermissionType.MaternityLeave && user.Gender == GenderEnum.Female)
            {
                permission.PermissionEndDate = vm.PermissionStartDate.AddDays(112);
                permission.PermissionDays = 112;
            }
            else if (vm.PermissionType == Enums.PermissionType.MaternityLeave && user.Gender == GenderEnum.Male)
            {
                TempData["Message"] = "You can't take maternity leave..!";
                return View(vm);
            }
            else if (vm.PermissionType == Enums.PermissionType.PaternityLeave && user.Gender == GenderEnum.Female)
            {
                TempData["Message"] = "You can't take paternity leave..!";
                return View(vm);
            }

            _db.Permissions.Add(permission);
            _db.SaveChanges();
            return RedirectToAction("Permissions");
        }

        public async Task<IActionResult> Permissions()
        {
            var user = GetUser();
            var userIsManager = await _userManager.IsInRoleAsync(user, "Company Manager");
            var userIsEmployee = await _userManager.IsInRoleAsync(user, "Employee");
            var vm = new PermissionViewModel();
            if (userIsEmployee)
                vm.Permissions = _db.Permissions.Include(x => x.ApplicationUser).ThenInclude(x => x.Corparation).Where(x => x.ApplicationUserId == user.Id && x.ApplicationUser.CorparationId == user.CorparationId).ToList();
            else
                vm.Permissions = _db.Permissions.Include(x => x.ApplicationUser).ThenInclude(x => x.Corparation).Where(x => x.ApplicationUser.CorparationId == user.CorparationId).ToList();
            return View(vm);
        }

        [Authorize(Roles = "Company Manager")]
        [HttpPost]
        public async Task<IActionResult> PermissionReply(Permission permission, int id)
        {
            var user = GetUser();
            var accept = Request.Form["Accept"];
            var decline = Request.Form["Decline"];
            var per = _db.Permissions.FirstOrDefault(x => x.Id == id);


            if (accept.Count == 1)
            {
                await _emailSender.SendEmailAsync(user.Email, "Information", "Your permission request has been approved.");
                per.Status = SmartIK.Enums.StatusEnum.Accept;

            }
            if (decline.Count == 1)
            {
                await _emailSender.SendEmailAsync(user.Email, "Information", "Your permission request has been denied.");
                per.Status = SmartIK.Enums.StatusEnum.Decline;
            }
            //_db.Permissions.Update(permission);
            user.UsingPermissionDays += permission.PermissionDays;

            _db.SaveChanges();

            //var vm = new PermissionViewModel();

            //var userIsManager = await _userManager.IsInRoleAsync(user, "Company Manager");
            //var userIsEmployee = await _userManager.IsInRoleAsync(user, "Employee");
            //if (userIsEmployee)
            //    vm.Permissions = _db.Permissions.Include(x => x.ApplicationUser).ThenInclude(x => x.Corparation).Where(x => x.ApplicationUserId == user.Id && x.ApplicationUser.CorparationId == user.CorparationId).ToList();
            //else
            //    vm.Permissions = _db.Permissions.Include(x => x.ApplicationUser).ThenInclude(x => x.Corparation).Where(x => x.ApplicationUser.CorparationId == user.CorparationId).ToList();

            return RedirectToAction("Permissions");
        }
        private ApplicationUser GetUser()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // kullanıcının ID getirir.
            return _db.Users.Find(userId);

        }

        public IActionResult PermissionReply(int id)
        {
            var permission = _db.Permissions.Include(x => x.ApplicationUser).FirstOrDefault(x => x.Id == id);
            return View(permission);
        }
    }
}
