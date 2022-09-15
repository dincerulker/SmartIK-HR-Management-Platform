using SmartIK.Areas.Admin.Enums;
using SmartIK.Attributes;
using System.ComponentModel.DataAnnotations;

namespace SmartIK.Areas.Admin.Models
{
    public class EditPackageViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Package Name")]
        [Required(ErrorMessage = "{0} field is required.")]
        [MaxLength(20, ErrorMessage = "The {0} field cannot exceed {1} characters.")]
        public string PackageName { get; set; }

        [Display(Name = "Package Description")]
        [Required(ErrorMessage = "{0} field is required.")]
        [MaxLength(100, ErrorMessage = "The {0} field cannot exceed {1} characters.")]
        public string PackageDescription { get; set; }


        [FileType("image", 1024, ErrorMessage = "File Format")]
        public IFormFile Image { get; set; }


        [Display(Name = "Package Price")]
        [Required(ErrorMessage = "{0} field is required.")]
        [Range(0, 100000, ErrorMessage = "{0} must be at least {1} and at most {2}.")]
        public decimal PackagePrice { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = false)]
        [Display(Name = "Publish Start Date")]
        [Required(ErrorMessage = "{0} field is required.")]   
        public DateTime? PublishStartDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = false)]
        [Display(Name = "Publish End Date")]
        [Required(ErrorMessage = "{0} field is required.")]
        [CheckDate]
        public DateTime? PublishEndDate { get; set; }

        public string PicturePath { get; set; }

    }
}
