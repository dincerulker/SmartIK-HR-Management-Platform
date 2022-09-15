using SmartIK.Data;

namespace SmartIK.Areas.Company.Models
{
    public class AdvancePaymentUserViewModel
    {
        public  List<AdvancePayment> AdvancePayments { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

    }
}
