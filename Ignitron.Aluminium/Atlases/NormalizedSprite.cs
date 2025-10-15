using System.Numerics;

namespace Ignitron.Aluminium.Atlases;

public readonly record struct NormalizedSprite(float U0, float V0, float U1, float V1)
{
    public readonly float U0 = U0;
    public readonly float V0 = V0;
    public readonly float U1 = U1;
    public readonly float V1 = V1;

    public float Width => U1 - U0;
    public float Height => V1 - V0;

    public Vector2 Min => new(U0, V0);
    public Vector2 Max => new(U1, V1);

    public Vector4 AsVector4() => new(U0, V0, U1, V1);
}