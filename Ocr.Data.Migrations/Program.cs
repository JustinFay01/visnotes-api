// See https://aka.ms/new-console-template for more information

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Ocr.Data;
using Ocr.Services.Extensions;

OcrDbContext? dbContext;
ILogger<Program>? logger;
var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false)
    .AddJsonFile($"appsettings.{environmentName}.json", optional: true)
    .AddEnvironmentVariables()
    .Build();

var services = new ServiceCollection();
ConfigureServices(services);

var serviceProvider = services.BuildServiceProvider();
GetServices(serviceProvider);
return;

void ConfigureServices(IServiceCollection s)
{
    s.AddTransient<IConfiguration>(_ => configuration);
    s.AddLogging(builder =>
    {
        builder.AddConsole();
        builder.AddDebug();
    });

    s.UseOcrServices();
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