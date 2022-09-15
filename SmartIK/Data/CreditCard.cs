using SmartIK.Enums;

namespace SmartIK.Data
{
    public class CreditCard
    {
        public int Id { get; set; }
        public string CardName { get; set; }
        public string CardNumber { get; set; }
        public BrandEnum? Brand { get; set; }
        public BankEnum? Bank { get; set; }
        public string Cvv { get; set; }
        public string CardExpire { get; set; }
        public Corporation Corporation { get; set; }
        public int? CorporationId { get; set; }
    }
}
