using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RelativityAzurePojekt.Models;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

namespace RelativityAzurePojekt.Controllers
{
    public class AppUserController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly MyDatabaseContext _context;
        private readonly ILogger<AppUserController> _logger;

        public AppUserController(ILogger<AppUserController> logger, MyDatabaseContext context, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        // GET: AppUser/Index
        public IActionResult Index()
        {
            return View();
        }

        // GET: AppUser/Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: AppUser/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([Bind("ID,FirstName,LastName,Passwd")] AppUser user)
        {
            if (ModelState.IsValid)
            {
                var dbUser = await _context.AppUser
                    .FirstOrDefaultAsync(u => u.FirstName == user.FirstName && u.LastName == user.LastName);
                if(dbUser == null)
                {
                    return View("~/Views/Home/Index.cshtml");
                }
                if(dbUser.Passwd == user.Passwd)
                {
                    var session = _httpContextAccessor.HttpContext.Session;
                    session.SetInt32("userAuthenticated", 1);
                    session.SetInt32("userID", dbUser.ID);
                    _logger.LogInformation("Logged in as {FirstName} {LastName}", user.FirstName, user.LastName);
                    return View("~/Views/Home/Index.cshtml");
                }
                else
                {
                    _logger.LogInformation("Password didn't match, not logged in");
                    return View();
                }
            }
            _logger.LogInformation("Invalid ModelState");
            return View();
        }

        // GET: Home/Index
        public IActionResult Logout()
        {
            var session = _httpContextAccessor.HttpContext.Session;
            session.SetInt32("userAuthenticated", 0);
            _logger.LogInformation("Logged out user");
            return View("~/Views/Home/Index.cshtml");
        }

        // GET: AppUser/Index
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // POST: Home/Index
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("ID,FirstName,LastName,Passwd")] AppUser user)
        {
            if (user != null && 
                user.LastName.IsNullOrEmpty() == false &&
                user.FirstName.IsNullOrEmpty() == false &&
                user.Passwd.IsNullOrEmpty() == false)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Registered as {FirstName} {LastName}", user.FirstName, user.LastName);
                return View("~/Views/AppUser/Login.cshtml");
            }
            return View("~/Views/Home/Index.cshtml");
        }

    }
}
