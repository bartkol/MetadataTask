namespace FivetranClient.Infrastructure;

/// <summary>
/// A thread-safe dictionary that supports time-to-live (TTL) for its entries.
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TValue"></typeparam>
public class TtlDictionary<TKey, TValue> where TKey : notnull
{
    private readonly Dictionary<TKey, (TValue, DateTime)> _dictionary = new();
    private readonly object _lock = new();

    /// <summary>
    /// Adds a value to the dictionary with a specified TTL.
    /// If the key already exists, it returns the existing value.
    /// If the key does not exist, it uses the provided factory to create a new value and adds it to the dictionary with the specified TTL.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="valueFactory"></param>
    /// <param name="ttl"></param>
    /// <returns></returns>
    public TValue GetOrAdd(TKey key, Func<TValue> valueFactory, TimeSpan ttl)
    {
        lock (_lock)
        {
            if (TryGetValueInternal(key, out var existingValue))
            {
                return existingValue;
            }

            var value = valueFactory();
            _dictionary[key] = (value, DateTime.UtcNow.Add(ttl));
            return value;
        }
    }

    /// <summary>
    /// Attempts to get the value associated with the specified key.
    /// If the key exists and has not expired, it returns true and outputs the value.
    /// If the key does not exist or has expired, it returns false and outputs the default.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <returns>True if the key exists, false if doesn't.</returns>
    public bool TryGetValue(TKey key, out TValue value)
    {
        lock (_lock)
        {
            return TryGetValueInternal(key, out value);
        }
    }

    private bool TryGetValueInternal(TKey key, out TValue value)
    {
        if (_dictionary.TryGetValue(key, out var entry))
        {
            if (DateTime.UtcNow < entry.Item2)
            {
                value = entry.Item1;
                return true;
            }

            _dictionary.Remove(key);
        }

        value = default!;
        return false;
    }
}