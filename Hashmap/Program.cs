using System.Text.RegularExpressions;

namespace WordCount;

internal partial class Program
{
    [GeneratedRegex(@"\w+")]
    private static partial Regex SplitRegex();

    [GeneratedRegex(@"\.txt")]
    private static partial Regex FileExtensionRegex();

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

            Console.WriteLine($"Top {top} most used words:\n");

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
        List<string> tokens = GetTokensFor(source);

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

    private static List<string> GetTokensFor(string source)
    {
        List<string> tokens = [];

        using (var reader = GetReaderFor(source))
        {
            while (!reader.EndOfStream)
            {
                tokens
                    .AddRange(
                        SplitRegex()
                            .Matches(reader.ReadLine()!)
                            .Select(match => match.Value)
                            .ToArray()
                    );

            }
        }

        return tokens;

    }

    static StreamReader GetReaderFor(string source)
    {

        string extension = Path.GetExtension(source)?.ToLower();

        return extension switch
        {
            ".txt" => new StreamReader(source),
            _ => throw new NotSupportedException($"File type {extension} is not supported."),
        };
    }

}

internal class WordInfo(string word, int count)
{
    internal string Word { get; set; } = word;
    internal int Count { get; set; } = count;
}

internal readonly struct KVP<TFirst, TSecond>(TFirst first, TSecond second)
{
    internal TFirst First { get; } = first;
    internal TSecond Second { get; } = second;
}

internal class Hashmap<TKey, TValue>
{
    private readonly int Capacity;
    private readonly List<KVP<TKey, TValue>>[] Buckets;

    internal Hashmap(int capacity)
    {
        Capacity = capacity;
        Buckets = new List<KVP<TKey, TValue>>[capacity];
    }

    internal int Hash(TKey key)
    {
        if (key is null) throw new ArgumentNullException(nameof(key));

        return Math.Abs(key.GetHashCode() % Capacity);
    }

    internal Hashmap<TKey, TValue> Add(TKey key, TValue value)
    {
        int index = Hash(key);

        if (Buckets[index] is not null)
        {
            var bucket = Buckets[index];
            bucket.Add(new KVP<TKey, TValue>(key, value));
            return this;
        }

        Buckets[index] = [new(key, value)];

        return this;
    }

    internal bool Has(TKey key)
    {
        int index = Hash(key);

        return Buckets[index] is not null && Buckets[index].Any(it => it.First!.Equals(key));
    }

    internal TValue Get(TKey key)
    {
        int index = Hash(key);

        if (Buckets[index] is null) throw new ArgumentException($"No entry for key {key}");

        return Buckets[index].Find(pair => pair.First!.Equals(key)).Second;

    }

    internal TValue[] Values()
    {
        return Buckets
            .Where(item => item is not null)
            .SelectMany(item => item.Select(i => i.Second))
            .ToArray();

    }

    internal TKey[] Keys()
    {
        return Buckets
            .Where(item => item is not null)
            .SelectMany(item => item.Select(i => i.First))
            .ToArray();
    }
}