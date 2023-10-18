using Microsoft.AspNetCore.Mvc;

namespace Clientt.Controllers
{
    public class DepartmentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
