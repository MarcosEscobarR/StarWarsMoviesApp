using System.Text.Json.Serialization;
using Domain.Entities;

namespace Infrastructure.Jobs;

public record SwapiResponse(
    List<SwapiResult> result
);

public record SwapiResult(SwapiProperties properties);

public record SwapiProperties(string title, string producer, string director, DateTime? release_date);
