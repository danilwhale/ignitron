using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace Ignitron.Aluminium.Registries;

public sealed class NamedRegistry<TValue> : IEnumerable<KeyValuePair<string, TValue>>
    where TValue : notnull
{
    private readonly Dictionary<string, TValue> _byName = [];
    private readonly Dictionary<TValue, string> _byValue = [];

    public event Action<NamedRegistry<TValue>, string, TValue>? Registered;

    public bool TryGetValue(string key, [NotNullWhen(true)] out TValue? value)
    {
        return _byName.TryGetValue(key.ToLowerInvariant(), out value);
    }

    public bool TryGetName(TValue value, [NotNullWhen(true)] out string? key)
    {
        return _byValue.TryGetValue(value, out key);
    }

    public void Register(string key, TValue value)
    {
        key = key.ToLowerInvariant();
        _byName.Add(key, value);
        _byValue.Add(value, key);
        Registered?.Invoke(this, key, value);
    }

    public bool TryRegister(string key, TValue value)
    {
        key = key.ToLowerInvariant();
        if (_byName.TryAdd(key, value) && _byValue.TryAdd(value, key))
        {
            Registered?.Invoke(this, key, value);
            return true;
        }

        return false;
    }

    public TValue this[string key] => _byName[key.ToLowerInvariant()];

    public IEnumerator<KeyValuePair<string, TValue>> GetEnumerator()
    {
        return _byName.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}