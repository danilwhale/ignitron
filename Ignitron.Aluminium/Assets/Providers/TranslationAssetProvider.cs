using System.Buffers;
using System.Collections.Frozen;
using System.Text;
using Allumeria;
using Ignitron.Aluminium.Translation;

namespace Ignitron.Aluminium.Assets.Providers;

public sealed class TranslationAssetProvider : IAssetProvider<TranslationBox>
{
    public static TranslationAssetProvider Default { get; } = new();

    public TranslationBox Create(AssetManager assets, string assetName)
    {
        ReadOnlySpan<char> translation;
        using (Stream stream = assets.Open(assetName))
        using (StreamReader reader = new(stream, Encoding.UTF8))
        {
            translation = reader.ReadToEnd();
        }

        KeyValuePair<string, string>[] keys = new KeyValuePair<string, string>[translation.Count('\n') + 1];

        int i = 0;
        foreach (ReadOnlySpan<char> line in translation.EnumerateLines())
        {
            if (line.IsEmpty || line.IsWhiteSpace()) continue;

            int splitIndex = line.IndexOf(' ');
            if (splitIndex < 0)
            {
                Logger.Warn($"Invalid translation key at line {i}! Expected value in format '<key> <translation>', got {line}");
                continue;
            }

            keys[i++] = new KeyValuePair<string, string>(new string(line[..splitIndex]), new string(line[(splitIndex + 1)..]));
        }

        return new TranslationBox(keys.ToFrozenDictionary()); // TODO: use FrozenDictionary.Create in 0.12
    }
}