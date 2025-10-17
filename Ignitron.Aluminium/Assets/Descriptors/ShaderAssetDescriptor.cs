using Allumeria.Rendering;

namespace Ignitron.Aluminium.Assets.Descriptors;

public readonly record struct ShaderAssetDescriptor(string? VertexAssetName, string? FragmentAssetName)
    : IAssetDescriptor<Shader>;