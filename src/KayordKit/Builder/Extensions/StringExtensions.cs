namespace KayordKit.Builder.Extensions;

public static class StringExtensions
{
    public static string Repeat(this string value, int quantity)
    {
        return new System.Text.StringBuilder().Insert(0, value, quantity).ToString();
    }

    public static string CapitalizeFirstLetter(this string input) =>
        string.IsNullOrEmpty(input)
            ? ""
            : input.First().ToString().ToUpper() + input[1..];

    public static string TrimLastCharacters(this string str) => string.IsNullOrEmpty(str) ? str : str.TrimEnd(str[^1]);

    public static string RemoveFirstCharacter(this string str) => string.IsNullOrEmpty(str) ? str : str[1..];

    public static string RemoveLastCharacter(this string str) => string.IsNullOrEmpty(str) ? str : str[..^1];

    public static string RemovePluralization(this string input) =>
        !string.IsNullOrEmpty(input) && input.EndsWith("s", StringComparison.InvariantCultureIgnoreCase) ? input.RemoveLastCharacter() : input;

    public static string TabsToSpaces(this string value, int tabLength = 4)
    {
        var tabSpace = new string(' ', tabLength);
        return value.Replace("\t", tabSpace);
    }
}

