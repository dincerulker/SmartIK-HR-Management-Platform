using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using SmartIK.Attributes;
using SmartIK.Enums;
using System.ComponentModel.DataAnnotations;

namespace SmartIK.Areas.Company.Models
{
    public class AddEmployeeViewModel : IdentityUser
    {
        [Display(Name = "First Name")]
        [Required(ErrorMessage = "{0} field is required.")]
        [MaxLength(20, ErrorMessage = "The {0} field cannot exceed {1} characters.")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public string FirstName { get; set; }

        [Display(Name = "Second Name")]
        [MaxLength(20, ErrorMessage = "The {0} field cannot exceed {1} characters.")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public string SecondName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "{0} field is required.")]
        [MaxLength(20, ErrorMessage = "The {0} field cannot exceed {1} characters.")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public string LastName { get; set; }

        [Display(Name = "Turkis Identity Number")]
        [Required(ErrorMessage = "{0} field is required.")]
        [MaxLength(11, ErrorMessage = "The {0} field cannot exceed {1} characters.")]
        [MinLength(11, ErrorMessage = "The {0} field cannot less {1} characters.")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Only numberic character")]
        [CheckTCKN]
        public string TCKN { get; set; }

        [Display(Name = "Phone Number")]
        [Required(ErrorMessage = "{0} field is required.")]
        [MaxLength(11, ErrorMessage = "The {0} field cannot exceed {1} characters.")]
        [MinLength(11, ErrorMessage = "The {0} field cannot less {1} characters.")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Only numberic character")]
        public string Phone { get; set; }

        [Required]
        [FileType("image", 1024, ErrorMessage = "File Format")]
        public IFormFile Image { get; set; }

        [Display(Name = "Picture Path")]
        public string PicturePath { get; set; }

        [Display(Name = "Address")]
        [Required(ErrorMessage = "{0} field is required.")]
        [MaxLength(100, ErrorMessage = "The {0} field cannot exceed {1} characters.")]
        [MinLength(10, ErrorMessage = "The {0} field cannot less {1} characters.")]
        public string Address { get; set; }

        [Required]
        [Range(0,50000)]
        public int? Salary { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Hiring Date")]
        [Required(ErrorMessage = "{0} field is required.")]
        [HiringDate]
        public DateTime? HiringDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Birth Date")]
        [Required(ErrorMessage = "{0} field is required.")]
        [BirthDate]
        public DateTime? BirthDate { get; set; }

        [Required(ErrorMessage = "Gender field is required.")]
        public GenderEnum Gender { get; set; }
        [Display(Name = "Blood Type")]
        [Required(ErrorMessage = "{0} field is required.")]
        public BloodTypeEnum BloodType { get; set; }
        [Required]
        public TitleEnum Title { get; set; }


        public bool? IsActive { get; set; }
        public bool? IsDelete { get; set; }

        [Required]
        [DataType(DataType.EmailAddress, ErrorMessage = "E-mail is not valid")]
        public string Email { get; set; }
        public int TotalEmployee { get; set; }
    }
}
