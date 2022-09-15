using SmartIK.Enums;
using System.ComponentModel.DataAnnotations;

namespace SmartIK.Areas.Company.Models
{
    public class AddCreditCardViewModel
    {
        [Required(ErrorMessage = "Required")]
        public string CardName { get; set; }

        [Required(ErrorMessage = "Required")]
        [CreditCard(ErrorMessage = "Invalid")]
        public string CardNumber { get; set; }
        public BrandEnum? Brand { get; set; }
        public BankEnum? Bank { get; set; }

        [Required(ErrorMessage = "Required")]
        [RegularExpression("^[0-9]{3,4}$", ErrorMessage = "Invalid")]
        public string Cvv { get; set; }

        [Required(ErrorMessage = "Required")]
        [RegularExpression(@"^(0[1-9]|1[0-2])\/[0-9]{2}$", ErrorMessage = "Invalid")]
        public string Expire { get; set; }
    }
}
