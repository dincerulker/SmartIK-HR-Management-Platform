using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartIK.Areas.Company.Models;
using SmartIK.Data;
using System.Security.Claims;

namespace SmartIK.Areas.Company.Controllers
{
    [Authorize]
    [Area("Company")]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public HomeController(ApplicationDbContext db, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<ApplicationUser> signInManager, IEmailSender emailSender)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }
        [HttpGet]
        public IActionResult BuyPackage()
        {
            HomeViewModel vm = new HomeViewModel();
            vm.Packages = _db.Packages.ToList();
            return View(vm);

        }
       
        public IActionResult BuyPackageDetail(int id)
        {
          
            var user = GetUser();
            var package = _db.Packages.FirstOrDefault(x => x.Id == id);
            BuyPackageViewModel vm = new BuyPackageViewModel();
            vm.Package = package;
            vm.CorporationId = (int)user.CorparationId;
            vm.PackageId = id;
            
            
           
           return View(vm);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public  IActionResult BuyPackageDetail(BuyPackageViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var package = _db.Packages.FirstOrDefault(x=> x.Id == vm.PackageId);
            var user = GetUser();
            var copmany = _db.Corporations.FirstOrDefault(x => x.Id == user.CorparationId);
            var wallet = _db.Wallets.FirstOrDefault(x=> x.Id == copmany.WalletId);
            if (wallet == null) return BadRequest();
            int companybalance = (int)copmany.Wallet.Balance;
            if (wallet.Balance == null)
            {
                // para yükleme sayfasına göndersin
                return RedirectToAction(nameof(Index));
            }
            if (wallet.Balance <= package.PackagePrice)
            {
                // para yükleme sayfasına göndersin
                return RedirectToAction(nameof(Index));
            }
            else
            {

                copmany.PackageId = package.Id;
                
                wallet.Balance -= (int)package.PackagePrice;

            }
            _db.Update(copmany);
            _db.Update(wallet);
            _db.SaveChanges();

            return RedirectToAction(nameof(Index));

        }

        public IActionResult Index()
        {
            var user = GetUser();
            var company = _db.Corporations.Include(x => x.CorporationUsers).Include(x => x.Wallet).FirstOrDefault(x => x.Id == user.CorparationId);
            if (company == null) return NotFound();

            var vm = new HomeViewModel();
            var advancepaments = _db.AdvancePayment.Where(x => x.ApplicationUserId == user.Id && x.Status == SmartIK.Enums.StatusEnum.Accept).ToList();
            vm.Corporation = company;
            vm.ApplicationUser = user;
            vm.Wallet = company.Wallet;
            if (user.Corparation.Package != null)
            {
                vm.HavePackage = true;
            }
            else
            {
                vm.HavePackage = false;
                vm.Packages = _db.Packages.ToList();
            }

            if (user.UsingPermissionDays == null)
            {
                vm.RemaningPermission = 14;
            }
            else
            {
                vm.RemaningPermission = (int)(user.MaxPermissionDays - user.UsingPermissionDays ?? 0);
            }
            decimal usedadvance = 0;

            foreach (var item in advancepaments)
                usedadvance += item.AdvanceAmount;

            vm.UsedAdvance = usedadvance;
            return View(vm);
        }

        private ApplicationUser GetUser()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // kullanıcının ID getirir.
            return _db.Users.Include(x => x.Permissions).FirstOrDefault(x => x.Id == userId);
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        public IActionResult AddEmployee()
        {
            return View();
        }


    }
}
