using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using RelativityAzurePojekt.Controllers;
using RelativityAzurePojekt.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RelativityAzureTests
{
    internal class ReviewControllerTests
    {
        private DbContextOptions<MyDatabaseContext> _options;
        private MyDatabaseContext _context;
        private ReviewController _controller;
        private Mock<IHttpContextAccessor> _httpContextAccessorMock;

        [TestInitialize]
        public void Setup()
        {
            _options = new DbContextOptionsBuilder<MyDatabaseContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryTestDatabase")
                .Options;
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            var context = new DefaultHttpContext();
            _httpContextAccessorMock.Setup(_ => _.HttpContext).Returns(context);
            _context = new MyDatabaseContext(_options);
            var mockLogger = new Mock<ILogger<ReviewController>>();
            _controller = new ReviewController(mockLogger.Object, _context, _httpContextAccessorMock.Object);
        }

        [TestMethod]
        public async Task MovieReviews_ReturnsViewWithReviews()
        {

            var movieId = 1;
            _context.Review.Add(new Review { MovieID = movieId, Stars = 4, Opinion = "Good, even very good must I say" });
            await _context.SaveChangesAsync();

            var result = await _controller.MovieReviews(movieId) as ViewResult;
            Assert.IsNotNull(result);

            var model = result.Model as List<Review>;
            Assert.IsNotNull(model);
            Assert.IsTrue(model.Any());
        }

        [TestMethod]
        public async Task AddReview_AddsReviewAndReturnsMovieReviewsView()
        {

                var movieId = 1;
                _context.Movie.Add(new Movie { ID = movieId, Title = "Test Movie" });
                await _context.SaveChangesAsync();

                var reviewToAdd = new Review { MovieID = movieId, Stars = 3, Opinion = "Meh" };
            
                var result = await _controller.AddReview(reviewToAdd) as ViewResult;
                Assert.IsNotNull(result);

                var model = result.Model as List<Review>;
                Assert.IsNotNull(model);
                Assert.IsTrue(model.Any());

                var addedReview = await _context.Review.FirstOrDefaultAsync(r => r.MovieID == movieId);
                Assert.IsNotNull(addedReview);
                Assert.AreEqual(reviewToAdd.Stars, addedReview.Stars);
                Assert.AreEqual(reviewToAdd.Opinion, addedReview.Opinion);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _context.Database.EnsureDeleted();
        }
    }
}
