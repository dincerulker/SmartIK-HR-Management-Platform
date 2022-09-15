using Microsoft.AspNetCore.Mvc.Rendering;

namespace SmartIK.Areas.Company.Models
{
    public class AddBalanceViewModel
    {
        public int Balance { get; set; }
        public List<SelectListItem> CreditCarts { get; set; }
    }
}
