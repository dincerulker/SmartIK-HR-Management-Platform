using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartIK.Areas.Admin.Models;
using SmartIK.Data;

namespace SmartIK.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _db;

        public DashboardController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            var packages = _db.Packages.ToList();
            var vm = new AdminIndexViewModel()
            {
                Packages = packages.Where(x => x.IsDelete == false).ToList(),
                ActivePackageCount = packages.Where(x => x.IsActive == true && x.IsDelete == false).Count(),
                PassivePackageCount = packages.Where(x => x.IsActive == false && x.IsDelete == false).Count(),
                DeletedPackageCount = packages.Where(x => x.IsDelete == true).Count()
            };
            vm.Users = _db.Users.Include(x => x.Corparation).Where(x => x.CorparationId != null && x.IsDelete == false).ToList();

            return View(vm);
        }
    }
}
