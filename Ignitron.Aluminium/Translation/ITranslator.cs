namespace Ignitron.Aluminium.Translation;

public interface ITranslator
{
    void Load(Dictionary<string, string> destination, string locale);
    void Translate();
}