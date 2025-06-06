using Application.Auth.DTOs;
using FluentValidation;

namespace Application.Auth.Validators;

public class RegisterUserRequestValidator: AbstractValidator<RegisterUserRequest>
{
    public RegisterUserRequestValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Name).NotEmpty().MinimumLength(2).MaximumLength(50);
        RuleFor(x => x.Lastname).NotEmpty().MinimumLength(2).MaximumLength(50);
        RuleFor(x => x.Password).NotEmpty().MinimumLength(6).MaximumLength(50)
            .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{6,50}$")
            .WithMessage("Password must contain at least one uppercase letter, one lowercase letter, and one number.");
        RuleFor(x => x.UserRole).NotEmpty().Must(role => role == "Admin" || role == "User")
            .WithMessage("UserRole must be either 'Admin' or 'User'.");
    }
}