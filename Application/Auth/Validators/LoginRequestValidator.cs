using Application.Auth.DTOs;
using FluentValidation;

namespace Application.Auth.Validators;

public class LoginRequestValidator: AbstractValidator<RegisterUserRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(req => req.Email).NotEmpty().EmailAddress();
        RuleFor(req => req.Password).NotEmpty();
    }
}