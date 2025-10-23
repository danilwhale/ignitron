using Allumeria;
using Allumeria.Rendering;
using Ignitron.Aluminium.Assets.Descriptors;
using OpenTK.Graphics.OpenGL;
using StbImageSharp;

namespace Ignitron.Aluminium.Assets.Providers;

public sealed class TextureAssetProvider : IAssetProvider<Texture, TextureAssetDescriptor>
{
    public static TextureAssetProvider Default { get; } = new();

    public Texture Create(AssetManager assets, string assetName, TextureAssetDescriptor descriptor)
    {
        Texture texture = new(GL.GenTexture());
        texture.Use();

        if (descriptor.Flip) StbImage.stbi_set_flip_vertically_on_load(1);
        else StbImage.stbi_set_flip_vertically_on_load(0);

        // why do i have to do this
        ImageResult image;

        using (Stream stream = assets.Open(assetName))
        using (MemoryStream ms = new())
        {
            stream.CopyTo(ms);
            ms.Position = 0;

            image = ImageResult.FromStream(ms, ColorComponents.RedGreenBlueAlpha);
            Logger.Verbose("Image loaded successfully");
        }

        if (image != null)
        {
            texture.size = image.Width;
            if (descriptor.KeepImage)
            {
                texture.sourceImage = image;
            }

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, image.Width, image.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, image.Data);
            if (descriptor.Clamp)
            {
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
            }
            else
            {
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            }

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.NearestMipmapNearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMaxLod, descriptor.Mipmaps ? 1 : 0);
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
        }

        return texture;
    }
}