using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RelativityAzurePojekt.Models;

namespace RelativityAzurePojekt.Controllers
{
    public class AppUserController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly MyDatabaseContext _context;

        public AppUserController(MyDatabaseContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

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
                    return View("~/Views/Home/Index.cshtml");
                }
                else
                {
                    return View();
                }
            }
            return View();
        }

        public IActionResult Logout()
        {
            var session = _httpContextAccessor.HttpContext.Session;
            session.SetInt32("userAuthenticated", 0);
            return View("~/Views/Home/Index.cshtml");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("ID,FirstName,LastName,Passwd")] AppUser user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return View("~/Views/AppUser/Login.cshtml");
            }
            return View("~/Views/Home/Index.cshtml");
        }

    }
}
