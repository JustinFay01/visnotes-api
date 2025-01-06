namespace Ocr.Shared.Utilities;

public static class DockerSecretUtil
{
    public static string GetSecret(string secretName, string logSafeSecretName)
    {
        if (string.IsNullOrEmpty(secretName))
            throw new ArgumentException("Attempted to get secret with empty name. Please provide a secret name.", nameof(secretName));

        
        var secretPath = $"/run/secrets/{secretName}";
        if (!File.Exists(secretPath)) throw new Exception($"Secret was not found. {logSafeSecretName}");

        return File.ReadAllText(secretPath);
    }
}