using Microsoft.AspNetCore.Mvc.Testing;
using MovieReviewsAPI.Entities;

namespace MovieReviewsAPI.IntegrationTests.Helpers
{
    public static class WebApplicationFactoryHelper
    {
        public static void ClearDatabase(this WebApplicationFactory<Program> factory)
        {
            var scopeFactory = factory.Services.GetService<IServiceScopeFactory>();
            using var scope = scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetService<MovieReviewsDbContext>();
            dbContext.ChangeTracker.Clear();
            dbContext.Review.RemoveRange(dbContext.Review.ToList());
            dbContext.Movie.RemoveRange(dbContext.Movie.ToList());
            dbContext.User.RemoveRange(dbContext.User.ToList());
            dbContext.Director.RemoveRange(dbContext.Director.ToList());
            dbContext.SaveChanges();
        }

    }
}
