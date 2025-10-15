namespace Ignitron.Aluminium.Atlases;

public readonly record struct StitchedSprite(ushort U0, ushort V0, ushort U1, ushort V1)
{
    public readonly ushort U0 = U0;
    public readonly ushort V0 = V0;
    public readonly ushort U1 = U1;
    public readonly ushort V1 = V1;

    public ushort Width => (ushort)(U1 - U0);
    public ushort Height => (ushort)(V1 - V0);

    public NormalizedSprite Normalize(float ratioX, float ratioY)
    {
        return new NormalizedSprite(U0 * ratioX, V0 * ratioY, U1 * ratioX, V1 * ratioY);
    }

    public NormalizedSprite Normalize(int sizeX, int sizeY)
    {
        return Normalize(1.0f / sizeX, 1.0f / sizeY);
    }

    public NormalizedSprite Normalize(IAtlas atlas)
    {
        return Normalize(1.0f / atlas.Width, 1.0f / atlas.Height);
    }
}