using Microsoft.AspNetCore.Authorization;
using MovieReviewsAPI.Entities;
using System.Security.Claims;

namespace MovieReviewsAPI.Authorization
{
    public class ResourceOperationRequirementHandler : AuthorizationHandler<ResourceOperationRequirement, Review>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ResourceOperationRequirement requirement, Review review)
        {
            if (requirement.ResourceOperation == ResourceOperation.Read ||
                requirement.ResourceOperation == ResourceOperation.Create)
            {
                context.Succeed(requirement);
            }

            var userId = context.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value;

            if (review.UserId == int.Parse(userId))
            {
                context.Succeed(requirement);
            }

            if (context.User.IsInRole("Admin") || context.User.IsInRole("Moderator"))
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
