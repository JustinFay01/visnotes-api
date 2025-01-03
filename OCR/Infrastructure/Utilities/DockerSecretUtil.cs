namespace OCR.Infrastructure.Utilities;

public static class DockerSecretUtil
{
    public static string GetSecret(string secretName)
    {
        var secretPath = $"/run/secrets/{secretName}";
        if (!File.Exists(secretPath))
        {
            throw new Exception($"Secret {secretName} not found at {secretPath}");
        }

        return File.ReadAllText(secretPath);
    }
}