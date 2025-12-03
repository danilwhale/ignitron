using Ignitron.Aluminium.Translation;

namespace Ignitron.Aluminium.Registries;

public static class AluminiumRegistries
{
    public static readonly NamedRegistry<ITranslator> Translators = new();
}