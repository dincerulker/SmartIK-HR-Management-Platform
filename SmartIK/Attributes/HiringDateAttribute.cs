using System.ComponentModel.DataAnnotations;

namespace SmartIK.Attributes
{
    public class HiringDateAttribute : ValidationAttribute
    {
        public override bool IsValid(object value )
        {
            var hiringDate = Convert.ToDateTime(value);

            if (hiringDate >= DateTime.Now)
            {
                ErrorMessage = $"Date can not be in the future";
                return false;
            }
            else if (hiringDate <= DateTime.Now.AddYears(-100))
            {
                ErrorMessage = $"Year bigger than {DateTime.Now.AddYears(-99)}";
                return false;
            }
            
            return true;
        }
    }
}
