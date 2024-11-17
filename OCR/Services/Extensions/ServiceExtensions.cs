namespace OCR.Services.Extensions;

public static class ServicesExtensions
{
    public static IServiceCollection UseOcrServices(this IServiceCollection services)
    {
        services.AddScoped<IVisionService, VisionService>(provider =>
        {
            var key = Environment.GetEnvironmentVariable("VISION_KEY");
            var endpoint = Environment.GetEnvironmentVariable("VISION_ENDPOINT");
            var logger = provider.GetRequiredService<ILogger<VisionService>>();
            
            return new VisionService(
                endpoint ?? throw new ArgumentException("No VISION_KEY Env var"), 
                key ?? throw new ArgumentException("No VISION_ENDPOINT Env var"), 
                logger);
        });

        return services;
    }
}