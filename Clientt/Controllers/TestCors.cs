using Microsoft.AspNetCore.Mvc;

namespace Clientt.Controllers
{
    public class TestCors : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
