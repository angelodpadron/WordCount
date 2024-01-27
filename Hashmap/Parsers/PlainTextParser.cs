using System.Text.RegularExpressions;

namespace WordCount.Parsers;

public partial class PlainTextParser : ITextParser
{
    private readonly string ValidExtension = ".txt";

    [GeneratedRegex(@"\w+")]
    private static partial Regex SplitRegex();

    public void CheckIfCanHandle(string source)
    {
        var extension = Path.GetExtension(source).ToLower();
        if (extension != ValidExtension) throw new ArgumentException($"{ValidExtension} expected, but {extension} passed instead");
    }

    public List<string> GetTokens(string source)
    {

        CheckIfCanHandle(source);

        List<string> tokens = [];

        using (var reader = new StreamReader(source))
        {
            while (!reader.EndOfStream)
            {
                tokens
                    .AddRange(
                        SplitRegex()
                            .Matches(reader.ReadLine()!)
                            .Select(match => match.Value)
                    );

            }
        }

        return tokens;
    }


}
