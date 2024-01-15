using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RelativityAzurePojekt.Models;

namespace RelativityAzurePojekt.Controllers
{
    public class ReviewController : Controller
    {
        private readonly MyDatabaseContext _context;
        public ReviewController(MyDatabaseContext context)
        {
            _context = context;
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
                return NotFound("No reviews found for the given MovieID.");
            }

            return View(reviews);
        }
    }
}
