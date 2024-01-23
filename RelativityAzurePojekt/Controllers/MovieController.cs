using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RelativityAzurePojekt.Models;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

namespace RelativityAzurePojekt.Controllers
{
    public class MovieController : Controller
    {
        private readonly ILogger<MovieController> _logger;
        private readonly MyDatabaseContext _context;

        public MovieController(ILogger<MovieController> logger, MyDatabaseContext context)
        {
            _logger = logger;
            _context = context;
        }

        // GET: Movie
        public async Task<IActionResult> Index()
        {
            List<Movie> movies = await _context.Movie.ToListAsync();
            List<RatedMovie> ratedMovies = new List<RatedMovie>();
            foreach(var movie in movies)
            {
                List<Review> reviewList = await _context.Review
                .Where(r => r.MovieID == movie.ID).ToListAsync();
                double averageStars = 0;
                if (reviewList.Count > 0)
                {
                    averageStars = reviewList.Average(item => item.Stars);
                    averageStars = Math.Round(averageStars, 2);
                }
                ratedMovies.Add(new RatedMovie(movie, averageStars));
            }

            return View(ratedMovies);
        }

        // GET: Movie/Description/5
        public async Task<IActionResult> Description(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie
                .FirstOrDefaultAsync(m => m.ID == id);
            if (movie == null)
            {
                return NotFound();
            }

            List<Review> reviewList = await _context.Review
                .Where(r => r.MovieID == movie.ID).ToListAsync();
            double averageStars = 0;
            if (reviewList.Count > 0)
            {
                averageStars = reviewList.Average(item => item.Stars);
            }
            RatedMovie ratedMovie = new RatedMovie(movie, averageStars);

            return View(ratedMovie);
        }

        // GET: Movie/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Movie/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Title,Description,ReleaseDate")] Movie movie)
        {
            if (ModelState.IsValid)
            {
                _context.Add(movie);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Added movie {title}", movie.Title);
                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }

        // GET: Movie/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            return View(movie);
        }

        // POST: Movie/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Title,Description,ReleaseDate")] Movie movie)
        {
            if (id != movie.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(movie);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Edited movie {title}", movie.Title);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieExists(movie.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }

        // GET: Movie/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie
                .FirstOrDefaultAsync(m => m.ID == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // POST: Movie/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movie = await _context.Movie.FindAsync(id);
            _context.Movie.Remove(movie);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Deleted movie {title}", movie.Title);
            return RedirectToAction(nameof(Index));
        }

        private bool MovieExists(int id)
        {
            return _context.Movie.Any(e => e.ID == id);
        }
    }
}