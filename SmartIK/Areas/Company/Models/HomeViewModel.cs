using SmartIK.Data;

namespace SmartIK.Areas.Company.Models
{
    public class HomeViewModel
    {
        public Corporation Corporation { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public decimal UsedAdvance { get; set; }
        public int RemaningPermission { get; set; }
        public bool HavePackage { get; set; }
        public int PackageId { get; set; }
        public List<Package> Packages { get; set; }

        public Wallet Wallet { get; set; }
    }
}
