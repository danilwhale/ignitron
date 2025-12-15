using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace Ignitron.Aluminium.Registries;

/// <summary>
/// Represents a bidirectional collection of unique keys and values
/// </summary>
/// <typeparam name="TValue">The type of values in the registry</typeparam>
public sealed class NamedRegistry<TValue> : IEnumerable<KeyValuePair<string, TValue>>
    where TValue : notnull
{
    private readonly Dictionary<string, TValue> _byName = [];
    private readonly Dictionary<TValue, string> _byValue = [];

    /// <summary>
    /// Occurs when a new unique entry has been added to the registry
    /// </summary>
    public event Action<NamedRegistry<TValue>, string, TValue>? Registered;

    /// <summary>
    /// Gets the value associated with the specified name
    /// </summary>
    /// <param name="key">The name of the value to get</param>
    /// <param name="value">When this method returns, contains the value associated with the specified name, if the name is found; otherwise, the default value for the type of the value parameter</param>
    /// <returns>true if the <see cref="NamedRegistry{TValue}"/> contains an entry with the specified name; otherwise, false</returns>
    public bool TryGetValue(string key, [NotNullWhen(true)] out TValue? value)
    {
        return _byName.TryGetValue(key.ToLowerInvariant(), out value);
    }

    /// <summary>
    /// Gets the name associated with the specified value
    /// </summary>
    /// <param name="value">The value of the name to get</param>
    /// <param name="key">When this method returns, contains the name associated with the specified value, if the value is found; otherwise, null</param>
    /// <returns>true if the <see cref="NamedRegistry{TValue}"/> contains an entry with the specified value; otherwise, false</returns>
    public bool TryGetName(TValue value, [NotNullWhen(true)] out string? key)
    {
        return _byValue.TryGetValue(value, out key);
    }

    /// <summary>
    /// Adds the specified unique key and value to the registry
    /// </summary>
    /// <param name="key">The unique key of the entry to add</param>
    /// <param name="value">The unique value of the entry to add</param>
    public void Register(string key, TValue value)
    {
        key = key.ToLowerInvariant();
        _byName.Add(key, value);
        _byValue.Add(value, key);
        Registered?.Invoke(this, key, value);
    }

    /// <summary>
    /// Adds the specified unique key and value to the registry
    /// </summary>
    /// <param name="key">The unique key of the entry to add</param>
    /// <param name="value">The unique value of the entry to add</param>
    /// <returns>true if the entry was added to the registry successfully; otherwise, false</returns>
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