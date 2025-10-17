using Allumeria.Rendering;
using Ignitron.Aluminium.Assets.Descriptors;

namespace Ignitron.Aluminium.Assets.Providers;

public sealed class ShaderAssetProvider : IAssetProvider<Shader, ShaderAssetDescriptor>
{
    public static ShaderAssetProvider Default { get; } = new();
    
    public Shader Create(string assetName, string rootPath, ShaderAssetDescriptor descriptor)
    {
        string name = Path.GetFileNameWithoutExtension(assetName);
        return new Shader(
            Path.Join(rootPath, descriptor.VertexAssetName ?? name + ".vert"),
            Path.Join(rootPath, descriptor.FragmentAssetName ?? name + ".frag"));
    }
}