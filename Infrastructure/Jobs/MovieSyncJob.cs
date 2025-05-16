using System.Text.Json;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Jobs;

public class MovieSyncJob(AppDbContext dbContext, HttpClient httpClient, ILogger<MovieSyncJob> logger)
{
    public async Task SyncMoviesAsync()
    {
        try
        {
            var response = await httpClient.GetAsync("https://www.swapi.tech/api/films");
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException(response.ReasonPhrase);
            }

            var content = await response.Content.ReadAsStringAsync();
            var movies = JsonSerializer.Deserialize<SwapiResponse>(content);
            logger.LogInformation(JsonSerializer.Serialize(movies));

            if (movies is null)
            {
                throw new Exception("Failed to deserialize movies");
            }

            foreach (var result in movies.result)
            {
                var movie = result.properties;
                if (!dbContext.Movies.Any(m => m.Title == movie.title))
                {
                    var newMovie = new Movie
                    {
                        Title = movie.title,
                        Producer = movie.producer,
                        Director = movie.director,
                        ReleaseDate = movie.release_date
                    };
                
                    dbContext.Movies.Add(newMovie);
                }
                
            }

            await dbContext.SaveChangesAsync();
        }
        catch (Exception e)
        {
            logger.LogError(e, "An error occurred while syncing movies");
            throw;
        }
    }
}