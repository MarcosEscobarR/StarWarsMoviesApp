using Application.Movies.DTOs;
using AutoMapper;
using Domain.Entities;

namespace Application.Movies.Mapping;

public class MoviesMappingProfile: Profile
{
    public MoviesMappingProfile()
    {
        CreateMap<Movie, MovieResponse>();
        CreateMap<MovieRequest, Movie>();
    }
}