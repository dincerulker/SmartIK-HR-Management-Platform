using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartIK.Areas.Admin.Models;
using SmartIK.Data;

namespace SmartIK.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class PackageController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _env;

        public PackageController(ApplicationDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }
        public IActionResult AddPackage()
        {
            return View();
        }
        [HttpGet]
        public IActionResult BuyPackage()
        {
            return View();
        }


        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult AddPackage(AddPackageViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.Image = null;
                return View(vm);
            }

            if (vm.PublishStartDate > vm.PublishEndDate)
            {
                ViewBag.ErrorMessage = "Publish start date cannot be later than publish end date";
                return View(vm);
            }
            var existPackage = _db.Packages.FirstOrDefault(x => x.PackageName.ToLower() == vm.PackageName.ToLower());
            if (existPackage != null)
            {
                ViewBag.ExistPackage = "This package name is already exist";
                return View(vm);
            }
            if (vm.PublishStartDate > DateTime.Now.AddDays(-1))
                vm.IsActive = false;

            string extension = Path.GetExtension(vm.Image.FileName);
            string fileName = Guid.NewGuid() + extension;
            string savePath = Path.Combine(_env.WebRootPath, "img", fileName);

            using (var stream = System.IO.File.Create(savePath))
            {
                vm.Image.CopyTo(stream);
            }
            Package package = new()
            {
                IsActive = vm.IsActive,
                CreatedDate = DateTime.Now,
                PackageDescription = vm.PackageDescription,
                PackageName = vm.PackageName,
                PackagePrice = vm.PackagePrice,
                PublishStartDate = vm.PublishStartDate,
                PublishEndDate = vm.PublishEndDate,
                PicturePath = fileName,
            };
            _db.Packages.Add(package);
            _db.SaveChanges();
            return RedirectToAction("Packages", "Package", new { area = "admin" });
        }

        public IActionResult EditPackage(int id)
        {
            Package package = _db.Packages.FirstOrDefault(x => x.Id == id);
            if (package == null) return NotFound();

            EditPackageViewModel vm = new()
            {
                Id = package.Id,
                PackageName = package.PackageName,
                PackageDescription = package.PackageDescription,
                PackagePrice = package.PackagePrice,
                PublishStartDate = package.PublishStartDate,
                PublishEndDate = package.PublishEndDate,
                PicturePath = package.PicturePath
            };
            return View(vm);
        }

        public IActionResult DetailPackage(int id)
        {
            Package package = _db.Packages.Include(x => x.Companies).FirstOrDefault(x => x.Id == id);
            if (package == null) return NotFound();

            DetailPackageViewModel vm = new()
            {
                Id = package.Id,
                PackageName = package.PackageName,
                PackageDescription = package.PackageDescription,
                PackagePrice = package.PackagePrice,
                PublishStartDate = package.PublishStartDate,
                PublishEndDate = package.PublishEndDate,
                CreatedDate = package.CreatedDate,
                PicturePath = package.PicturePath,
                Companies = package.Companies,
            };

            return View(vm);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult EditPackage(EditPackageViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            Package package = _db.Packages.FirstOrDefault(x => x.Id == vm.Id);
            if (package == null) return NotFound();


            if (vm.Image != null)
            {
                string extension = Path.GetExtension(vm.Image.FileName);
                string fileName = Guid.NewGuid() + extension;
                string savePath = Path.Combine(_env.WebRootPath, "img", fileName);
                using (var stream = System.IO.File.Create(savePath))
                {
                    vm.Image.CopyTo(stream);
                }
                package.PicturePath = fileName;
            }
            package.PackageName = vm.PackageName;
            package.PublishStartDate = vm.PublishStartDate.Value;
            package.PublishEndDate = vm.PublishEndDate.Value;
            package.PackageDescription = vm.PackageDescription;
            package.PackagePrice = vm.PackagePrice;

            _db.SaveChanges();
            Update();
            return RedirectToAction("Packages", "Package", new { area = "admin" });
        }

        public IActionResult Packages()
        {
            Update();
            var vm = new PackageViewModel()
            {
                Packages = _db.Packages.Where(x => x.IsDelete == false).ToList()
            };
            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            Package package = await _db.Packages.FirstOrDefaultAsync(x => x.Id == id);
            if (package == null) return NotFound();
            package.IsDelete = true;
            package.PublishEndDate = DateTime.Now;
            _db.Packages.Update(package);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index", "Dashboard", new { area = "admin" });
        }

        public IActionResult ActivePackages()
        {
            Update();
            var vm = new PackageViewModel()
            {
                Packages = _db.Packages.Where(x => x.IsActive == true && x.IsDelete == false).ToList()
            };
            return View(vm);
        }

        public IActionResult PassivePackages()
        {
            Update();
            var vm = new PackageViewModel()
            {
                Packages = _db.Packages.Where(x => x.IsActive == false && x.IsDelete == false).ToList()
            };
            return View(vm);
        }

        public IActionResult DeletedPackages()
        {
            var vm = new PackageViewModel()
            {
                Packages = _db.Packages.Where(x => x.IsDelete == true).ToList()
            };
            return View(vm);
        }

        [HttpPost]
        public IActionResult DeletedPackages(int id)
        {
            var package = _db.Packages.FirstOrDefault(x => x.Id == id);
            if (package !=null)
            {
                package.IsDelete = false;
                _db.SaveChanges();
                return RedirectToAction("DeletedPackages");
            }
            return View();
        }

        public void Update()
        {
            foreach (Package package in _db.Packages)
            {
                if (package.PublishStartDate > DateTime.Now && package.IsDelete != true)
                {
                    package.IsActive = false;
                    _db.Packages.Update(package);
                }
                if (package.PublishStartDate <= DateTime.Now && package.IsDelete != true)
                {
                    if (package.PublishEndDate <= DateTime.Now && package.IsDelete != true)
                        package.IsActive = false;
                    else
                        package.IsActive = true;
                    _db.Packages.Update(package);
                }
            }
            _db.SaveChanges();
        }


    }
}
