using System.ComponentModel.DataAnnotations;

namespace SmartIK.Data
{
    public class Package
    {
        public int Id { get; set; }

        [Required, MaxLength(20)]
        public string PackageName { get; set; }

        [Required, MaxLength(100)]
        public string PackageDescription { get; set; }

        [Required, MaxLength(255)]
        public string PicturePath { get; set; }

        public decimal PackagePrice { get; set; }
        public int PersonalCount { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime PublishStartDate { get; set; }
        public DateTime PublishEndDate { get; set; }
        public int? ExpirationDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }

        public List<Corporation> Companies { get; set; }


    }
}
