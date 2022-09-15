using Microsoft.AspNetCore.Identity;
using SmartIK.Enums;
using System.ComponentModel.DataAnnotations;

namespace SmartIK.Data
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string LastName { get; set; }
        public string TCKN { get; set; }
        public string Phone { get; set; }
        public string PicturePath { get; set; }
        public string Address { get; set; }
        public int? Salary { get; set; }
        public int? MaxPermissionDays { get; set; }
        public int? UsingPermissionDays { get; set; }
        public decimal? AdvancePaymentLimit { get; set; }
        public decimal? AdvanceUsed { get; set; }
        public DateTime? HiringDate { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? BirthDate { get; set; }
        public DateTime? DismissalDate { get; set; }
        public GenderEnum? Gender { get; set; }
        public BloodTypeEnum? BloodType { get; set; }
        public TitleEnum? Title { get; set; }

        public int? CountryId { get; set; }
        public Country Country { get; set; }
        public int? CityId { get; set; }
        public City City { get; set; }

        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }

        public int? CorparationId { get; set; }
        public Corporation Corparation { get; set; }
        public int? OccupationId { get; set; }
        public Occupation Occupation { get; set; }
        public List<AdvancePayment> AdvancePayments { get; set; }
        public List<Permission> Permissions { get; set; }
        public List<Expense> Expenses { get; set; }
    }
}
