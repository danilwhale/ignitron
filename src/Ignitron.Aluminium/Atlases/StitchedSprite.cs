namespace Ignitron.Aluminium.Atlases;

/// <summary>
/// Stores pixel bounds of the sprite that was stitched in the atlas
/// </summary>
/// <param name="U0">Minimum U coordinate</param>
/// <param name="V0">Minimum V coordinate</param>
/// <param name="U1">Maximum U coordinate</param>
/// <param name="V1">Maximum V coordinate</param>
public readonly record struct StitchedSprite(ushort U0, ushort V0, ushort U1, ushort V1)
{
    /// <summary>
    /// Minimum U coordinate of the sprite
    /// </summary>
    public readonly ushort U0 = U0;

    /// <summary>
    /// Minimum V coordinate of the sprite
    /// </summary>
    public readonly ushort V0 = V0;

    /// <summary>
    /// Maximum U coordinate of the sprite
    /// </summary>
    public readonly ushort U1 = U1;

    /// <summary>
    /// Maximum V coordinate of the sprite
    /// </summary>
    public readonly ushort V1 = V1;

    /// <summary>
    /// Width of the sprite
    /// </summary>
    public ushort Width => (ushort)(U1 - U0);
        
    /// <summary>
    /// Height of the sprite
    /// </summary>
    public ushort Height => (ushort)(V1 - V0);

    /// <summary>
    /// Normalizes the sprite bounds to [0;1] range using the given X and Y ratio
    /// </summary>
    /// <param name="ratioX">1:N sprite ratio of the X axis</param>
    /// <param name="ratioY">1:N sprite ratio of the Y axis</param>
    public NormalizedSprite Normalize(float ratioX, float ratioY)
    {
        return new NormalizedSprite(U0 * ratioX, V0 * ratioY, U1 * ratioX, V1 * ratioY);
    }

    /// <summary>
    /// Normalizes the sprite bounds to [0;1] range using the given X and Y size of the texture
    /// </summary>
    /// <param name="sizeX">Size of the texture on X axis (width)</param>
    /// <param name="sizeY">Size of the texture on Y axis (height)</param>
    public NormalizedSprite Normalize(int sizeX, int sizeY)
    {
        return Normalize(1.0f / sizeX, 1.0f / sizeY);
    }

    /// <summary>
    /// Normalizes the sprite bounds to [0;1] range using the given atlas' size
    /// </summary>
    /// <param name="atlas">The atlas to use size of</param>
    public NormalizedSprite Normalize(IAtlas atlas)
    {
        return Normalize(1.0f / atlas.Width, 1.0f / atlas.Height);
    }
}