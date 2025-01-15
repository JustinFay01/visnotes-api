namespace Ocr.Shared.Utilities;

public static class DockerSecretUtil
{
    public static string GetSecret(string secretPath, string logSafeSecretName)
    {
        if (string.IsNullOrEmpty(secretPath))
            throw new ArgumentException("Attempted to get secret with empty name. Please provide a secret name.", nameof(logSafeSecretName));
        
        if (!File.Exists(secretPath)) throw new Exception($"Secret was not found. {logSafeSecretName}");

        return File.ReadAllText(secretPath);
    }
}