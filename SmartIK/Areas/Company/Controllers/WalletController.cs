using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SmartIK.Areas.Company.Models;
using SmartIK.Data;
using System.Security.Claims;

namespace SmartIK.Areas.Company.Controllers
{
    [Authorize(Roles = "Company Manager")]
    [Area("Company")]
    public class WalletController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public WalletController(ApplicationDbContext db, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(WalletCreateViewModel vm)
        {
            Wallet wallet = new()
            {
                Balance = vm.Balance,
                CorporationId = vm.CorporationId,
            };
            _db.Wallets.Add(wallet);
            _db.SaveChanges();
            return RedirectToAction("Wallets", "Wallet");
        }

        
        public IActionResult AddBalance()
        {
            var user = GetUser();
            var card = _db.CreditCards.Include(x => x.Corporation).Where(x => x.CorporationId == user.CorparationId).ToList();
            if (card == null)
            {
                return RedirectToAction("AddCreditCard", "CreditCard");
            }
            var model = new AddBalanceViewModel();
            model.CreditCarts = _db.CreditCards.Where(x => x.CorporationId == user.CorparationId ).Select(x => new SelectListItem
            {
                Text = x.CardNumber,
                Value = x.Id.ToString()
            }).ToList();
            return View(model);
        }

        [HttpPost]
        public IActionResult AddBalance(AddBalanceViewModel wallet)
        {
            var user = GetUser();
            user.Corparation.Wallet.Balance+= wallet.Balance;
            _db.SaveChanges();
            return RedirectToAction("Index", "Home", new { area = "Company" });

        }

        private ApplicationUser GetUser()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return _db.Users.Include(x => x.Corparation).ThenInclude(x => x.Wallet).FirstOrDefault(x => x.Id == userId);
        }
    }
}
