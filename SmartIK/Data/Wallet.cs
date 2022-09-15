using System.ComponentModel.DataAnnotations.Schema;

namespace SmartIK.Data
{
    public class Wallet
    {
        public int Id { get; set; }
        public Corporation Corporation { get; set; }
        [ForeignKey("Corporation")]
        public int? CorporationId { get; set; }
        public int? Balance { get; set; } = 0;
    }
}
