using System.IO;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting.Server;


namespace Optimage.Controllers
{
    public class ImageUploadController : Controller
    {
        private readonly IWebHostEnvironment Environment;
        
        public ImageUploadController(IWebHostEnvironment _environment)
        {
            Environment = _environment;
        }
        public IActionResult ImageUpload()
        {
            DirectoryInfo folder = new DirectoryInfo(Path.Combine(this.Environment.ContentRootPath, "Images"));
            foreach (FileInfo file in folder.EnumerateFiles())
            {
                // exist !
                file.Delete();
            }

            var items = GetFiles();
            return View(items);
        }

        [HttpPost]
        public IActionResult ImageUpload(IFormFile uploadedImage)
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
                using (FileStream stream = new FileStream(Path.Combine(appPath, fileName), FileMode.Create))
                {
                    uploadedImage.CopyTo(stream);
                    ViewBag.Message = string.Format($"<b>{fileName}</b> uploaded.<br />");
                }
            }
            catch (System.NullReferenceException ex)
            {
                ViewBag.Message = "ERROR: " + ex.Message.ToString();
            }

            var items = GetFiles();
            return View(items);
        }

        private List<string> GetFiles()
        {
            DirectoryInfo folder = new DirectoryInfo(Path.Combine(this.Environment.ContentRootPath, "Images"));
            FileInfo[] fileNames = folder.GetFiles("*.*");
            List<string> images = new List<string>();

            foreach (var file in fileNames)
            {
                images.Add(file.Name);
            }

            return images;
        }

        public FileResult Download(string ImageName)
        {
            var FileVirtualPath = Path.Combine(this.Environment.ContentRootPath, "Images") + "\\" + ImageName;
            byte[] fileBytes = System.IO.File.ReadAllBytes(FileVirtualPath);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, Path.GetFileName(FileVirtualPath));
        }
    }
}
