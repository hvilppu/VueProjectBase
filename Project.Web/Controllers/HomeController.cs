using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Project.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Error()
        {
            ViewData["RequestId"] = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}