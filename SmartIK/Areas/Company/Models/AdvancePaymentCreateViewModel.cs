using SmartIK.Data;

namespace SmartIK.Areas.Company.Models
{
    public class AdvancePaymentCreateViewModel
    {
        public string Description { get; set; }
        public decimal AdvanceAmount { get; set; }
        public ApplicationUser applicationUser { get; set; }
        
    }
}
