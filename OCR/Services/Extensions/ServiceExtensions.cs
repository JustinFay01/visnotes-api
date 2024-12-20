using Microsoft.EntityFrameworkCore;
using OCR.Data;

namespace OCR.Services.Extensions;

public static class ServicesExtensions
{
    public static IServiceCollection UseOcrServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<OcrDbContext>(options => 
            options.UseNpgsql(config.GetConnectionString("DefaultConnection")));
        
        services.AddScoped<IVisionService, VisionService>(provider =>
        {
            var key = Environment.GetEnvironmentVariable("VISION_KEY") ??
                      throw new ArgumentException("No VISION_KEY Env var");
            var endpoint = Environment.GetEnvironmentVariable("VISION_ENDPOINT") ??
                           throw new ArgumentException("No VISION_ENDPOINT Env var");
            var logger = provider.GetRequiredService<ILogger<VisionService>>();
            
            return new VisionService(key, endpoint, logger);
        });


        services.AddScoped<IDocumentIntelligenceService, DocumentIntelligenceService>(provider =>
        {
            var key = Environment.GetEnvironmentVariable("DI_KEY") ??
                      throw new ArgumentException("No DI_KEY Env var");

            var endpoint = Environment.GetEnvironmentVariable("DI_ENDPOINT") ??
                           throw new ArgumentException("No DI_ENDPOINT Env var");

            var logger = provider.GetRequiredService<ILogger<DocumentIntelligenceService>>();

            return new DocumentIntelligenceService(key, endpoint, logger);
        });

        return services;
    }
}