using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Ocr.Data;
using Ocr.Services.Abstract;
using Ocr.Services.Profiles;
using Ocr.Services.SystemStorage;
using Ocr.Shared.Utilities;

namespace Ocr.Services.Extensions;

public static class ServicesExtensions
{
    public static IServiceCollection UseOcrContext(this IServiceCollection services, IConfiguration config)
    {
        var connectionString = config.BuildConnectionString("DefaultConnection");
        
        services.AddDbContext<OcrDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });

        return services;
    }
    public static IServiceCollection UseOcrServices(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(NoteProfile).Assembly);

        services.AddScoped<INoteService, NoteService>();
        return services;
    }

    public static IServiceCollection UseOcrInfrastructure(this IServiceCollection services)
    {
        var storagePath = Environment.GetEnvironmentVariable("OCR_STORAGE_PATH")
                          ?? throw new ArgumentException("No OCR_STORAGE_PATH in Env vars");
        services.AddSingleton<IFileSystemStorage>(_ => new FileSystemStorage(storagePath));
        
        var diKey = Environment.GetEnvironmentVariable("DI_KEY")
                    ?? throw new ArgumentException("No DI_KEY in Env vars");
        var diEndpoint = Environment.GetEnvironmentVariable("DI_ENDPOINT")
                        ?? throw new ArgumentException("No DI_ENDPOINT in Env vars");
        
        if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
        {
            diKey = DockerSecretUtil.GetSecret(diKey, "DI_KEY");
            diEndpoint = DockerSecretUtil.GetSecret(diEndpoint, "DI_ENDPOINT");
        }

        services.AddScoped<IDocumentIntelligenceService, DocumentIntelligenceService>(provider =>
        {
            var logger = provider.GetRequiredService<ILogger<DocumentIntelligenceService>>();

            return new DocumentIntelligenceService(
                diKey,
                diEndpoint,
                logger
            );
        });

        return services;
    }
}