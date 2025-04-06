using FluentAssertions;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using MovieReviewsAPI.Entities;
using MovieReviewsAPI.IntegrationTests.Helpers;

namespace MovieReviewsAPI.IntegrationTests
{
    public class ReviewControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private HttpClient _client;
        private WebApplicationFactory<Program> _factory;

        public ReviewControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        var dbContextOptions = services
                            .SingleOrDefault(service => service.ServiceType == typeof(DbContextOptions<MovieReviewsDbContext>));
                        services.Remove(dbContextOptions);
                        services.AddSingleton<IPolicyEvaluator, FakeAuthPolicyEvaluator>();
                        services.AddMvc(options => options.Filters.Add(new FakeUserFilter()));
                        services
                            .AddDbContext<MovieReviewsDbContext>(options => options.UseInMemoryDatabase("MovieReviewsDb"));

                    });
                });
            _client = _factory
                .CreateClient();
        }

        [Fact]
        public async Task DeleteReview_NoReviewFound_ReturnsNotFound()
        {
            // Arrange
            _factory.ClearDatabase();

            // Act
            var result = await _client.DeleteAsync("/api/movies/1/reviews/100");

            // Assert
            result.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task DeleteReview_ForReviewOwnerOrAdmin_ReturnsNoContent(int userId)
        {
            // Arrange
            _factory.ClearDatabase();
            var review = new Review()
            {
                Id = 1,
                MovieId = 1,
                Rating = 5,
                Content = "Content",
                DateOfCreation = DateTime.Now,
                DateOfLastModification = DateTime.Now,
                Author = new User() { Id = userId, Email = "t@t.t", FirstName = "name", LastName = "last", PasswordHash = " 123", UserName = "user1" }
            };
            var scopeFactory =  _factory.Services.GetService<IServiceScopeFactory>();
            using var scope = scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetService<MovieReviewsDbContext>();

            dbContext.Review.Add(review);
            dbContext.SaveChanges();
            // Act
            var result = await _client.DeleteAsync("/api/movies/1/reviews/" + review.Id);

            // Assert

            result.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
        }
    }
}
