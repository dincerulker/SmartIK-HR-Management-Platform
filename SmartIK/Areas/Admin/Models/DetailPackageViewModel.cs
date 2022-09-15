using SmartIK.Data;
using System.ComponentModel.DataAnnotations;

namespace SmartIK.Areas.Admin.Models
{
    public class DetailPackageViewModel
    {
        public int Id { get; set; }
        public string PackageName { get; set; }
        public string PackageDescription { get; set; }
        public string PicturePath { get; set; }
        public decimal PackagePrice { get; set; }
        public DateTime CreatedDate { get; set; }

        public DateTime PublishStartDate { get; set; } 
        public DateTime PublishEndDate { get; set; }
        public List<Corporation> Companies { get; set; }
    }
}
