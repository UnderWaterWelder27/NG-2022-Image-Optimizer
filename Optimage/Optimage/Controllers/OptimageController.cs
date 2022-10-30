using Microsoft.AspNetCore.Mvc;

namespace Optimage.Controllers
{
    public class OptimageController : Controller
    {
        public IActionResult Index()
        {
            return View("Optimage");
        }
    }
}
