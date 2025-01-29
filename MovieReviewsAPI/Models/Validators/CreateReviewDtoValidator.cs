using FluentValidation;

namespace MovieReviewsAPI.Models.Validators
{
    public class CreateReviewDtoValidator : AbstractValidator<CreateReviewDto>
    {
        public CreateReviewDtoValidator()
        {
            RuleFor(r => r.Rating)
                .NotEmpty()
                .LessThanOrEqualTo(10)
                .GreaterThanOrEqualTo(1);
                
        }
    }
}
