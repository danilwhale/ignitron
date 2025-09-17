namespace ContentAPI;

public readonly record struct Icon(int X, int Y, int Width, int Height)
{
    public readonly int X = X;
    public readonly int Y = Y;
    public readonly int Width = Width;
    public readonly int Height = Height;

    public ushort SpriteX => (ushort)(X / 16);
    public ushort SpriteY => (ushort)(Y / 16);
}