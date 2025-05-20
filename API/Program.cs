using System.Reflection;
using Application.Auth.Mapping;
using Application.Auth.Services;
using Application.Auth.Validators;
using Application.Movies.Services;
using Domain.Entities;
using FluentValidation.AspNetCore;
using Hangfire;
using Infrastructure;
using Infrastructure.Jobs;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();

// Add services to the container.
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IMoviesServices,MoviesServices>();
builder.Services.AddAutoMapper(typeof(AuthMappingProfile));
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();


builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddControllers()
    .AddFluentValidation(fv =>
    {
        fv.RegisterValidatorsFromAssemblyContaining<RegisterUserRequestValidator>();
    });;
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opts =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    opts.IncludeXmlComments(xmlPath);
    
    opts.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Ingresa el token JWT con prefijo 'Bearer', Ejemplo: Bearer [TOKEN]"
    });
    
    opts.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            []
        }
    });
});

var app = builder.Build();
// Ejecutar migraciones al iniciar
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>(); 
    dbContext.Database.Migrate();
}
app.UseHangfireDashboard("/jobs", new DashboardOptions
{
    Authorization = new[] { new DashboardAuthorizationFilter() }
});// Configure the HTTP request pipeline.
Console.WriteLine(app.Environment.EnvironmentName);

    Console.WriteLine("Is Staging");
    app.UseSwagger();
    app.UseSwaggerUI();


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var scheduler = scope.ServiceProvider.GetRequiredService<HangfireJobScheduler>();
    scheduler.ScheduleJobs();
}

app.Run();

public partial class Program { }
