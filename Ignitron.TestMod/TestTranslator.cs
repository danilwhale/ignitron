using Ignitron.Aluminium.Assets;
using Ignitron.Aluminium.Assets.Providers;
using Ignitron.Aluminium.Translation;

namespace Ignitron.TestMod;

internal sealed class TestTranslator(AssetManager ass) : ITranslator
{
    public void Load(Dictionary<string, string> destination, string locale)
    {
        ass
            .Load(Path.Join(AssetManager.TranslationsDirectory, $"{locale}.txt"), TranslationAssetProvider.Default)
            .Load(destination);
    }

    public void Translate()
    {
    }
}