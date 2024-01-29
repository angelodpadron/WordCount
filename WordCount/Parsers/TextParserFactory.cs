namespace WordCount.Parsers;

internal class TextParserFactory
{
    internal static ITextParser GetParser(string source)
    {

        string extension = Path.GetExtension(source).ToLower();

        return extension switch
        {
            ".txt" => new PlainTextParser(),
            ".pdf" => new PDFParser(),
            _ => throw new NotSupportedException($"File type {extension} is not supported."),
        };
    }
}
