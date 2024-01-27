namespace WordCount.Structures;

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
