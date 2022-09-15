using System.ComponentModel.DataAnnotations;

namespace SmartIK.Attributes
{
    public class CheckDateAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var inputDate = Convert.ToDateTime(value);
            if (inputDate >= DateTime.Now.AddDays(-1) && inputDate < DateTime.Now.AddYears(3)) return true;
            ErrorMessage = $"The input date must be today or a future date (Max 3 Years)";
            return false;
        }
    }
}
