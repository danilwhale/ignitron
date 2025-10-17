using SoLoud;

namespace Ignitron.Aluminium.Assets.Providers;

public sealed class SoundAssetProvider : IAssetProvider<Wav>
{
    public static SoundAssetProvider Default { get; } = new();
    
    public Wav Create(string assetName, string rootPath)
    {
        Wav wav = new();
        wav.load(Path.Join(rootPath, assetName));
        return wav;
    }
}