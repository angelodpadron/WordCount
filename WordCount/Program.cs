using System.Text.RegularExpressions;
using WordCount.Parsers;
using WordCount.Structures;

namespace WordCount;

internal partial class Program
{
    [GeneratedRegex(@"^[1-9]\d*$")]
    private static partial Regex TopFormatRegex();

    static void Main(string[] args)
    {

        if (args.Length != 2 || !TopFormatRegex().Match(args[1]).Success)
        {
            string program = Path.GetFileNameWithoutExtension(Environment.GetCommandLineArgs()[0]);
            Console.WriteLine($"Usage: {program} <path_to_txt_file> <top>");

            return;
        }

        string filepath = args[0];
        int top = int.Parse(args[1]);

        try
        {
            Console.WriteLine($"\nAnalyzing {Path.GetFileName(filepath)}...\n");

            var wordsInfo = WordCount(filepath);

            Console.WriteLine($"{top} most used:\n");

            foreach (var wordInfo in wordsInfo.Take(top))
            {
                Console.WriteLine("- {0,-15} {1,2} ocurrences", wordInfo.Word, wordInfo.Count);
            }

            Console.WriteLine($"\nWords analyzed: {wordsInfo.Length}\n");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }


    }

    static WordInfo[] WordCount(string source)
    {
        List<string> tokens = TextParserFactory.GetParser(source).GetTokens(source);

        Hashmap<string, WordInfo> table = new(tokens.Distinct().Count());  //TODO: a more optimal way to calculate the required space

        foreach (var item in tokens)
        {
            var itemAsLowerCase = item.ToLower();

            if (table.Has(itemAsLowerCase))
            {
                table
                    .Get(itemAsLowerCase)
                    .Count += 1;

                continue;
            }

            table.Add(itemAsLowerCase, new(itemAsLowerCase, 1));

        }

        return [.. table.Values().OrderByDescending(item => item.Count)];

    }


}