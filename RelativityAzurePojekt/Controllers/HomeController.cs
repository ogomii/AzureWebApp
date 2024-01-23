using Microsoft.AspNetCore.Mvc;
using RelativityAzurePojekt.Models;
using System.Diagnostics;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

namespace RelativityAzurePojekt.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        // GET: Home/Index
        public IActionResult Index()
        {
            return View("Index");
        }

        // GET: Home/Index
        public IActionResult Privacy()
        {
            return View("Privacy");
        }

        // GET: Shared/Error
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}