using SmartIK.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartIK.Areas.Company.Models
{
    public class WalletCreateViewModel
    {
        public Corporation Corporation { get; set; }

        [ForeignKey("Corporation")]
        public int? CorporationId { get; set; }
        public int? Balance { get; set; }
    }
}
