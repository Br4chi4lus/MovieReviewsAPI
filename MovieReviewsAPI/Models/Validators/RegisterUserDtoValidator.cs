using FluentValidation;
using MovieReviewsAPI.Entities;
using Npgsql.Replication;

namespace MovieReviewsAPI.Models.Validators
{
    public class RegisterUserDtoValidator : AbstractValidator<RegisterUserDto>
    {
        public RegisterUserDtoValidator(MovieReviewsDbContext dbContext)
        {
            RuleFor(u => u.Email)
                .NotEmpty()
                .EmailAddress()
                .MaximumLength(64)
                .Custom((value, context) =>
                {
                    var emailInUse = dbContext.User.Any(x => x.Email == value);
                    if (emailInUse)
                    {
                        context.AddFailure("Email", "Email already taken");
                    }
                });

            RuleFor(u => u.Password)
                .NotEmpty()
                .MinimumLength(8)
                .MaximumLength(64);

            RuleFor(u => u.PasswordConfirm)
                .Equal(x => x.Password)
                .WithMessage("Passwords must match!");

            RuleFor(u => u.UserName)
                .NotEmpty()
                .MaximumLength(32)
                .Custom((value, context) =>
                {
                    var userNameInUse = dbContext.User.Any(x => x.UserName == value);
                    if (userNameInUse)
                    {
                        context.AddFailure("UserName", "UserName already taken");
                    }
                });

            RuleFor(u => u.DateOfBirth)
                .NotEmpty();
        }
    }
}
