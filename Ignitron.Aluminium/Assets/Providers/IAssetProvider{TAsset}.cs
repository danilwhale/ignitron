using System.Diagnostics.CodeAnalysis;

namespace Ignitron.Aluminium.Assets.Providers;

public interface IAssetProvider<out TAsset>
{
    [return: NotNull]
    TAsset Create(AssetManager assets, string assetName);
}