using SmartIK.Enums;
using System.ComponentModel.DataAnnotations;

namespace SmartIK.Data
{
    public class AdvancePayment
    {
        public int Id { get; set; }
        public string Description { get; set; }

        [Display(Name = "Advance Amount")]
        public decimal AdvanceAmount { get; set; }

        [Display(Name = "Request Date")]
        public DateTime RequestDate { get; set; }

        [Display(Name = "Reply Date")]
        public DateTime? ReplyDate { get; set; }
        public string Response { get; set; }
        public StatusEnum Status { get; set; }

        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
