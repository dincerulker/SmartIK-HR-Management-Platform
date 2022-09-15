using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartIK.Areas.Company.Models;
using SmartIK.Data;
using System.Collections.Generic;
using System.Security.Claims;


namespace SmartIK.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Company")]
    public class AdvancePaymentController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;

        public AdvancePaymentController(ApplicationDbContext db, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<ApplicationUser> signInManager, IEmailSender emailSender)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var user = GetUser();
            AdvancePaymentUserViewModel vm = new AdvancePaymentUserViewModel();
            vm.ApplicationUser = user;
            vm.AdvancePayments = _db.AdvancePayment.Include(x => x.ApplicationUser).Where(x => x.ApplicationUser.Id == user.Id && x.ApplicationUser.CorparationId == user.CorparationId).ToList();
            return View(vm);
        }
        [HttpGet]
        public IActionResult APCreate()
        {
            AdvancePaymentCreateViewModel vm = new AdvancePaymentCreateViewModel();
            var user = GetUser();
            vm.applicationUser = user;

            return View(vm);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult APCreate(AdvancePaymentCreateViewModel vm)
        {

            if (!ModelState.IsValid) return BadRequest();
            var user = GetUser();
            if (vm.AdvanceAmount < 100 && vm.AdvanceAmount > (decimal)(user.Salary * 3.6))
            {
                return BadRequest();
            }
            vm.applicationUser = user;
            AdvancePayment advancePayment = new AdvancePayment();
            advancePayment.Status = SmartIK.Enums.StatusEnum.Waiting;
            advancePayment.RequestDate = DateTime.Now;
            advancePayment.AdvanceAmount = vm.AdvanceAmount;
            advancePayment.Description = vm.Description;
            advancePayment.ApplicationUserId = vm.applicationUser.Id;
            advancePayment.ApplicationUser = vm.applicationUser;
            _db.AdvancePayment.Add(advancePayment);
            _db.SaveChanges();
            return View(vm);

        }

        private ApplicationUser GetUser()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // kullanıcının ID getirir.
            return _db.Users.Find(userId);
        }

        public async Task<IActionResult> Advances()
        {
            var user = GetUser();
            var userIsManager = await _userManager.IsInRoleAsync(user, "Company Manager");
            var userIsEmployee = await _userManager.IsInRoleAsync(user, "Employee");

            var vm = new AdvanceViewModel();
            if (userIsEmployee)
                vm.Advances = _db.AdvancePayment.Where(x => x.ApplicationUserId == user.Id).ToList();
            else
                vm.Advances = _db.AdvancePayment.Include(x => x.ApplicationUser).Where(x => x.ApplicationUser.CorparationId == user.CorparationId).ToList();

            return View(vm);
        }

        [HttpPost, ValidateAntiForgeryToken]

        public async Task<IActionResult> Advances(AdvancePayment advancePayment, int? id)
        {
            if (id == null)
                return RedirectToAction("Advances", "AdvancePayment");

            var user = GetUser();
            var accept = Request.Form["Accept"];
            var decline = Request.Form["Decline"];

            AdvancePayment ap = _db.AdvancePayment.FirstOrDefault(x => x.Id == id);
            var employeeId = ap.ApplicationUserId;
            var employee = _db.ApplicationUsers.FirstOrDefault(x => x.Id == employeeId);
            var email = employee.Email;
            if (accept.Count == 1)
            {
                await _emailSender.SendEmailAsync(email, "Information", "Your advance request has been approved.");
                ap.Status = SmartIK.Enums.StatusEnum.Accept;
                user.AdvanceUsed += advancePayment.AdvanceAmount;
            }
            if (decline.Count == 1)
            {
                await _emailSender.SendEmailAsync(email, "Information", "Your advance request has been denied.");
                ap.Status = SmartIK.Enums.StatusEnum.Decline;
            }
            ap.ReplyDate = DateTime.Now;
            ap.AdvanceAmount = advancePayment.AdvanceAmount;
            ap.Description = advancePayment.Description;
            ap.ApplicationUserId = ap.ApplicationUserId;
            ap.RequestDate = advancePayment.RequestDate;
            ap.Response = advancePayment.Response;
            _db.SaveChanges();

            var vm = new AdvanceViewModel();

            var userIsManager = await _userManager.IsInRoleAsync(user, "Company Manager");
            var userIsEmployee = await _userManager.IsInRoleAsync(user, "Employee");

            if (userIsEmployee)
                vm.Advances = _db.AdvancePayment.Where(x => x.ApplicationUserId == user.Id).ToList();
            else
                vm.Advances = _db.AdvancePayment.Include(x => x.ApplicationUser).Where(x => x.ApplicationUser.CorparationId == user.CorparationId).ToList();

            return View(vm);
        }

        public IActionResult AdvanceReply(int id)
        {
            AdvancePayment advancePayment = _db.AdvancePayment.Include(x => x.ApplicationUser).FirstOrDefault(x => x.Id == id);
            return View(advancePayment);
        }
    }
}
