namespace Ocr.Api.Utilities;

public static class DockerSecretUtil
{
    public static string GetSecret(string secretName)
    {
        if (string.IsNullOrEmpty(secretName))
            throw new ArgumentException("Attempted to get secret with empty name. Please provide a secret name.");

        var secretPath = $"/run/secrets/{secretName}";
        if (!File.Exists(secretPath)) throw new Exception("Secret was not found.");

        return File.ReadAllText(secretPath);
    }
}