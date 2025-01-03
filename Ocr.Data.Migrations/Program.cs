// See https://aka.ms/new-console-template for more information

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Ocr.Data;

OcrDbContext? dbContext;
ILogger<Program>? logger;
// var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
// var builder = new ConfigurationBuilder();
// var configuration = builder
//     .SetBasePath(Directory.GetCurrentDirectory())
//     .AddJsonFile("appsettings.json", true)
//     .AddJsonFile($"appsettings.{environmentName}.json", true)
//     .AddEnvironmentVariables()
//     .Build();
// var services = new ServiceCollection();
// ConfigureServices(services);
//
// var serviceProvider = services.BuildServiceProvider();
// GetServices(serviceProvider);
// return;
//
// void ConfigureServices(IServiceCollection services)
// {
//     services.AddTransient<IConfiguration>(_ => configuration);
//
//     services.AddLogging(loggingBuilder =>
//     {
//         loggingBuilder.AddDebug();
//         loggingBuilder.AddSimpleConsole();
//     });
//
//     // services.UseOcrServices()
// }

void GetServices(ServiceProvider serviceProvider)
{
    logger = serviceProvider.GetService<ILogger<Program>>()!;

    //dbContext = serviceProvider.GetService<GenosContext>()!;
}


void RunMigration()
{
    logger.LogInformation("Starting migration of database schema changes.");

    dbContext.Database.Migrate();

    logger.LogInformation("Finished migration of database schema changes.");
}