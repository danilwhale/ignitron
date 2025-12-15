using System.Numerics;

namespace Ignitron.Aluminium.Atlases;

/// <summary>
/// Stores normalized bounds of the sprite
/// </summary>
/// <param name="U0">Minimum U coordinate</param>
/// <param name="V0">Minimum V coordinate</param>
/// <param name="U1">Maximum U coordinate</param>
/// <param name="V1">Maximum V coordinate</param>
public readonly record struct NormalizedSprite(float U0, float V0, float U1, float V1)
{
    /// <summary>
    /// Minimum U coordinate of the sprite
    /// </summary>
    public readonly float U0 = U0;

    /// <summary>
    /// Minimum V coordinate of the sprite
    /// </summary>
    public readonly float V0 = V0;

    /// <summary>
    /// Maximum U coordinate of the sprite
    /// </summary>
    public readonly float U1 = U1;

    /// <summary>
    /// Maximum V coordinate of the sprite
    /// </summary>
    public readonly float V1 = V1;

    /// <summary>
    /// Width of the sprite
    /// </summary>
    public float Width => U1 - U0;
    
    /// <summary>
    /// Height of the sprite
    /// </summary>
    public float Height => V1 - V0;

    /// <summary>
    /// Minimum UV coordinates of the sprite
    /// </summary>
    public Vector2 Min => new(U0, V0);
    
    /// <summary>
    /// Maximum UV coordinates of the sprite
    /// </summary>
    public Vector2 Max => new(U1, V1);

    /// <summary>
    /// Creates a new instance of the <see cref="Vector4"/> structure that stores UV bounds of the sprite
    /// </summary>
    public Vector4 AsVector4() => new(U0, V0, U1, V1);
}