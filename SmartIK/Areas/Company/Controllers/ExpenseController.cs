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
    public class ExpenseController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailSender _emailSender;
        private readonly IWebHostEnvironment _env;

        public ExpenseController(ApplicationDbContext db, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IEmailSender emailSender, IWebHostEnvironment env)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
            _emailSender = emailSender;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            var user = GetUser();
            var userIsManager = await _userManager.IsInRoleAsync(user, "Company Manager");
            var userIsEmployee = await _userManager.IsInRoleAsync(user, "Employee");
            if (userIsEmployee)
                return View(_db.Expenses
                    .Include(x => x.ApplicationUser)
                    .ThenInclude(x => x.Corparation)
                    .Where(x => x.ApplicationUserId == user.Id && x.ApplicationUser.CorparationId == user.CorparationId)
                    .ToList());
            if (userIsManager)
                return View(_db.Expenses
                    .Include(x => x.ApplicationUser)
                    .ThenInclude(x => x.Corparation)
                    .Where(x => x.ApplicationUser.CorparationId == user.CorparationId).ToList());
            return NotFound();
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(AddExpenseViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.File = null;
                return BadRequest(ModelState);
            }
            if (vm.File == null) return BadRequest(ModelState);
            var user = GetUser();
            Expense expense = new();
            expense.Status = SmartIK.Enums.StatusEnum.Waiting;
            expense.ApplicationUserId = user.Id;
            expense.Description = vm.Description;
            expense.ExpenseType = vm.ExpenseType;
            expense.Price = vm.Price;
            try
            {
                string extension = Path.GetExtension(vm.File.FileName);
                string fileName = Guid.NewGuid() + extension;
                string savePath = Path.Combine(_env.WebRootPath, "expense-files/", fileName);

                using (var stream = System.IO.File.Create(savePath))
                {
                    vm.File.CopyTo(stream);
                }
                expense.FilePath = fileName;
            }
            catch (Exception)
            {
                return BadRequest();
            }

            _db.Expenses.Add(expense);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            Expense expense = _db.Expenses.FirstOrDefault(x => x.Id == id);
            if (expense == null) return NotFound();

            EditExpenseViewModel vm = new()
            {
                Id = expense.Id,
                Description = expense.Description,
                ExpenseType = expense.ExpenseType,
                Price = expense.Price,
                FilePath = expense.FilePath
            };
            return View(vm);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(EditExpenseViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            Expense expense = _db.Expenses.FirstOrDefault(x => x.Id == vm.Id);
            if (expense == null) return NotFound();

            var user = GetUser();
            expense.ApplicationUserId = user.Id;
            expense.Id = vm.Id;
            expense.Description = vm.Description;
            expense.Price = vm.Price;
            expense.ExpenseType = vm.ExpenseType;

            if (vm.File != null)
            {
                string extension = Path.GetExtension(vm.File.FileName);
                string fileName = Guid.NewGuid() + extension;
                string savePath = Path.Combine(_env.WebRootPath, "expense-files/", fileName);

                using (var stream = System.IO.File.Create(savePath))
                {
                    vm.File.CopyTo(stream);
                }
                expense.FilePath = fileName;
            }
            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult ExpenseReply(int id)
        {
            var expense = _db.Expenses.Include(x => x.ApplicationUser).FirstOrDefault(x => x.Id == id);
            if (expense == null) return NotFound();
            return View(expense);
        }


        [HttpPost]
        [Authorize(Roles = "Company Manager")]
        public IActionResult ExpenseReply(int id, Expense expense)
        {
            if (id != expense.Id)
            {
                return NotFound();
            }

            var user = GetUser();
            var accept = Request.Form["Accept"];
            var decline = Request.Form["Decline"];
            var exp = _db.Expenses.FirstOrDefault(x => x.Id == id);


            if (accept.Count == 1)
            {
                //await _emailSender.SendEmailAsync(user.Email, "Information", "Your permission request has been approved.");
                exp.Status = SmartIK.Enums.StatusEnum.Accept;

            }
            if (decline.Count == 1)
            {
                //await _emailSender.SendEmailAsync(user.Email, "Information", "Your permission request has been denied.");
                exp.Status = SmartIK.Enums.StatusEnum.Decline;
            }

            if (expense.Response != null)
            {
                exp.Response = expense.Response;
                exp.ResponseDate = DateTime.Now;
            }
            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        private ApplicationUser GetUser()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // kullanıcının ID getirir.
            return _db.Users.Find(userId);
        }

        [HttpGet]
        public IActionResult GetFile(int id)
        {
            var expance = _db.Expenses.Find(id);
            if (expance == null) return NotFound();
            var path = $"wwwroot/expense-files/{expance.FilePath}";
            using (var stream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Read))
            {
                return Ok(stream.Name);
            }
        }
    }
}
