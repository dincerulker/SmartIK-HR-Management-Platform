using System.ComponentModel.DataAnnotations;

namespace SmartIK.Attributes
{
    public class CheckTCKNAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var tckn = value as string;
            if (tckn[0] == 0)
            {
                ErrorMessage = "TCKN can not start with 0";
                return false;
            }
            int temp = 0, sum = 0;
            for (int i = 1; i < 8; i += 2)
                temp = temp + Convert.ToInt32(tckn[i - 1].ToString()) * 7 - Convert.ToInt32(tckn[i].ToString());
            temp += Convert.ToInt32(tckn[8].ToString()) * 7;
            int ninth = temp % 10;
            for (int i = 0; i < 9; i++)
                sum += Convert.ToInt32(tckn[i].ToString());
            int eleventh = (sum + ninth) % 10;
            if (ninth == Convert.ToInt32(tckn[9].ToString()) && eleventh == Convert.ToInt32(tckn[10].ToString()))
                return true;
            else
            {
                ErrorMessage = "Invalid TCKN";
                return false;
            }
        }
    }
}
