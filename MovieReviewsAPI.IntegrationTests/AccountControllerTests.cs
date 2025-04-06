using FluentAssertions;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Moq;
using MovieReviewsAPI.Entities;
using MovieReviewsAPI.IntegrationTests.Helpers;
using MovieReviewsAPI.Models;
using MovieReviewsAPI.Services;

namespace MovieReviewsAPI.IntegrationTests
{
    public class AccountControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private WebApplicationFactory<Program> _factory;
        private HttpClient _client;
        private Mock<IAccountService> _accountServiceMock = new Mock<IAccountService>();
        public AccountControllerTests(WebApplicationFactory<Program> factory) {
            _factory = factory
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        var dbContextOptions = services
                            .SingleOrDefault(service => service.ServiceType == typeof(DbContextOptions<MovieReviewsDbContext>));
                        services.Remove(dbContextOptions);
                        services.AddDbContext<MovieReviewsDbContext>(options => options.UseInMemoryDatabase("MovieReviewsTestDb"));

                        services.AddSingleton<IAccountService>(_accountServiceMock.Object);
                    });
                });
            _client = _factory.CreateClient();

        }
        [Fact]
        public async Task RegisterUser_ValidModel_ReturnsCreated()
        {
            // Arrange
            var dto = new RegisterUserDto()
            {
                Email = "email@test.com",
                UserName = "abcdefg1",
                Password = "some_password",
                PasswordConfirm = "some_password",
                DateOfBirth = DateTime.Now,
                FirstName = "Adam"
            };

            var httpContent = dto.ToJsonHttpContent();

            // Act
            var result = await _client.PostAsync("/api/account/register", httpContent);

            // Assert
            result.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
            result.Content.Should().NotBeNull();
        }

        [Fact]
        public async Task RegisterUser_InvalidModel_ReturnsCreated()
        {
            // Arrange
            var dto = new RegisterUserDto()
            {
                Email = "email",
                UserName = "abcdefg1",
                Password = "some_password",
                PasswordConfirm = "some_password",
                DateOfBirth = DateTime.Now,
                FirstName = "Adam"
            };

            var httpContent = dto.ToJsonHttpContent();

            // Act
            var result = await _client.PostAsync("/api/account/register", httpContent);

            // Assert
            result.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task LoginUser_UserRegistered_ReturnsOk()
        {
            // Arrange
            var dto = new LoginUserDto()
            {
                Email = "email@a.com",
                Password = "abcdefghij"
            };
            var httpContent = dto.ToJsonHttpContent();

            _accountServiceMock
                .Setup(m => m.LoginUser(It.IsAny<LoginUserDto>()))
                .Returns(Task.FromResult("abc"));

            // Act
            var result = await _client.PostAsync("/api/account/login", httpContent);

            // Assert
            result.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }
    }
}
