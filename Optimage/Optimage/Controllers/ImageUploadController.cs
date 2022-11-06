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
        // Setting a variable for control the content root folder of solution
        private readonly IWebHostEnvironment Environment;
        public ImageUploadController(IWebHostEnvironment _environment)
        {
            Environment = _environment;
        }

        /* --- Startup action --- */
        /**
            Contains the procedures need to be activated when the project is launched:
            - Auto clearing image directory from files and folders
            - View result is a list of uploaded files (running the project, by default, the result is empty)
        **/
        public IActionResult ImageUpload()
        {
            DirectoryInfo folder = new DirectoryInfo(Path.Combine(this.Environment.ContentRootPath, "Images"));
            foreach (FileInfo file in folder.EnumerateFiles())
            {
                if (file.Exists)
                {
                    file.Delete();
                }
            }
            foreach (DirectoryInfo directory in folder.EnumerateDirectories())
            {
                if (directory.Exists)
                {
                    directory.Delete();
                }
            }
            var items = GetFiles();
            return View(items);
        }

 
        /* --- Uploading action --- */
        /**
            Saves image upload directory. If missing, create a folder.
            With using file stream creates file in choosen directory.
            Uploading process it's just copying form file in folder.
            A message of uploading result is displayed below the form and list of files becomes updated.
        **/
        [HttpPost]
        public IActionResult ImageUpload(IFormFile uploadedImage)
        {
            string appPath = Path.Combine(this.Environment.ContentRootPath, "Images");
            if (!Directory.Exists(appPath))
            {
                Directory.CreateDirectory(appPath);
            }
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

        /* --- List of files --- */
        /** 
            Summary function which looking for files in choosen directory and return the string List. 
            Used to display file names on the page. 
        **/
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

        /* --- Download files --- */
        /** 
            Result is generated for every uploaded file as an action link.
            Return action for file download.
        **/
        public FileResult Download(string ImageName)
        {
            var FileVirtualPath = Path.Combine(this.Environment.ContentRootPath, "Images") + "\\" + ImageName;
            byte[] fileBytes = System.IO.File.ReadAllBytes(FileVirtualPath);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, Path.GetFileName(FileVirtualPath));
        }
    }
}
