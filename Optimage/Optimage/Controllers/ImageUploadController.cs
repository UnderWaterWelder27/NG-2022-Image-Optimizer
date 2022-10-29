using System.IO;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;


namespace Optimage.Controllers
{
    public class ImageUploadController : Controller
    {
        private readonly IWebHostEnvironment Environment;

        public ImageUploadController(IWebHostEnvironment _environment)
        {
            Environment = _environment;
        }
        public IActionResult Index()
        {
            DirectoryInfo folder = new DirectoryInfo(Path.Combine(this.Environment.ContentRootPath, "Images"));
            foreach (FileInfo file in folder.EnumerateFiles())
            {
                file.Delete();
            }
            return View("ImageUploadView");
        }

        [HttpPost]
        public IActionResult Index(IFormFile uploadedImage)
        {
            /// Save image upload directory. If missing, create a folder
            string appPath = Path.Combine(this.Environment.ContentRootPath, "Images");
            if (!Directory.Exists(appPath))
            {
                Directory.CreateDirectory(appPath);
            }

            /// Creates a file object and saves it to the FileStream. Then view output uploaded file
            try
            {
                string fileName = Path.GetFileName(uploadedImage.FileName);
                FileStream stream = new FileStream(Path.Combine(appPath, fileName), FileMode.Create);
                uploadedImage.CopyTo(stream);
                ViewBag.Message = string.Format($"<b>{fileName}</b> uploaded.<br />");
            }
            catch (System.NullReferenceException ex)
            {
                ViewBag.Message = "ERROR: " + ex.Message.ToString();
            }

            return View("ImageUploadView");
        }
    }
}
