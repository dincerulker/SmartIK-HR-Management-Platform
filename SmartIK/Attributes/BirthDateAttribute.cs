using System.ComponentModel.DataAnnotations;

namespace SmartIK.Attributes
{
    public class BirthDateAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var birthDate = Convert.ToDateTime(value);

            if (birthDate >= DateTime.Now)
            {
                ErrorMessage = $"Date can not be in the future amd now";
                return false;
            }
            else if(birthDate >= DateTime.Now.AddYears(-18))
            {
                ErrorMessage = $"Date bigger than {DateTime.Now.AddYears(-18)}";
                return false;
            }
            else if (birthDate <= DateTime.Now.AddYears(-100))
            {
                ErrorMessage = $"Year bigger than {DateTime.Now.AddYears(-99)}";
                return false;
            }
            return true;
        }
    }
}
