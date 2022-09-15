using System.ComponentModel.DataAnnotations;

namespace SmartIK.Enums
{
    public enum BrandEnum
    {
        Visa,
        Master,
        [Display(Name = "American Express")]
        AmericanExpress,
        Sodexo,
        Troy
    }
}
