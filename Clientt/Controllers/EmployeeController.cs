using Microsoft.AspNetCore.Mvc;

namespace Clientt.Controllers
{
    public class EmployeeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
