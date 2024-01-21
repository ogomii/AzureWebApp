using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RelativityAzurePojekt.Models;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

namespace RelativityAzurePojekt.Controllers
{
    public class ReviewController : Controller
    {
        private readonly ILogger<ReviewController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor; 
        private readonly MyDatabaseContext _context;
        public ReviewController(ILogger<ReviewController> logger, MyDatabaseContext context, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        // GET: Review/MovieReviews/5
        public async Task<IActionResult> MovieReviews(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            List<Review> reviews = await _context.Review
            .Where(r => r.MovieID == id)
            .ToListAsync();

            if (reviews == null || !reviews.Any())
            {
                return View("~/Views/Review/NoReviewFound.cshtml");
            }

            return View(reviews);
        }

        [HttpGet]
        public IActionResult AddReview(int? id)
        {
            
            Review review = new Review();
            review.MovieID = (int)id;
            var session = _httpContextAccessor.HttpContext.Session;
            review.AppUserID = (int)session.GetInt32("userID");
            return View(review);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddReview([Bind("ID,MovieID,AppUserID,Stars,Opinion")] Review review)
        {
            review.ID = null;
            if (ModelState.IsValid)
            {
                _context.Add(review);
                await _context.SaveChangesAsync();
            }
            List<Review> reviews = await _context.Review
            .Where(r => r.MovieID == review.MovieID)
            .ToListAsync();

            if (reviews == null || !reviews.Any())
            {
                return View("~/Views/Review/NoReviewFound.cshtml");
            }
            return View("~/Views/Review/MovieReviews.cshtml", reviews);
        }
    }
}
