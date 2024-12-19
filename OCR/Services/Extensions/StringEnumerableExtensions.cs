namespace OCR.Services.Extensions;

public static class StringEnumerableExtensions
{
    public static IEnumerable<string> SplitBySpace(this IEnumerable<string> lines)
    {
        return lines.SelectMany(line => line.Split(' '));
    }

    public static IEnumerable<string> FilterSpecialCharacters(this IEnumerable<string> words)
    {
        return words
            .Select(word => new string(word.Where(char.IsLetterOrDigit).ToArray()))
            .Where(filteredWord => !string.IsNullOrEmpty(filteredWord));
    }
}
