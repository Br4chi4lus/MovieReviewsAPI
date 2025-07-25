using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MovieReviewsAPI.Entities;
using Microsoft.Extensions.DependencyInjection;
using MovieReviewsAPI.Models;
using Newtonsoft.Json;
using System.Text;
using Microsoft.AspNetCore.Authorization.Policy;
using MovieReviewsAPI.IntegrationTests.Helpers;
using Microsoft.AspNetCore.WebUtilities;
namespace MovieReviewsAPI.IntegrationTests
{
    public class MovieControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private HttpClient _client;
        private WebApplicationFactory<Program> _factory;
        public MovieControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory
                .WithWebHostBuilder(builder => {
                    builder.ConfigureServices(services =>
                    {
                        var dbContextOptions = services
                            .SingleOrDefault(service => service.ServiceType == typeof(DbContextOptions<MovieReviewsDbContext>));
                        services.Remove(dbContextOptions);
                        services.AddSingleton<IPolicyEvaluator, FakeAuthPolicyEvaluator>();
                        services.AddMvc(options => options.Filters.Add(new FakeUserFilter()));
                        services
                            .AddDbContext<MovieReviewsDbContext>(options => options.UseInMemoryDatabase("MovieReviewsTestDb"));
                    });
                });
            _client = _factory
                .CreateClient();
        }
        [Fact]
        public async Task GetAllMovies_ReturnsOkResult()
        {
            // Arrange
            _factory.ClearDatabase();
            var url = QueryHelpers.AddQueryString("/api/movies", new Dictionary<string, string?>
            {
                ["PageNumber"] = "1",
                ["PageSize"] = "10"
            });
            // Act
            var result = await _client.GetAsync(url);
            // Assert
            result.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetAllMovies_WithInvalidQueryValues_ReturnsBadRequest()
        {
            // Arrange
            _factory.ClearDatabase();
            var url = QueryHelpers.AddQueryString("/api/movies", new Dictionary<string, string?>
            {
                ["PageNumber"] = "1",
                ["PageSize"] = "11"
            });

            // Act
            var result = await _client.GetAsync(url);

            // Assert
            result.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task CreateMovie_WithValidModel_ReturnsCreatedResult()
        {
            // Arrange
            var dto = new CreateMovieDto()
            {
                Title = "Title",
                DirectorFirstName = "FirstName",
                DirectorLastName = "LastName"
            };
            var httpContent = dto.ToJsonHttpContent();
            // Act
            var result = await _client.PostAsync("/api/movies", httpContent);

            // Assert
            result.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
            result.Headers.Location.Should().NotBeNull();
        }

        [Fact]
        public async Task CreateMovie_WithInvalidModel_ReturnsBadRequest()
        {
            // Arrange
            var dto = new CreateMovieDto()
            {
                Title = "Title",
                DirectorFirstName = "FirstName"
            };
            var httpContent = dto.ToJsonHttpContent();

            // Act
            var result = await _client.PostAsync("/api/movies", httpContent);

            // Assert
            result.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task DeleteMovie_MovieNotFound_ReturnsNotFound()
        {
            // Arrange
            _factory.ClearDatabase();
            // Act
            var result = await _client.DeleteAsync("/api/movies/100");

            // Assert
            result.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }
    }
}
