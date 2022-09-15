using Microsoft.AspNetCore.Identity;
using System.Net;

namespace SmartIK.Data
{
    public static class ApplicationDbContextSeed
    {
        public static void Seed(ApplicationDbContext db)
        {
            if (!db.Countries.Any() || !db.Cities.Any())
            {
                var country1 = new Country() { CountryName = "Turkey" };
                var country2 = new Country() { CountryName = "USA" };

                db.Countries.AddRange(country1, country2);
                db.SaveChanges();

                var city1 = new City() { CityName = "Istanbul", CountryId = 1 };
                var city2 = new City() { CityName = "Ankara", CountryId = 1 };
                var city3 = new City() { CityName = "Izmir", CountryId = 1 };
                var city4 = new City() { CityName = "New York City", CountryId = 2 };
                var city5 = new City() { CityName = "Los Angeles", CountryId = 2 };
                var city6 = new City() { CityName = "Texas", CountryId = 2 };

                db.Cities.AddRange(city1, city2, city3, city4, city5, city6);
                db.SaveChanges();
            }

            if (db.Packages.Any() || db.Corporations.Any() || db.Occupations.Any()) return;

            Package package1 = new Package()
            {
                PackageName = "Start",
                PackageDescription = "Trial Version",
                PicturePath = "entry.jpg",
                PackagePrice = 0m,
                PersonalCount = 0,
                CreatedDate = DateTime.Now,
                PublishEndDate = DateTime.Now,
                PublishStartDate = DateTime.Now
            };

            var package2 = new Package()
            {
                PackageName = "Performance",
                PackageDescription = "Emplpoyes performance",
                PicturePath = "performance.jpg",
                PackagePrice = 100.49m,
                PersonalCount = 10,
                CreatedDate = DateTime.Now,
                PublishEndDate = DateTime.Now,
                PublishStartDate = DateTime.Now
            };

            var package3 = new Package()
            {
                PackageName = "Shift",
                PackageDescription = "Employes shift",
                PicturePath = "shift.jpg",
                PackagePrice = 12m,
                PersonalCount = 20,
                CreatedDate = DateTime.Now,
                PublishEndDate = DateTime.Now,
                PublishStartDate = DateTime.Now
            };

            db.Packages.AddRange(package1, package2, package3);
            db.SaveChanges();

            Corporation company1 = new Corporation()
            {
                CompanyName = "BilgeAdam Akademi",
                PhoneNumber = "4443330",
                LogoUri = "https://akademi.bilgeadam.com/wp-content/uploads/2021/01/akademilogo-yatay.png",
                CountryId = 1,
                CityId = 1,
                MailAddress = "info@bilgeadam.com",
                Websites = "https://akademi.bilgeadam.com/",
                Address = "Reşitpaşa Mah. Katar Cad. Teknokent ARI 3 No: 4 B3 Sarıyer / İstanbul",
                TaxNumber = "9874563210",
                MersisNumber = "1122334455667788",
                PackageId = 1,
                CompanyType = Areas.Admin.Enums.CompanyTypeEnum.Holding
            };

            Corporation company2 = new Corporation()
            {
                CompanyName = "MUFUDI Software",
                PhoneNumber = "05401234567",
                LogoUri = "https://akademi.bilgeadam.com/wp-content/uploads/2021/01/akademilogo-yatay.png",
                CountryId = 2,
                CityId = 2,
                MailAddress = "info@bilgeadam.com",
                Websites = "https://smartik.azurewebsites.net",
                Address = "Cyperpunk Ankara",
                TaxNumber = "9874563210",
                MersisNumber = "1122334455667788",
                PackageId = 2,
                CompanyType = Areas.Admin.Enums.CompanyTypeEnum.Holding
            };

            db.Corporations.AddRange(company1, company2);
            db.SaveChanges();

            var oc1 = new Occupation() { OccupationName = "Junior Frontend Developer" };
            var oc2 = new Occupation() { OccupationName = "Junior Backend Developer" };
            var oc3 = new Occupation() { OccupationName = "Senior Frontend Developer" };
            var oc4 = new Occupation() { OccupationName = "Senior Backend Developer" };
            var oc5 = new Occupation() { OccupationName = "Junior Fullstack Developer" };
            var oc6 = new Occupation() { OccupationName = "Senior Fullstack Developer" };
            var oc7 = new Occupation() { OccupationName = "Human Resources" };
            var oc8 = new Occupation() { OccupationName = "Security" };
            var oc9 = new Occupation() { OccupationName = "Biomedical Engineer" };
            var oc10 = new Occupation() { OccupationName = "Winding Technician" };
            var oc11 = new Occupation() { OccupationName = "Regional director" };
            var oc12 = new Occupation() { OccupationName = "Call Center Officer" };
            var oc13 = new Occupation() { OccupationName = "Mobile Developer" };
            var oc14 = new Occupation() { OccupationName = "Electric and Electronic Engineer" };
            var oc15 = new Occupation() { OccupationName = "Mechatronics Engineer" };
            var oc16 = new Occupation() { OccupationName = "Physics Engineer" };
            var oc17 = new Occupation() { OccupationName = "Finance Expert" };
            var oc18 = new Occupation() { OccupationName = "UI/UX Developer" };
            var oc19 = new Occupation() { OccupationName = "Graphics Developer" };
            var oc20 = new Occupation() { OccupationName = "Software Architect" };

            db.Occupations.AddRange(oc1, oc2, oc3, oc4, oc5, oc6, oc7, oc8, oc9, oc10, oc11, oc12, oc13, oc14, oc15, oc16, oc17, oc18, oc19, oc20);
            db.SaveChanges();


        }

        public static async Task SeedUserAsync(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            var myilmaz = new ApplicationUser()
            {
                Email = "mustafa.yilmaz@smartik.com",
                UserName = "mustafa.yilmaz@smartik.com",
                EmailConfirmed = true,
                PicturePath = "mustafa_yilmaz.jpg",
                CountryId = 1,
                CityId = 3
            };
            var mciftci = new ApplicationUser()
            {
                Email = "mustafa.ciftci@smartik.com",
                UserName = "mustafa.ciftci@smartik.com",
                EmailConfirmed = true,
                PicturePath = "mustafa_ciftci.jpg",
                CountryId = 1,
                CityId = 3
            };
            var dulker = new ApplicationUser()
            {
                Email = "dincer.ulker@smartik.com",
                UserName = "dincer.ulker@smartik.com",
                EmailConfirmed = true,
                PicturePath = "dincer_ulker.jpg",
                CountryId = 1,
                CityId = 3
            };
            var ferciyas = new ApplicationUser()
            {
                Email = "furkan.erciyas@smartik.com",
                UserName = "furkan.erciyas@smartik.com",
                EmailConfirmed = true,
                PicturePath = "furkan_erciyas.jpg",
                CountryId = 1,
                CityId = 3
            };
            await roleManager.CreateAsync(new IdentityRole("Admin"));
            await roleManager.CreateAsync(new IdentityRole("Company Manager"));
            await roleManager.CreateAsync(new IdentityRole("Employee"));
            await userManager.CreateAsync(myilmaz, "Smartik.06");
            await userManager.AddToRoleAsync(myilmaz, "Admin");
            await userManager.CreateAsync(mciftci, "Smartik.06");
            await userManager.AddToRoleAsync(mciftci, "Admin");
            await userManager.CreateAsync(ferciyas, "Smartik.06");
            await userManager.AddToRoleAsync(ferciyas, "Admin");
            await userManager.CreateAsync(dulker, "Smartik.06");
            await userManager.AddToRoleAsync(dulker, "Admin");
        }
    }
}