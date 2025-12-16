using System.IO.Compression;
using Ignitron.Aluminium.Assets.Descriptors;
using Ignitron.Aluminium.Assets.Providers;

namespace Ignitron.Aluminium.Assets;

/// <summary>
/// Represents a manager to lazily load assets from the directory
/// </summary>
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

    private readonly Dictionary<AssetKey, object> _assets = [];
    private ZipArchive? _archive;
    private string _rootPath = null!;

    /// <summary>
    /// Gets a value that indicates whether the <see cref="AssetManager"/> is wrapped around an archive file
    /// </summary>
    /// <value>true if the <see cref="AssetManager"/> is wrapped around an archive file; otherwise, false</value>
    public bool IsVirtual => _archive != null;

    /// <summary>
    /// Gets the root directory path used to load the assets either from the filesystem or an archive file
    /// </summary>
    /// <remarks>
    /// You can specify the directory inside an archive file by appending it to the path of the wanted archive:
    /// <code>Path.Join(box.RootPath, AssetManager.RootDirectory)</code>
    /// <code>.../Mod.zip/res</code>
    /// </remarks>
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

    /// <summary>
    /// Initialises a new instance of the <see cref="AssetManager"/> class that has unspecified root path
    /// </summary>
    /// <remarks>
    /// Use <see cref="AssetManager(string)"/> to create with the path you want or manually set <see cref="RootPath"/> later.
    /// Trying to load an asset without specified <see cref="RootPath"/> will throw an exception
    /// </remarks>
    public AssetManager()
        : this(string.Empty)
    {
    }

    /// <summary>
    /// Initialises a new instance of the <see cref="AssetManager"/> class with the specified value for <see cref="RootPath"/>
    /// </summary>
    /// <param name="rootPath">Path to the root directory of the assets</param>
    public AssetManager(string rootPath)
    {
        RootPath = rootPath;
    }

    /// <summary>
    /// Opens the stream for the asset located at the <paramref name="assetPath"/>
    /// </summary>
    /// <param name="assetPath">Path of the requested asset relative to the <see cref="RootPath"/></param>
    /// <returns>A stream for the requested asset. The stream must be disposed manually</returns>
    /// <exception cref="InvalidOperationException">The <see cref="RootPath"/> isn't set to any value or is empty</exception>
    /// <exception cref="FileNotFoundException">The requested asset wasn't found in the <see cref="RootPath"/> directory</exception>
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

    /// <summary>
    /// Lazily loads the asset located at the <paramref name="assetName"/>
    /// </summary>
    /// <param name="assetName">Path of the requested asset relative to the <see cref="RootPath"/></param>
    /// <param name="descriptor">The description for the requested asset as required by the <paramref name="provider"/></param>
    /// <param name="provider">The provider of the asset that will load and deserialize data of the asset using the <paramref name="descriptor"/></param>
    /// <typeparam name="T">Type of asset to load</typeparam>
    /// <typeparam name="TDescriptor">Type of the descriptor object used by the provider</typeparam>
    /// <returns>The asset that was either previously or freshly deserialized and loaded by the <paramref name="provider"/></returns>
    /// <exception cref="InvalidOperationException">The <see cref="RootPath"/> isn't set to any value or is empty</exception>
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

    /// <summary>
    /// Lazily loads the asset located at the <paramref name="assetName"/>
    /// </summary>
    /// <param name="assetName">Path of the requested asset relative to the <see cref="RootPath"/></param>
    /// <param name="provider">The provider of the asset that will load and deserialize data of the asset</param>
    /// <typeparam name="T">Type of asset to load</typeparam>
    /// <returns>The asset that was either previously or freshly deserialized and loaded by the <paramref name="provider"/></returns>
    /// <exception cref="InvalidOperationException">The <see cref="RootPath"/> isn't set to any value or is empty</exception>
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