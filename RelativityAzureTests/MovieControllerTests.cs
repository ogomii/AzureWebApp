using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using RelativityAzurePojekt.Controllers;
using RelativityAzurePojekt.Models;
using System.Linq.Expressions;

namespace RelativityAzureTests
{
    [TestClass]
    public class MovieControllerTests
    {
        private DbContextOptions<MyDatabaseContext> _options;
        private MyDatabaseContext _context;
        private MovieController _controller;

        [TestInitialize]
        public void TestInitialize()
        {
            _options = new DbContextOptionsBuilder<MyDatabaseContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryMovieDatabase")
                .Options;
            _context = new MyDatabaseContext(_options);
            var mockLogger = new Mock<ILogger<MovieController>>();
            _controller = new MovieController(mockLogger.Object, _context);
        }

        [TestMethod]
        public async Task Index_ReturnsViewWithRatedMovies()
        {

            var movie = new Movie { ID = 1, Title = "Movie", Description = "Description", ReleaseDate = DateTime.Now };
            _context.Movie.Add(movie);

            var reviews = new List<Review>
            {
                new Review { MovieID = 1, Stars = 4 },
                new Review { MovieID = 1, Stars = 5 }
            };

            _context.Review.AddRange(reviews);
            _context.SaveChanges();

            var result = await _controller.Index() as ViewResult;

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.Model, typeof(List<RatedMovie>));
            var ratedMovies = result.Model as List<RatedMovie>;
            Assert.AreEqual(1, ratedMovies.Count);
        }

        [TestMethod]
        public async Task Description_ValidId_ReturnsViewWithRatedMovie()
        {

            var movie = new Movie { ID = 2, Title = "Movie", Description = "Description", ReleaseDate = DateTime.Now };
            _context.Movie.Add(movie);

            var reviews = new List<Review>
            {
            new Review { MovieID = 2, Stars = 4 },
            new Review { MovieID = 2, Stars = 5 }
            };

            _context.Review.AddRange(reviews);
            _context.SaveChanges();

            var result = await _controller.Description(2) as ViewResult;

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.Model, typeof(RatedMovie));
            var ratedMovie = result.Model as RatedMovie;
            Assert.AreEqual(2, ratedMovie.movie.ID);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _context.Database.EnsureDeleted();
        }
    }
}