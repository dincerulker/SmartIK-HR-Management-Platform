using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using SmartIK.Areas.Admin.Data;
using SmartIK.Areas.Admin.Models;
using SmartIK.Data;
using SmartIK.Models;
using System.Text;
using System.Text.Encodings.Web;

namespace SmartIK.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly IWebHostEnvironment _env;

        public UserController(ApplicationDbContext db, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<ApplicationUser> signInManager, IEmailSender emailSender, IWebHostEnvironment env)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _env = env;

        }
        public IActionResult Index()
        {
            return View(_db.Users.Include(x => x.Corparation).Where(x => x.CorparationId != null && x.IsDelete == false).ToList());
        }

        public IActionResult CMRegister()
        {
            ApplicationUserViewModel vm = new ApplicationUserViewModel();
            vm.Corparations = _db.Corporations.Select(x => new SelectListItem()
            {
                Text = x.CompanyName,
                Value = x.Id.ToString()
            }).ToList();
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> CMRegister(ApplicationUserViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.Corparations = _db.Corporations.Select(x => new SelectListItem()
                {
                    Text = x.CompanyName,
                    Value = x.Id.ToString()
                }).ToList();
                return View(vm);
            }

            var userCheck = _db.Users.FirstOrDefault(x => x.Email == vm.Email);
            if (userCheck != null)
            {
                ViewBag.UserExist = $"{vm.Email} already exist, please entry another e-mail adress";
                vm.Corparations = _db.Corporations.Select(x => new SelectListItem()
                {
                    Text = x.CompanyName,
                    Value = x.Id.ToString()
                }).ToList();
                return View(vm);
            }

            GeneratePassword rastgele = new GeneratePassword();
            int sayi = rastgele.RastgeleSayi(1, 10);
            string yazi = rastgele.RasteleHarf(2, true);
            string sifre = rastgele.sifreUret();

            string extension = Path.GetExtension(vm.Image.FileName);
            string fileName = Guid.NewGuid() + extension;
            string savePath = Path.Combine(_env.WebRootPath, "img/", fileName);

            using (var stream = System.IO.File.Create(savePath))
            {
                vm.Image.CopyTo(stream);
            }
            var user1 = new ApplicationUser()
            {
                SecondName = vm.SecondName,
                Salary = vm.Salary.Value,
                EmailConfirmed = true,
                UserName = vm.Email,
                Address = vm.Address,
                BloodType = vm.BloodType,
                CorparationId = vm.CorparationId,
                CreatedTime = DateTime.Now,
                Email = vm.Email,
                FirstName = vm.FirstName,
                IsActive = true,
                LastName = vm.LastName,
                Gender = vm.Gender,
                Phone = vm.Phone,
                TCKN = vm.TCKN,
                UpdatedDate = DateTime.Now,
                Title = vm.Title,
                PhoneNumber = vm.PhoneNumber,
                IsDelete = false,
            };

            var workingYears = (int)(Math.Floor((DateTime.Now - vm.HiringDate).Value.TotalDays / 365));

            if (workingYears < 1)
                user1.MaxPermissionDays = 0;
            else if (workingYears >= 1 && workingYears < 6)
                user1.MaxPermissionDays = 14;
            else if (workingYears <= 6 && workingYears > 15)
                user1.MaxPermissionDays = 20;
            else
                user1.MaxPermissionDays = 26;

            if (vm.Image == null)
                user1.PicturePath = "unknown.jpg";
            user1.PicturePath = fileName;
            if (vm.HiringDate <= vm.BirthDate)
            {
                ViewBag.ErrorMessage = "Hiring Date can not bigger than Birth Date";
                vm.Image = null;
                vm.Corparations = _db.Corporations.Select(x => new SelectListItem()
                {
                    Text = x.CompanyName,
                    Value = x.Id.ToString()
                }).ToList();
                return View(vm);
            }
            user1.BirthDate = vm.BirthDate;
            user1.HiringDate = vm.HiringDate;
            await _userManager.CreateAsync(user1, "Smartik.06");
            await _userManager.AddToRoleAsync(user1, "Company Manager");
            await _db.SaveChangesAsync();

            var code = await _userManager.GeneratePasswordResetTokenAsync(user1);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = Url.Page(
                    "/Account/ResetPassword",
                    pageHandler: null,
                    values: new { area = "Identity", code },
                    protocol: Request.Scheme);

            await _emailSender.SendEmailAsync(
                vm.Email,
                "Password",
                $"Please create a new password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

            return RedirectToAction("Index", "User", new { area = "admin" });
        }

        [HttpGet]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _db.Users.FindAsync(id);
            if (user != null)
            {
                user.IsDelete = true;
                await _db.SaveChangesAsync();
                return RedirectToAction("Index", "User", new { area = "admin" });
            }
            return BadRequest(ModelState);
        }

        public async Task<IActionResult> Details(string id)
        {
            var user = await _db.Users.FindAsync(id);
            var companyıd = user.CorparationId;
            var company = await _db.Corporations.FindAsync(companyıd);
            UserDetailViewModel vm = new UserDetailViewModel();
            if (user != null)
            {
                vm.ApplicationUser = user;
                vm.Gender = user.Gender.ToString();
                vm.CompanyName = company.CompanyName.ToString();
                return View(vm);
            }
            return BadRequest(ModelState);
        }
    }
}
