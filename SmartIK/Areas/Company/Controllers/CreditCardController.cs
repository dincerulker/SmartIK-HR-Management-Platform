using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartIK.Areas.Company.Models;
using SmartIK.Data;
using System.Security.Claims;

namespace SmartIK.Areas.Company.Controllers
{
    [Authorize(Roles = "Company Manager")]
    [Area("Company")]
    public class CreditCardController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public CreditCardController(ApplicationDbContext db, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult AddCreditCard()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddCreditCard(AddCreditCardViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var user = GetUser();

            CreditCard creditCard = new()
            {
                CardName = vm.CardName,
                CardNumber = vm.CardNumber,
                Cvv = vm.Cvv,
                CardExpire = vm.Expire,
                Brand = vm.Brand,
                Bank = vm.Bank,
                CorporationId = user.CorparationId
            };
            _db.CreditCards.Add(creditCard);
            _db.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        //public IActionResult CreditCards()
        //{
        //    return View();
        //}

        private ApplicationUser GetUser()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return _db.Users.Include(x => x.Corparation).FirstOrDefault(x => x.Id == userId);
        }
    }
}