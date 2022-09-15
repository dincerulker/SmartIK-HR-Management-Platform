using Microsoft.AspNetCore.Http;
using SmartIK.Attributes;
using SmartIK.Enums;
using System.ComponentModel.DataAnnotations;

namespace SmartIK.Areas.Company.Models
{
    public class AddExpenseViewModel
    {
        [Display(Name = "Expense Type")]
        [Required]
        public ExpenseTypeEnum ExpenseType { get; set; }

        [Required]
        [Range(1,100000, ErrorMessage ="Price Range (1$ - 100000$)")]
        public decimal Price { get; set; }
        
        [Required(ErrorMessage = "Decription filed required")]
        [MaxLength(100,ErrorMessage = "Max 100 Characters")]
        public string Description { get; set; }

        [ExpenseFileTypeAttribute]
        [Required(ErrorMessage ="This field is required")]
        public IFormFile File { get; set; }
    }
}
