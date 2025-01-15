using Microsoft.Extensions.Configuration;

namespace Ocr.Shared.Utilities;

public static class ConfigurationExtensions
{

    public static string BuildKey(this IConfiguration configuration, string keyName)
    {
        var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";

        string key;
        if(env == "Development")
        {
            key =  Environment.GetEnvironmentVariable(keyName) ?? throw new ArgumentException($"No {keyName} in appsettings.json");
        }
        else
        {
            var keyFileName = keyName + "_FILE";
            var keyPath = Environment.GetEnvironmentVariable(keyFileName) ?? throw new ArgumentException($"No {keyFileName} in Env vars");
            key = DockerSecretUtil.GetSecret(keyPath, keyFileName);
        }

        return key;
    }
    
    public static string BuildConnectionString(this IConfiguration configuration, string name)
    {
        var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";
        
        var connectionString = configuration.GetConnectionString(name) 
            ?? throw new ArgumentException($"No {name} in appsettings.json");

        if (env == "Development") return connectionString;
        
        var postgresPasswordPath = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD_FILE")
                                   ?? throw new ArgumentException("No POSTGRES_PASSWORD_FILE in Env vars");
            
        var pass = DockerSecretUtil.GetSecret(postgresPasswordPath, "POSTGRES_PASSWORD_FILE");
        connectionString += pass;

        return connectionString;
    }
}