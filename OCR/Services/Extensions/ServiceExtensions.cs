namespace OCR.Services.Extensions;

public static class ServicesExtensions
{
    public static IServiceCollection UseOcrServices(this IServiceCollection services)
    {
        services.AddScoped<IVisionService, VisionService>(provider =>
        {
            var key = Environment.GetEnvironmentVariable("VISION_KEY") ??
                      throw new ArgumentException("No VISION_KEY Env var");
            var endpoint = Environment.GetEnvironmentVariable("VISION_ENDPOINT") ??
                           throw new ArgumentException("No VISION_ENDPOINT Env var");
            var logger = provider.GetRequiredService<ILogger<VisionService>>();
            
            return new VisionService(key, endpoint, logger);
        });

        return services;
    }
}