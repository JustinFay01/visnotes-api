using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Ocr.Data;
using Ocr.Services.Profiles;
using Ocr.Services.SystemStorage;
using Ocr.Services.Utilities;

namespace Ocr.Services.Extensions;

public static class ServicesExtensions
{
    public static IServiceCollection UseOcrContext(this IServiceCollection services, IConfiguration config)
    {
        var postgresPassword = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD") ??
                               throw new ArgumentException("No POSTGRES_PASSWORD in Env vars");
        var parsedPassword = DockerSecretUtil.GetSecret(postgresPassword);
        
        services.AddDbContext<OcrDbContext>(options =>
            options.UseNpgsql(config.GetConnectionString("DefaultConnection") + parsedPassword));

        return services;
    }
    public static IServiceCollection UseOcrServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddAutoMapper(typeof(NoteProfile).Assembly);

        services.AddScoped<INoteService, NoteService>();
        return services;
    }

    public static IServiceCollection UseOcrInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<IFileSystemStorage>(_ =>
        {
            var storagePath = Environment.GetEnvironmentVariable("OCR_STORAGE_PATH")
                              ?? throw new ArgumentException("No OCR_STORAGE_PATH in Env vars");
            return new FileSystemStorage(storagePath);
        });


        services.AddScoped<IVisionService, VisionService>(provider =>
        {
            var keyName = Environment.GetEnvironmentVariable("VISION_KEY") ??
                          throw new ArgumentException("No VISION_KEY Env var");
            var endpointName = Environment.GetEnvironmentVariable("VISION_ENDPOINT") ??
                               throw new ArgumentException("No VISION_ENDPOINT Env var");
            var logger = provider.GetRequiredService<ILogger<VisionService>>();

            return new VisionService(DockerSecretUtil.GetSecret(keyName), DockerSecretUtil.GetSecret(endpointName),
                logger);
        });


        services.AddScoped<IDocumentIntelligenceService, DocumentIntelligenceService>(provider =>
        {
            var key = Environment.GetEnvironmentVariable("DI_KEY") ??
                      throw new ArgumentException("No DI_KEY Env var");

            var endpoint = Environment.GetEnvironmentVariable("DI_ENDPOINT") ??
                           throw new ArgumentException("No DI_ENDPOINT Env var");

            var logger = provider.GetRequiredService<ILogger<DocumentIntelligenceService>>();

            return new DocumentIntelligenceService(
                DockerSecretUtil.GetSecret(key),
                DockerSecretUtil.GetSecret(endpoint),
                logger
            );
        });

        return services;
    }
}