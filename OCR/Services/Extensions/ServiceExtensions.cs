using Microsoft.EntityFrameworkCore;
using OCR.Data;
using OCR.Infrastructure;
using OCR.Infrastructure.SystemStorage;
using OCR.Services.Profiles;

namespace OCR.Services.Extensions;

public static class ServicesExtensions
{
    public static IServiceCollection UseOcrServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddAutoMapper(typeof(NoteProfile).Assembly);
        
        services.AddDbContext<OcrDbContext>(options => 
            options.UseNpgsql(config.GetConnectionString("DefaultConnection")));
        
        services.AddScoped<INoteService, NoteService>();
        return services;
    }
}