using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SmartIK.Areas.Company.Controllers
{
    
    public class DisplayController : Controller
    {
        private readonly IWebHostEnvironment _environment;

        public DisplayController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        [HttpPost]
        public string Index(string fileName)
        {
            string filePath = Path.Combine(_environment.WebRootPath, "expense-files", fileName);
            return System.IO.File.Exists(filePath) ? filePath : "File not found!";
        }
        public FileResult Show(string path)
        {
            var type = path.Split('.')[1];
            var contentType = "";
            var fileStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Read);

            if (type == "pdf") contentType = "application/pdf";
            else if (type == "doc") contentType = "application/msword";
            else if (type == "docx") contentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
            else if (type == "docx") contentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
            else if (type == "jpeg" || type == "jpg") contentType = "image/jpeg";
            else if (type == "png") contentType = "image/png";
            else if (type == "webp") contentType = "image/webp";

            return File(fileStream, contentType);
        }
    }
}

