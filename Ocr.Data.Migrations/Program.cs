// See https://aka.ms/new-console-template for more information

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Ocr.Data;
using Ocr.Services.Abstract;
using Ocr.Services.Extensions;
using Ocr.Shared.Utilities;

OcrDbContext? dbContext;
ILogger<Program>? logger;
var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false)
    .AddJsonFile($"appsettings.{environmentName}.json", optional: true)
    .AddEnvironmentVariables()
    .Build();

var services = new ServiceCollection();
ConfigureServices();

var serviceProvider = services.BuildServiceProvider();
GetServices(serviceProvider);


RunMigration();
return;

void ConfigureServices()
{
    services.AddTransient<IConfiguration>(_ => configuration);
    services.AddLogging(builder =>
    {
        builder.AddConsole();
        builder.AddDebug();
    });

    services.UseOcrContext(configuration);
}

void GetServices(ServiceProvider sp)
{
    ArgumentNullException.ThrowIfNull(sp);

    logger = sp.GetService<ILogger<Program>>()!;
    dbContext = sp.GetService<OcrDbContext>();
}


void RunMigration()
{
    logger.LogInformation("Starting migration of database schema changes.");

    dbContext.Database.Migrate();

    logger.LogInformation("Finished migration of database schema changes.");
}