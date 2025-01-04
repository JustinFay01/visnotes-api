namespace Ocr.Data.Utilities;

public class DockerSecretUtil
{
    public static string GetSecret(string secretName)
    {
        if (string.IsNullOrEmpty(secretName))
            throw new ArgumentException("Attempted to get secret with empty name. Please provide a secret name.");

        var secretPath = $"/run/secrets/{secretName}";
        if (!File.Exists(secretPath)) throw new Exception($"Secret {secretName} not found at {secretPath}");

        return File.ReadAllText(secretPath);
    }
}