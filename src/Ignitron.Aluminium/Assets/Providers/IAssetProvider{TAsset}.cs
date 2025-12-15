using System.Diagnostics.CodeAnalysis;

namespace Ignitron.Aluminium.Assets.Providers;

/// <summary>
/// Represents the provider of the <typeparamref name="TAsset"/> assets
/// </summary>
/// <typeparam name="TAsset">Type of the asset this <see cref="IAssetProvider{TAsset}"/> is providing</typeparam>
public interface IAssetProvider<out TAsset>
{
    /// <summary>
    /// Creates a new instance of the <typeparamref name="TAsset"/> from the specified <paramref name="assetName"/>
    /// </summary>
    /// <param name="assets">Instance of the <see cref="AssetManager"/> class this provider was used in</param>
    /// <param name="assetName">Path to the requested asset relative to the <see cref="AssetManager.RootPath"/></param>
    /// <returns>The asset from the specified <paramref name="assetName"/></returns>
    [return: NotNull]
    TAsset Create(AssetManager assets, string assetName);
}