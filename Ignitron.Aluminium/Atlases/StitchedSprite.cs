namespace Ignitron.Aluminium.Atlases;

public readonly struct StitchedSprite(ushort u0, ushort v0, ushort u1, ushort v1)
{
    public readonly ushort U0 = u0;
    public readonly ushort V0 = v0;
    public readonly ushort U1 = u1;
    public readonly ushort V1 = v1;

    public ushort Width => (ushort)(U1 - U0);
    public ushort Height => (ushort)(V1 - V0);
}