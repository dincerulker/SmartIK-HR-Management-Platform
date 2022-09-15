using SmartIK.Data;

namespace SmartIK.Areas.Company.Models
{
    public class BuyPackageViewModel
    {
        public int PackageId { get; set; }
        public int CorporationId { get; set; }
        public Package Package { get; set; }

    }
}
