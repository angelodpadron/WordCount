using System.Text.RegularExpressions;
using UglyToad.PdfPig.Content;
using UglyToad.PdfPig;

namespace WordCount.Parsers;
public partial class PDFParser : ITextParser
{

    [GeneratedRegex(@"\w+", RegexOptions.Compiled)]
    private static partial Regex SplitRegex();

    private readonly string ValidExtension = ".pdf";


    public void CheckIfCanHandle(string source)
    {
        var extension = Path.GetExtension(source).ToLower();
        if (extension != ValidExtension) throw new ArgumentException($"Invalid file extension: {ValidExtension} expected, but {extension} passed instead");
    }

    public List<string> GetTokens(string source)
    {

        CheckIfCanHandle(source);

        List<string> tokens = [];

        try
        {
            using (PdfDocument document = PdfDocument.Open(source))
            {
                foreach (Page page in document.GetPages())
                {
                    tokens
                        .AddRange(
                            SplitRegex()
                            .Matches(page.Text)
                            .Select(token => token.Value));
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error reading pdf file: {0}", ex.Message);
        }

        return tokens;

    }

}
