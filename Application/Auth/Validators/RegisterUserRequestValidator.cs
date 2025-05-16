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
            .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{6,}$")
            .WithMessage("Password must contain at least one uppercase letter, one lowercase letter, and one number.");
    }
}