using System.Diagnostics.CodeAnalysis;

namespace Ignitron.Aluminium.Assets.Providers;

public interface IAssetProvider<TAsset>
{
    [return: NotNull]
    TAsset Create(string assetName, string rootPath);
}