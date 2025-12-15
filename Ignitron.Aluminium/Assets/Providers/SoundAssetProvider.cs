using SoLoud;

namespace Ignitron.Aluminium.Assets.Providers;

public sealed unsafe class SoundAssetProvider : IAssetProvider<Wav>
{
    public static SoundAssetProvider Default { get; } = new();

    public Wav Create(AssetManager assets, string assetName)
    {
        using Stream stream = assets.Open(assetName);
        using MemoryStream ms = new();
        stream.CopyTo(ms);

        Wav wav = new();
        fixed (byte* pBuffer = ms.GetBuffer())
        {
            // let's pray soloud won't use the pointer outside this point
            // i really don't want to allocate and copy ANOTHER time
            wav.loadMem((nint)pBuffer, (uint)ms.Length, aTakeOwnership: 0);
        }
        return wav;
    }
}