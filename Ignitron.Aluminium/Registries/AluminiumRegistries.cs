using Ignitron.Aluminium.Translation;

namespace Ignitron.Aluminium.Registries;

/// <summary>
/// Collection of <see cref="NamedRegistry{TValue}"/> that are used across entire lifetime of the game
/// </summary>
public static class AluminiumRegistries
{
    /// <summary>
    /// <see cref="NamedRegistry{TValue}"/> containing the unique translators that are used by the Aluminium API to translate the game
    /// </summary>
    public static readonly NamedRegistry<ITranslator> Translators = new();
}