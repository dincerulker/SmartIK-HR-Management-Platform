using System.ComponentModel.DataAnnotations;

namespace SmartIK.Enums
{
    public enum BloodTypeEnum
    {
        [Display(Name = "A (Rh+)")]
        APositive,
        [Display(Name = "A (Rh-)")]
        ANegative,
        [Display(Name = "B (Rh+)")]
        BPositive,
        [Display(Name = "B (Rh-)")]
        BNegative,
        [Display(Name = "0 (Rh+)")]
        ZeroPositive,
        [Display(Name = "0 (Rh-)")]
        ZeroNegative,
        [Display(Name = "AB (Rh+)")]
        ABPositive,
        [Display(Name = "AB (Rh-)")]
        ABNegative,
    }
}
