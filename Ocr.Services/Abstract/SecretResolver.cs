namespace Ocr.Services.Abstract;

/// <summary>
/// Decrypts and returns the secret value from the secret store.
/// This implementation should never return a null or empty string.
///
/// In this app, it will be used to handle either Docker or Environment secrets.
/// </summary>
/// <param name="secretName"></param>
/// <returns></returns>
public delegate string ResolveSecret(string secretName);
