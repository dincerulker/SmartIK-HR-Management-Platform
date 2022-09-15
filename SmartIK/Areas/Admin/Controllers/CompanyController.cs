using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SmartIK.Areas.Admin.Models;
using SmartIK.Data;

namespace SmartIK.Areas.Admin.Controllers
{
    [Authorize(Roles ="Admin")]
    [Area("Admin")]
    public class CompanyController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _env;

        public CompanyController(ApplicationDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        public IActionResult AddCompany()
        {
            var vm = new AddCompanyViewModel();
            return View(vm);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult AddCompany(AddCompanyViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.CompanyLogo = null;
                return View(vm);
            }

            string extension = Path.GetExtension(vm.CompanyLogo.FileName);
            string fileName = Guid.NewGuid() + extension;
            string savePath = Path.Combine(_env.WebRootPath, "img", fileName);

            using (var stream = System.IO.File.Create(savePath))
            {
                vm.CompanyLogo.CopyTo(stream);
            }

            Wallet wallet = new()
            {
                Balance = 0
               
            };
            _db.Wallets.Add(wallet);
            _db.SaveChanges();

            Corporation corporation = new()
            {
                CompanyName = vm.CompanyName,
                Address = vm.Address,
                CompanyType = vm.CompanyType,
                Websites = vm.Websites,
                MailAddress = vm.MailAddress,
                MersisNumber = vm.MersisNumber,
                PhoneNumber = vm.PhoneNumber,
                TaxNumber = vm.TaxNumber,
                LogoUri = fileName,
                NumberOfEmployees = vm.NumberOfEmployees,
                WalletId = wallet.Id
            };

            _db.Corporations.Add(corporation);
            _db.SaveChanges();

            return RedirectToAction("Companies", "Company", new { area = "admin" });
        }

        public IActionResult Companies()
        {
            var vm = new CompanyViewModel()
            {
                Companies = _db.Corporations.Include(x => x.Wallet).ToList(),
            };
            return View(vm);
        }

        public IActionResult EditCompany(int? id)
        {
            if (id == null) return NotFound();

            Corporation corporation = _db.Corporations.FirstOrDefault(x => x.Id == id);

            if (corporation == null) return NotFound();

            EditCompanyViewModel vm = new()
            {
                Id = corporation.Id,
                CompanyName = corporation.CompanyName,
                PhoneNumber = corporation.PhoneNumber,
                ImagePath = corporation.LogoUri,
                //Country = corporation.Country,
                //City = corporation.City,
                MailAddress = corporation.MailAddress,
                WebSites = corporation.Websites,
                Address = corporation.Address,
                TaxNumber = corporation.TaxNumber,
                MersisNumber = corporation.MersisNumber,
                NumberOfEmployees = corporation.NumberOfEmployees
            };

            return View(vm);
        }

        public IActionResult DetailCompany(int id)
        {
            Corporation corporation = _db.Corporations.FirstOrDefault(x => x.Id == id);

            if (corporation == null) return NotFound();

            DetailCompanyViewModel vm = new()
            {
                Id = corporation.Id,
                CompanyName = corporation.CompanyName,
                PhoneNumber = corporation.PhoneNumber,
                LogoUri = corporation.LogoUri,
                //Country = corporation.Country,
                //City = corporation.City,
                MailAddress = corporation.MailAddress,
                WebSites = corporation.Websites,
                Address = corporation.Address,
                TaxNumber = corporation.TaxNumber,
                MersisNumber = corporation.MersisNumber,
                CompanyType = corporation.CompanyType,
                NumberOfEmployees = corporation.NumberOfEmployees
                
            };

            return View(vm);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult EditCompany(int? id, EditCompanyViewModel vm)
        {
            if (id != vm.Id) return NotFound();

            if (!ModelState.IsValid)
            {
                vm.CompanyLogo = null;
                return View(vm);
            }

            Corporation corporation = _db.Corporations.FirstOrDefault(x => x.Id == vm.Id);
            if (corporation == null) return NotFound();

            if (vm.CompanyLogo != null)
            {
                string extension = Path.GetExtension(vm.CompanyLogo.FileName);
                string fileName = Guid.NewGuid() + extension;
                string savePath = Path.Combine(_env.WebRootPath, "img", fileName);

                using (var stream = System.IO.File.Create(savePath))
                {
                    vm.CompanyLogo.CopyTo(stream);
                }

                corporation.LogoUri = fileName;
            }

            corporation.CompanyName = vm.CompanyName;
            corporation.Address = vm.Address;
            //corporation.Country = vm.Country;
            corporation.CompanyType = vm.CompanyType;
            corporation.Websites = vm.WebSites;
            corporation.MailAddress = vm.MailAddress;
            corporation.MersisNumber = vm.MersisNumber;
            corporation.PhoneNumber = vm.PhoneNumber;
            corporation.TaxNumber = vm.TaxNumber;
            corporation.NumberOfEmployees = vm.NumberOfEmployees;
            //corporation.City = vm.City;

            _db.Corporations.Update(corporation);
            _db.SaveChanges();
            return RedirectToAction("Companies", "Dashboard", new { area = "admin" });

        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            Corporation corporation = await _db.Corporations.FirstOrDefaultAsync(x => x.Id == id);
            if (corporation == null) return NotFound();
            _db.Corporations.Remove(corporation);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index", "Dashboard", new { area = "admin" });
        }
    }
}
