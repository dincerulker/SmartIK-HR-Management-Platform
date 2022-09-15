using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using SmartIK.Areas.Admin.Data;
using SmartIK.Areas.Company.Models;
using SmartIK.Data;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

namespace SmartIK.Areas.Company.Controllers
{
    [Authorize(Roles = "Company Manager")]
    [Area("Company")]
    public class EmployeeController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailSender _emailSender;
        private readonly IWebHostEnvironment _env;
        private Corporation currentCorparation;
        public EmployeeController(ApplicationDbContext db, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IEmailSender emailSender, IWebHostEnvironment env)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
            _emailSender = emailSender;
            _env = env;
        }

        [Authorize(Roles = "Company Manager")]
        public IActionResult List()
        {
            var user = GetUser();
            if (user == null) return NotFound();
            return View(_db.Users.Where(x => x.CorparationId == user.CorparationId).ToList());
        }

        public IActionResult CreateEmployee()
        {
            var vm = new AddEmployeeViewModel();
            var user = GetUser();
            var userCount = _db.Users.Where(x => x.CorparationId == user.CorparationId).Count();
            if (userCount < user.Corparation.Package.PersonalCount)
            {

                if (user.Corparation.Package != null)
                {
                    vm.TotalEmployee = user.Corparation.Package.PersonalCount;
                    // Arkadaşlar Burayı sadece yatağıını hazırlamak için ekledim. Paketin Maks personel sayisi olmadığı için sorguyu tamamlayamadım 
                    if (vm.TotalEmployee < 100)
                    {
                        return BadRequest();
                    }
                }
                return View(vm);
            }
            else
            {
                ViewBag.EmployeeError = "You have the maximum number of personnel. Please purchase the higher package to add new employees.";
                return View();
            }
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateEmployee(AddEmployeeViewModel vm)
        {

            if (ModelState.IsValid)
            {
                vm.Image = null;
                return View(vm);
            }

            int Age = vm.HiringDate.Value.Year - vm.BirthDate.Value.Year;
            if (Age < 16)
            {
                ViewBag.AgeErorr = $"Employees under the age of 16 cannot be employed.";
                return View(vm);
            }

            var userCheck = _db.Users.FirstOrDefault(x => x.Email == vm.Email);
            if (userCheck != null)
            {
                ViewBag.UserExist = $"{vm.Email} already exist, please entry another e-mail adress";
                return View(vm);
            }
            var currentUser = GetUser();
            currentCorparation = currentUser.Corparation;

            var user = new ApplicationUser()
            {
                FirstName = vm.FirstName,
                SecondName = vm.SecondName,
                LastName = vm.LastName,
                TCKN = vm.TCKN,
                Address = vm.Address,
                BirthDate = vm.BirthDate,
                BloodType = vm.BloodType,
                CorparationId = currentCorparation.Id,
                Gender = vm.Gender,
                IsActive = true,
                HiringDate = vm.HiringDate,
                Title = vm.Title,
                Salary = vm.Salary,
                CreatedTime = DateTime.Now,
                UserName = vm.Email,
                PhoneNumber = vm.PhoneNumber,

            };

            user.Email = (vm.FirstName + "." + vm.LastName + "@bilgeadamboost.com").ToLower().Trim();

            var workingYears = (int)(Math.Floor((DateTime.Now - vm.HiringDate).Value.TotalDays / 365));

            if (workingYears < 1)
                user.MaxPermissionDays = 0;
            else if (workingYears >= 1 && workingYears < 6)
                user.MaxPermissionDays = 14;
            else if (workingYears <= 6 && workingYears > 15)
                user.MaxPermissionDays = 20;
            else
                user.MaxPermissionDays = 26;


            GeneratePassword rastgele = new GeneratePassword();
            int sayi = rastgele.RastgeleSayi(1, 10);
            string yazi = rastgele.RasteleHarf(2, true);
            string sifre = rastgele.sifreUret();

            if (vm.Image == null)
                user.PicturePath = "unknown.jpg";
            else
            {
                string extension = Path.GetExtension(vm.Image.FileName);
                string fileName = Guid.NewGuid() + extension;
                string savePath = Path.Combine(_env.WebRootPath, "img/", fileName);
                using (var stream = System.IO.File.Create(savePath))
                {
                    vm.Image.CopyTo(stream);
                }
            }

            await _userManager.CreateAsync(user, sifre);
            await _userManager.AddToRoleAsync(user, "Employee");
            await _db.SaveChangesAsync();

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
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



            return RedirectToAction("List", "Emplooye", new { area = "Company" });
        }


        private ApplicationUser GetUser()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return _db.Users.Include(x => x.Corparation).ThenInclude(x => x.Package).FirstOrDefault(x => x.Id == userId);
        }

        //private Corporation GetCompany(int corparationId)
        //{
        //    return _db.Corporations.Include(x => x.CorporationUsers).FirstOrDefault(x => x.Id == corparationId);
        //}

    }
}
