using SmartIK.Areas.Admin.Enums;
using SmartIK.Data;

namespace SmartIK.Areas.Admin.Models
{
    public class DetailCompanyViewModel
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string PhoneNumber { get; set; }
        public string LogoUri { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string MailAddress { get; set; }
        public string WebSites { get; set; }
        public string Address { get; set; }
        public string TaxNumber { get; set; }
        public string MersisNumber { get; set; }
        public CompanyTypeEnum CompanyType { get; set; }

        public int NumberOfEmployees { get; set; }
        public List<ApplicationUser> CorporationUsers { get; set; }


        public int? PackageId { get; set; }
        public Package Package { get; set; }
    }
}
