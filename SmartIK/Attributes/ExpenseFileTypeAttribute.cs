using System.ComponentModel.DataAnnotations;

namespace SmartIK.Attributes
{
    public class ExpenseFileTypeAttribute : ValidationAttribute
    {
        private readonly int _size = 1024;

        public override bool IsValid(object value)
        {
            IFormFile formFile = value as IFormFile;
            if (formFile == null) return true;

            if (!formFile.ContentType.StartsWith("image" + "/") && !formFile.ContentType.StartsWith("application/pdf"))
            {
                ErrorMessage = $"File type format must be img/pdf";
                return false;
            }
            else if (formFile.Length > _size * 1024)
            {
                ErrorMessage = $"Maximum file size must be {_size * 1024}.";
                return false;
            }

            return true;
        }
    }
}
