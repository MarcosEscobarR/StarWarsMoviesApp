using Application.Movies.DTOs;
using FluentValidation;

namespace Application.Movies.Validators;

public class MovieRequestValidator: AbstractValidator<MovieRequest>
{
    public MovieRequestValidator()
    {
        RuleFor(req => req.Title).NotEmpty();
        RuleFor(req => req.Producer).NotEmpty();
        RuleFor(req => req.Director).NotEmpty();
        RuleFor(req => req.ReleaseDate)
            .Must(BeAValidDate)
            .WithMessage("Release date must be a valid date.");
    }
    
    private static bool BeAValidDate(DateTime? date)
    {
        return date.HasValue && date.Value != default;
    }
}