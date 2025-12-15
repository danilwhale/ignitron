using System.Diagnostics.CodeAnalysis;
using Ignitron.Aluminium.Assets.Descriptors;

namespace Ignitron.Aluminium.Assets.Providers;

/// <summary>
/// Represents the provider of the <typeparamref name="TAsset"/> assets described by the <typeparamref name="TDescriptor"/>
/// </summary>
/// <typeparam name="TAsset">Type of the asset this <see cref="IAssetProvider{TAsset,TDescriptor}"/> is providing</typeparam>
/// <typeparam name="TDescriptor">Type of the description this <see cref="IAssetProvider{TAsset,TDescriptor}"/> uses to provide the asset</typeparam>
public interface IAssetProvider<out TAsset, in TDescriptor>
    where TDescriptor : IAssetDescriptor<TAsset>
{
    /// <summary>
    /// Creates a new instance of the <typeparamref name="TAsset"/> from the specified <paramref name="assetName"/> described by the <paramref name="descriptor"/>
    /// </summary>
    /// <param name="assets">Instance of the <see cref="AssetManager"/> class this provider was used in</param>
    /// <param name="assetName">Path to the requested asset relative to the <see cref="AssetManager.RootPath"/></param>
    /// <param name="descriptor">The description for the requested asset</param>
    /// <returns>The asset from the specified <paramref name="assetName"/> described by the <paramref name="descriptor"/></returns>
    [return: NotNull]
    TAsset Create(AssetManager assets, string assetName, TDescriptor descriptor);
}