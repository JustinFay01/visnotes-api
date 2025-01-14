using Ocr.Api.ExceptionHandlers;
using Ocr.Services.Abstract;
using Ocr.Services.Extensions;
using Ocr.Shared.Utilities;

namespace Ocr.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Allow cors
        var myAllowSpecificOrigins = "_myAllowSpecificOrigins";
        builder.Services.AddCors(options =>
        {
            options.AddPolicy(myAllowSpecificOrigins,
                corsPolicyBuilder =>
                {
                    corsPolicyBuilder.WithOrigins("*")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
        });

        // Add services to the container.
        builder.Services.AddAuthorization();

        builder.Services.AddExceptionHandler<FileConflictExceptionHandler>();
        builder.Services.AddExceptionHandler<ArgumentExceptionHandler>();
        builder.Services.AddExceptionHandler<ValidationExceptionHandler>();
        builder.Services.AddExceptionHandler<EntityNotFoundExceptionHandler>();
        builder.Services.AddExceptionHandler<ExceptionHandler>();

        builder.Services.AddControllers();
        
        // If not in development, use Docker stack secrets
        ResolveSecret? secretResolver = !builder.Environment.IsDevelopment() ? 
            DockerSecretUtil.GetSecret 
            : null;
        
        builder.Services.UseOcrContext(builder.Configuration, secretResolver);
        builder.Services.UseOcrInfrastructure(secretResolver);
        builder.Services.UseOcrServices();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseExceptionHandler(options => { });

        app.UseHttpsRedirection();

        // Needs to be before UseAuthorization
        app.UseCors(myAllowSpecificOrigins);

        app.UseAuthorization();
        app.MapControllers();
        app.Run();
    }
}