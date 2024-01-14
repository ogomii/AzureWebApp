using Microsoft.AspNetCore.Mvc;

namespace RelativityAzurePojekt.Controllers
{
    public class AppUserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
