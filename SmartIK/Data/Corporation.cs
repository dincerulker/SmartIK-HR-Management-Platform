using Microsoft.AspNetCore.Http;
using SmartIK.Areas.Admin.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartIK.Data
{
    public class Corporation
    {
        public int Id { get; set; }
        [Required, MaxLength(40)]
        public string CompanyName { get; set; }
        [Required, MaxLength(20)]
        public string PhoneNumber { get; set; }
        [Required, MaxLength(255)]
        public string LogoUri { get; set; }
        [Required, MaxLength(50)]
        public string MailAddress { get; set; }
        // [Required, MaxLength(40)]
        public string Websites { get; set; }
        [MaxLength(200)]
        public string Address { get; set; }
        public string TaxNumber { get; set; }
        // [Required, MaxLength(40)]
        public string MersisNumber { get; set; }
        [Required]
        public int NumberOfEmployees { get; set; }
        public List<ApplicationUser> CorporationUsers { get; set; }
        public List<CreditCard> CreditCards { get; set; }
        [ForeignKey("Wallet")]
        public int? WalletId { get; set; }
        public Wallet Wallet { get; set; }
        public int? PackageId { get; set; }
        public Package Package { get; set; }
        public CompanyTypeEnum CompanyType { get; set; }
        public int? CountryId { get; set; }
        public Country Country { get; set; }
        public int? CityId { get; set; }
        public City City { get; set; }

    }
}
