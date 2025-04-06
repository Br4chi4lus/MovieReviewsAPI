using Microsoft.EntityFrameworkCore;
using Moq;
using MovieReviewsAPI.Entities;
using MovieReviewsAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using CalendarAPI.Services;
using Microsoft.AspNetCore.Authorization;
using FluentAssertions;
using MovieReviewsAPI.Exceptions;
using MovieReviewsAPI.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace MovieReviewsAPI.Tests
{
    public class ReviewServiceTests
    {
        private (MovieReviewsDbContext dbContext, IMapper automapper, IUserContextService userContextService, Mock<IAuthorizationService> authorizationServiceMock) SetupTestEnvironment(int? userId, string userRole)
        {
            var options = new DbContextOptionsBuilder<MovieReviewsDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            var dbContext = new MovieReviewsDbContext(options);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MovieReviewsMappingProfile());
            });
            var automapper = config.CreateMapper();

            var httpContextMock = new Mock<IHttpContextAccessor>();
            if (userId != null) {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                    new Claim(ClaimTypes.NameIdentifier, userRole)
                };
                var identity = new ClaimsIdentity(claims, "TestAuth");
                var user = new ClaimsPrincipal(identity);
                httpContextMock.Setup(h => h.HttpContext).Returns(new DefaultHttpContext { User = user });
            }
            else
            {
                httpContextMock.Setup(h => h.HttpContext).Returns((HttpContext?)null);
            }
            var userContextService = new UserContextService(httpContextMock.Object);

            var authorizationServiceMock = new Mock<IAuthorizationService>();

            return (dbContext, automapper, userContextService, authorizationServiceMock);
        }
        [Fact]
        public async Task GetAllReviews_NoMovieFound_ThrowsNotFoundException()
        {
            // Arrange
            var (dbContext, automapper, userContextService, authorizationServiceMock) = this.SetupTestEnvironment(1, "User");
            var reviewService = new ReviewService(dbContext, automapper, userContextService, authorizationServiceMock.Object);
            
            // Act + Assert
            await reviewService.Invoking(s => s.GetAllReviews(1)).Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task GetAllReviews_ReviewsFound_ReturnsTwoReviews()
        {
            // Arrange 
            var (dbContext, automapper, userContextService, authorizationServiceMock) = this.SetupTestEnvironment(1, "User");
            var reviewService = new ReviewService(dbContext, automapper, userContextService, authorizationServiceMock.Object);
            var date = DateTime.Now;
            await dbContext.Movie.AddAsync(new Movie() { 
                Id = 1,
                Title = "Title",
                DateOfPremiere = DateTime.Now,
                Description = "Description",
                Director = new Director() {
                    Id = 1,
                    FirstName = "FirstName",
                    LastName = "LastName"
                },
            });
            await dbContext.Review.AddRangeAsync(new Review()
            {
                Id = 1,
                MovieId = 1,
                Rating = 5,
                Content = "Content",
                DateOfCreation = date,
                DateOfLastModification = date,
                Author = new User() { Id = 1, Email = "t@t.t", FirstName = "name", LastName = "last", PasswordHash = " 123", UserName = "user1"}
            }, new Review()
            {
                Id = 2,
                MovieId = 1,
                Rating = 6,
                Content = "Content1",
                DateOfCreation = date,
                DateOfLastModification = date,
                Author = new User() { Id = 2, Email = "2t@t.t", FirstName = "name", LastName = "last", PasswordHash = " 123", UserName = "user12"}
            });
            await dbContext.SaveChangesAsync();

            // Act
            var result = await reviewService.GetAllReviews(1);

            // Assert        
            result.Should().HaveCount(2);
            var firstReview = result.ElementAt(0);
            firstReview.Id.Should().Be(1);
            firstReview.Rating.Should().Be(5);
            firstReview.Content.Should().Be("Content");
            firstReview.DateOfCreation.Should().Be(date);
            firstReview.DateOfLastModification.Should().Be(date);
            firstReview.UserName.Should().Be("user1");
            var secondReview = result.ElementAt(1);
            secondReview.Id.Should().Be(2);
            secondReview.Rating.Should().Be(6);
            secondReview.Content.Should().Be("Content1");
            secondReview.DateOfCreation.Should().Be(date);
            secondReview.DateOfLastModification.Should().Be(date);
            secondReview.UserName.Should().Be("user12");
        }

        [Fact]
        public async Task CreateReview_UserNotLoggedIn_ThrowsUnauthorizedException()
        {
            // Arrange
            var (dbContext, automapper, userContextService, authorizationServiceMock) = this.SetupTestEnvironment(null, "User");
            var reviewService = new ReviewService(dbContext, automapper, userContextService, authorizationServiceMock.Object);
            var createReviewDto = new CreateReviewDto()
            {
                Content = "Something",
                Rating = 3
            };
            // Act + Assert
            await reviewService.Invoking(s => s.CreateReview(1, createReviewDto)).Should().ThrowAsync<UnauthorizedException>();
        }

        [Fact]
        public async Task CreateReview_NoMovieFound_ThrowsNotFoundException()
        {
            // Arrange
            var (dbContext, automapper, userContextService, authorizationServiceMock) = this.SetupTestEnvironment(1, "User");
            var reviewService = new ReviewService(dbContext, automapper, userContextService, authorizationServiceMock.Object);
            var createReviewDto = new CreateReviewDto()
            {
                Content = "Something",
                Rating = 3
            };

            // Act + Assert
            await reviewService.Invoking(s => s.CreateReview(1, createReviewDto)).Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task CreateReview_ReviewCreatedSuccesfully_ReturnsReviewDto()
        {
            // Arrange 
            var (dbContext, automapper, userContextService, authorizationServiceMock) = this.SetupTestEnvironment(1, "User");
            var reviewService = new ReviewService(dbContext, automapper, userContextService, authorizationServiceMock.Object);
            var date = DateTime.Now;
            await dbContext.Movie.AddAsync(new Movie()
            {
                Id = 1,
                Title = "Title",
                DateOfPremiere = DateTime.Now,
                Description = "Description",
                Director = new Director()
                {
                    Id = 1,
                    FirstName = "FirstName",
                    LastName = "LastName"
                },
            });
            await dbContext.User.AddAsync(new User() 
            { 
                Id = 1,
                Email = "t@t.t",
                FirstName = "name",
                LastName = "last",
                PasswordHash = " 123",
                UserName = "user1" 
            });
            await dbContext.SaveChangesAsync();
            CreateReviewDto createReviewDto = new CreateReviewDto()
            {
                Content = "Abc",
                Rating = 10
            };

            // Act
            var result = await reviewService.CreateReview(1, createReviewDto);

            // Assert
            result.Rating.Should().Be(10);
            result.Content.Should().Be("Abc");
            result.Id.Should().NotBe(null).And.BeGreaterThanOrEqualTo(1);
        }


        [Fact]
        public async Task DeleteReview_ReviewNotFound_ThrowsNotFoundException()
        {
            // Arrange 
            var (dbContext, automapper, userContextService, authorizationServiceMock) = this.SetupTestEnvironment(1, "User");
            var reviewService = new ReviewService(dbContext, automapper, userContextService, authorizationServiceMock.Object);

            // Act + Assert
            await reviewService.Invoking(s => s.DeleteReview(1)).Should().ThrowAsync<NotFoundException>().WithMessage("Review does not exist");
        }

        [Fact]
        public async Task DeleteReview_UserNotLoggedIn_ThrowsUnauthorizedException()
        {
            // Arrange
            var (dbContext, automapper, userContextService, authorizationServiceMock) = this.SetupTestEnvironment(null, "User");
            var reviewService = new ReviewService(dbContext, automapper, userContextService, authorizationServiceMock.Object);
            await dbContext.Review.AddAsync(new Review()
            {
                Id = 1,
                MovieId = 1,
                Rating = 5,
                Content = "Content",
                DateOfCreation = DateTime.Now,
                DateOfLastModification = DateTime.Now,
                Author = new User() { Id = 1, Email = "t@t.t", FirstName = "name", LastName = "last", PasswordHash = " 123", UserName = "user1" }
            });
            await dbContext.SaveChangesAsync();

            // Act + Assert
            await reviewService.Invoking(s => s.DeleteReview(1)).Should().ThrowAsync<UnauthorizedException>().WithMessage("You are not logged in");
        }

        [Fact]
        public async Task DeleteReview_UserNotAuthorized_ThrowsForbidenException()
        {
            // Arrange
            var (dbContext, automapper, userContextService, authorizationServiceMock) = this.SetupTestEnvironment(2, "User");
            authorizationServiceMock
                .Setup(m => m.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<Review>(), It.IsAny<IEnumerable<IAuthorizationRequirement>>()))
                .ReturnsAsync(AuthorizationResult.Failed());
               
            var reviewService = new ReviewService(dbContext, automapper, userContextService, authorizationServiceMock.Object);
            await dbContext.Review.AddAsync(new Review()
            {
                Id = 1,
                MovieId = 1,
                Rating = 5,
                Content = "Content",
                DateOfCreation = DateTime.Now,
                DateOfLastModification = DateTime.Now,
                Author = new User() { Id = 2, Email = "t@t.t", FirstName = "name", LastName = "last", PasswordHash = " 123", UserName = "user1" }
            });
            await dbContext.SaveChangesAsync();

            // Act + Assert
            await reviewService.Invoking(s => s.DeleteReview(1)).Should().ThrowAsync<ForbidenException>();
        }
    }
}
