using System.IO.Compression;
using Ignitron.Aluminium.Assets.Descriptors;
using Ignitron.Aluminium.Assets.Providers;
using Ignitron.Loader;

namespace Ignitron.Aluminium.Assets;

public sealed class AssetManager : IDisposable
{
    private readonly record struct AssetKey(Type Type, string Path);

    public const string RootDirectory = "res";

    // some common subdirectories for assets
    public const string ModelsDirectory = "models";
    public const string ShadersDirectory = "shaders";
    public const string SoundsDirectory = "sounds";
    public const string StructuresDirectory = "structures";
    public const string TexturesDirectory = "textures";
    public const string TranslationsDirectory = "translations";

    private ZipArchive? _archive;
    private string _rootPath;

    public bool IsVirtual => _archive != null;

    public string RootPath
    {
        get => _rootPath;
        set
        {
            _archive?.Dispose();

            if (value.Contains(".zip", StringComparison.OrdinalIgnoreCase))
            {
                // trim until we hit .zip
                ReadOnlySpan<char> zipPath = value;
                while (!(zipPath = Path.GetDirectoryName(zipPath)).EndsWith(".zip")) ;

                // load archive
                _archive = ZipFile.OpenRead(new string(zipPath));

                // trim zip path from root path
                value = value[zipPath.Length..].TrimStart('/', '\\' /* we trim all path chars because you can never trust the user */);
            }
            else _archive = null;

            _rootPath = value;
        }
    }

    private readonly Dictionary<AssetKey, object> _assets = [];

    public AssetManager()
        : this(string.Empty)
    {
    }

    public AssetManager(ModBox mod)
        : this(Path.Join(mod.RootPath, RootDirectory))
    {
    }

    public AssetManager(string rootPath)
    {
        RootPath = rootPath;
    }

    public Stream Open(string assetPath)
    {
        if (string.IsNullOrWhiteSpace(RootPath))
        {
            throw new InvalidOperationException($"{nameof(RootPath)} isn't set or is empty");
        }

        if (_archive != null)
        {
            // open stream from an archive
            return _archive.GetEntry(Path.Join(RootPath, assetPath).Replace(Path.DirectorySeparatorChar, '/'))?.Open()
                   ?? throw new FileNotFoundException($"Asset '{assetPath}' wasn't found", assetPath);
        }
        else
        {
            // open stream from file system
            return File.OpenRead(Path.Join(RootPath, assetPath));
        }
    }

    public T Load<T, TDescriptor>(string assetName, TDescriptor descriptor, IAssetProvider<T, TDescriptor> provider)
        where TDescriptor : IAssetDescriptor<T>
    {
        if (string.IsNullOrWhiteSpace(RootPath))
        {
            throw new InvalidOperationException($"{nameof(RootPath)} isn't set or is empty");
        }

        ArgumentException.ThrowIfNullOrWhiteSpace(assetName);
        AssetKey key = new(typeof(T), assetName);
        if (_assets.TryGetValue(key, out object? cache))
        {
            return (T)cache;
        }

        T asset = provider.Create(this, assetName, descriptor);
        _assets[key] = asset;
        return asset;
    }

    public T Load<T>(string assetName, IAssetProvider<T> provider)
    {
        if (string.IsNullOrWhiteSpace(RootPath))
        {
            throw new InvalidOperationException($"{nameof(RootPath)} isn't set or is empty");
        }

        ArgumentException.ThrowIfNullOrWhiteSpace(assetName);
        AssetKey key = new(typeof(T), assetName);
        if (_assets.TryGetValue(key, out object? cache))
        {
            return (T)cache;
        }

        T asset = provider.Create(this, assetName);
        _assets[key] = asset;
        return asset;
    }

    public void Dispose()
    {
        _archive?.Dispose();
    }
}