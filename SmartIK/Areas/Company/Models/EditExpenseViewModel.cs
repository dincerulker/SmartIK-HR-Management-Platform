using SmartIK.Enums;
using System.ComponentModel.DataAnnotations;

namespace SmartIK.Areas.Company.Models
{
    public class EditExpenseViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Expense Type")]
        [Required]
        public ExpenseTypeEnum ExpenseType { get; set; }
        [Range(1, 100000, ErrorMessage = "Price Range (1$ - 100000$)")]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "Decription filed required")]
        [MaxLength(100, ErrorMessage = "Max 100 Characters")]
        public string Description { get; set; }
        public string FilePath { get; set; }
        public IFormFile File { get; set; }
    }
}
