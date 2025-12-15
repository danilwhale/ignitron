using System.Collections.Frozen;

namespace Ignitron.Aluminium.Translation;

/// <summary>
/// Represents a container of translation strings and their corresponding values in unspecified locale
/// </summary>
/// <param name="keys">Translation strings with corresponding translated values</param>
public sealed class TranslationBox(FrozenDictionary<string, string> keys)
{
    /// <summary>
    /// Translation strings with corresponding translated values
    /// </summary>
    public readonly FrozenDictionary<string, string> Keys = keys;

    /// <summary>
    /// Loads translation strings and their values to the specified storage
    /// </summary>
    /// <param name="destination">The storage to load translation strings to</param>
    /// <exception cref="ArgumentException">One of the translation strings is already loaded</exception>
    public void Load(Dictionary<string, string> destination)
    {
        foreach ((string key, string translation) in Keys)
        {
            destination.Add(key, translation);
        }
    }

    /// <summary>
    /// Loads translation strings and their values to the specified storage
    /// </summary>
    /// <param name="destination">The storage to load translation strings to</param>
    /// <returns>true if all the translation strings and values were loaded successfully; otherwise, false</returns>
    public bool TryLoad(Dictionary<string, string> destination)
    {
        foreach ((string key, string translation) in Keys)
        {
            if (!destination.TryAdd(key, translation)) return false;
        }

        return true;
    }
}