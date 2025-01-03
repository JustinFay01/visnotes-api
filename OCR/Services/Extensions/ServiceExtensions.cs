using Microsoft.EntityFrameworkCore;
using OCR.Data;
using OCR.Infrastructure;
using OCR.Infrastructure.SystemStorage;
using OCR.Infrastructure.Utilities;
using OCR.Services.Profiles;

namespace OCR.Services.Extensions;

public static class ServicesExtensions
{
    public static IServiceCollection UseOcrServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddAutoMapper(typeof(NoteProfile).Assembly);
        var postgresPassword = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD") ?? throw new ArgumentException("No POSTGRES_PASSWORD in Env vars");
        var parsedPassword = DockerSecretUtil.GetSecret(postgresPassword);
        services.AddDbContext<OcrDbContext>(options => 
            options.UseNpgsql(config.GetConnectionString("DefaultConnection") + parsedPassword));
        
        services.AddScoped<INoteService, NoteService>();
        return services;
    }
}