using SmartIK.Data;

namespace SmartIK.Areas.Admin.Models
{
    public class AdminIndexViewModel
    {
        public List<Package> Packages { get; set; }
        public int ActivePackageCount { get; set; }
        public int PassivePackageCount { get; set; }
        public int DeletedPackageCount { get; set; }
        public List<ApplicationUser> Users { get; set; }
    }
}
