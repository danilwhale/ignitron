using System.Diagnostics.CodeAnalysis;
using Ignitron.Aluminium.Assets.Descriptors;

namespace Ignitron.Aluminium.Assets.Providers;

public interface IAssetProvider<out TAsset, in TDescriptor>
    where TDescriptor : IAssetDescriptor<TAsset>
{
    [return: NotNull]
    TAsset Create(string assetName, string rootPath, TDescriptor descriptor);
}