using System.Collections;

namespace Ignitron.Aluminium.Registries;

public sealed class NamedRegistry<TValue> : IEnumerable<KeyValuePair<string, TValue>>
{
    private readonly Dictionary<string, TValue> _values = [];

    public event Action<string, TValue>? Registered;

    public bool TryGetValue(string key, out TValue? value)
    {
        return _values.TryGetValue(key.ToLowerInvariant(), out value);
    }

    public void Register(string key, TValue value)
    {
        key = key.ToLowerInvariant();
        _values.Add(key, value);
        Registered?.Invoke(key, value);
    }

    public bool TryRegister(string key, TValue value)
    {
        key = key.ToLowerInvariant();
        if (_values.TryAdd(key, value))
        {
            Registered?.Invoke(key, value);
            return true;
        }

        return false;
    }

    public TValue this[string key] => _values[key.ToLowerInvariant()];

    public IEnumerator<KeyValuePair<string, TValue>> GetEnumerator()
    {
        return _values.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}