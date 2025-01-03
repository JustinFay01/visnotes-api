using OCR.Infrastructure.SystemStorage;
using OCR.Infrastructure.Utilities;

namespace OCR.Infrastructure.Extensions;

public static class InfrastructureExtensions
{
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
        
            return new VisionService(DockerSecretUtil.GetSecret(keyName), DockerSecretUtil.GetSecret(endpointName), logger);
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