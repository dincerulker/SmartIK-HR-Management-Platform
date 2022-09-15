using System.ComponentModel.DataAnnotations;

namespace SmartIK.Enums
{
    public enum ExpenseTypeEnum
    {
        [Display(Name = "Education Costs")]
        LocationCosts,
        Utilities,
        [Display(Name = "Telephone And Internet")]
        TelephoneAndInternet,
        [Display(Name = "Business Insurance")]
        BusinessInsurance,
        [Display(Name = "Office Equipment")]
        OfficeEquipment,
        [Display(Name = "Business Meals")]
        BusinessMeals,
        [Display(Name = "Employee Gifts")]
        EmployeeGifts,
        [Display(Name = "Bussines Travel")]
        BusinessTravel,
        Education,
        [Display(Name = "Home Office")]
        HomeOffice,
        Events,
        Others
    }
}
