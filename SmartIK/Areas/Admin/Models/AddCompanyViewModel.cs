using Microsoft.AspNetCore.Mvc.Rendering;
using SmartIK.Areas.Admin.Enums;
using SmartIK.Attributes;
using SmartIK.Data;
using System.ComponentModel.DataAnnotations;

namespace SmartIK.Areas.Admin.Models
{
    public class AddCompanyViewModel
    {
        [Display(Name = "Company Name")]
        [Required(ErrorMessage = "{0} field is required.")]
        [MaxLength(40, ErrorMessage = "The {0} field cannot exceed {1} characters.")]
        public string CompanyName { get; set; }

        [Display(Name = "Phone Number")]
        [Required(ErrorMessage = "{0} field is required.")]
        [MaxLength(20, ErrorMessage = "The {0} field cannot exceed {1} characters.")]
        [RegularExpression("(05([0-9]{9}))", ErrorMessage = "Phone Number is Invalid. Format: 05XX XXX XXXX")]
        public string PhoneNumber { get; set; }

        public string LogoUri { get; set; }

        [FileType("image", 1024, ErrorMessage = "File Format")]
        public IFormFile CompanyLogo { get; set; }


        [Display(Name = "E-Mail Address")]
        [Required(ErrorMessage = "{0} field is required.")]
        [MaxLength(50, ErrorMessage = "The {0} field cannot exceed {1} characters.")]
        [EmailAddress(ErrorMessage = "E-mail is invalid.")]
        public string MailAddress { get; set; }

        // [Required, MaxLength(40)]
        [Display(Name = "Website")]
        public string Websites { get; set; }

        [Display(Name = "Address")]
        [Required(ErrorMessage = "{0} field is required.")]
        [MaxLength(200, ErrorMessage = "The {0} field cannot exceed {1} characters.")]
        public string Address { get; set; }

        [Display(Name = "Tax Number")]
        [Required(ErrorMessage = "{0} field is required.")]
        [MaxLength(50, ErrorMessage = "The {0} field cannot exceed {1} characters.")]
        [RegularExpression("([0-9]{10})", ErrorMessage = "Tax number is invalid.")]
        public string TaxNumber { get; set; }

        // [Required, MaxLength(40)]
        [Display(Name = "Mersis Number")]
        [RegularExpression("([0-9]{16})", ErrorMessage = "Mersis number is invalid.")]
        public string MersisNumber { get; set; }
        [Display(Name = "Employee Number")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Employee Number must be numeric")]
        [Range(1, 1000,ErrorMessage = "Employee Number for {0} must be between {1} and {2}.")]
        public int NumberOfEmployees { get; set; }

      


        [Display(Name = "Company Type")]
        public CompanyTypeEnum CompanyType { get; set; }
    }
}
