namespace Application.Movies.DTOs;

public record MovieResponse(Guid Id, string Title, string Producer, string Director, DateTime? ReleaseDate);
public record MovieRequest(string Title, string Producer, string Director, DateTime? ReleaseDate);
