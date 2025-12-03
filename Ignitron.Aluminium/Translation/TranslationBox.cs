using System.Collections.Frozen;

namespace Ignitron.Aluminium.Translation;

public sealed class TranslationBox(FrozenDictionary<string, string> keys)
{
    public readonly FrozenDictionary<string, string> Keys = keys;

    public void Load(Dictionary<string, string> destination)
    {
        foreach ((string key, string translation) in Keys)
        {
            destination.Add(key, translation);
        }
    }

    public bool TryLoad(Dictionary<string, string> destination)
    {
        foreach ((string key, string translation) in Keys)
        {
            if (!destination.TryAdd(key, translation)) return false;
        }

        return true;
    }
}