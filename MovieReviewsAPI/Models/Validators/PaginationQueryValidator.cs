using FluentValidation;

namespace MovieReviewsAPI.Models.Validators
{
    public class PaginationQueryValidator : AbstractValidator<PaginationQuery>
    {
        private int[] allowedPageSizes = new[] { 5, 10, 15, 20 };
        public PaginationQueryValidator()
        {
            RuleFor(q => q.PageSize).Custom((value, context) =>
            {
                if (!allowedPageSizes.Contains(value))
                {
                    context.AddFailure("PageSize", $"Page size must be in [{string.Join(",", allowedPageSizes)}]");
                }
            });
            RuleFor(q => q.PageNumber).GreaterThanOrEqualTo(1);
        }
    }
}
