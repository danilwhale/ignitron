using Ignitron.Aluminium.Assets.Descriptors;
using Ignitron.Aluminium.Assets.Providers;
using Ignitron.Loader.API;

namespace Ignitron.Aluminium.Assets;

public sealed class AssetManager(string rootPath)
{
    public const string RootDirectory = "res";

    // some common subdirectories for assets
    public const string ModelsDirectory = "models";
    public const string ShadersDirectory = "shaders";
    public const string SoundsDirectory = "sounds";
    public const string StructuresDirectory = "structures";
    public const string TexturesDirectory = "textures";
    public const string TranslationsDirectory = "translations";

    public string RootPath { get; set; } = rootPath;

    private readonly Dictionary<string, object> _assets = [];

    public AssetManager()
        : this(string.Empty)
    {
    }

    public AssetManager(Mod mod)
        : this(Path.Join(mod.RuntimeData.RootPath, RootDirectory))
    {
    }

    public T Load<T, TDescriptor>(string assetName, TDescriptor descriptor, IAssetProvider<T, TDescriptor> provider)
        where TDescriptor : IAssetDescriptor<T>
    {
        if (string.IsNullOrWhiteSpace(RootPath))
        {
            throw new InvalidOperationException($"{nameof(RootPath)} isn't set or is empty");
        }

        ArgumentException.ThrowIfNullOrWhiteSpace(assetName);
        if (_assets.TryGetValue(assetName, out object? cache))
        {
            return (T)cache;
        }

        T asset = provider.Create(assetName, RootPath, descriptor);
        _assets[assetName] = asset;
        return asset;
    }

    public T Load<T>(string assetName, IAssetProvider<T> provider)
    {
        if (string.IsNullOrWhiteSpace(RootPath))
        {
            throw new InvalidOperationException($"{nameof(RootPath)} isn't set or is empty");
        }

        ArgumentException.ThrowIfNullOrWhiteSpace(assetName);
        if (_assets.TryGetValue(assetName, out object? cache))
        {
            return (T)cache;
        }

        T asset = provider.Create(assetName, RootPath);
        _assets[assetName] = asset;
        return asset;
    }
}