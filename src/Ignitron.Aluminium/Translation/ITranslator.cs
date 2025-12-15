namespace Ignitron.Aluminium.Translation;

/// <summary>
/// Defines methods to translate the modded content
/// </summary>
/// <remarks>
/// The defined methods are invoked when running the <see cref="Allumeria.DataManagement.Translation.Translator.LoadTranslation"/> method.
/// That is, the methods are invoked during the game load and the translation menu
/// </remarks>
public interface ITranslator
{
    /// <summary>
    /// Loads language strings for specified locale into the storage
    /// </summary>
    /// <param name="destination">The storage of language strings</param>
    /// <param name="locale">Locale to load strings for</param>
    void Load(Dictionary<string, string> destination, string locale);
    
    /// <summary>
    /// Translates strings from previously loaded locale's translation strings 
    /// </summary>
    void Translate();
}