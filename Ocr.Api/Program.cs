using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Ocr.Api.Auth;
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
        // Program.cs
        var domain = $"https://{builder.Configuration["Auth0:Domain"]}/";
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.Authority = domain;
            options.Audience = builder.Configuration["Auth0:Audience"];
            options.TokenValidationParameters = new TokenValidationParameters
            {
                NameClaimType = ClaimTypes.NameIdentifier
            };
        });

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
        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("read:notes", policy => policy.Requirements.Add(new 
                HasScopeRequirement("read:notes", domain)));
            
            options.AddPolicy("write:notes", policy => policy.Requirements.Add(new
                HasScopeRequirement("write:notes", domain)));
        });
        
        builder.Services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();

        builder.Services.AddExceptionHandler<FileConflictExceptionHandler>();
        builder.Services.AddExceptionHandler<ArgumentExceptionHandler>();
        builder.Services.AddExceptionHandler<ValidationExceptionHandler>();
        builder.Services.AddExceptionHandler<EntityNotFoundExceptionHandler>();
        builder.Services.AddExceptionHandler<ExceptionHandler>();

        builder.Services.AddControllers();


        builder.Services.UseOcrContext(builder.Configuration);
        builder.Services.UseOcrInfrastructure(builder.Configuration);
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

        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
        app.Run();
    }
}