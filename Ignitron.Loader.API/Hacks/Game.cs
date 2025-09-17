using System.Reflection;

namespace Ignitron.Loader.API.Hacks;

public sealed class Game
{
    private static readonly Type RealType = ModLoader.Allumeria.GetType("Allumeria.Game") ?? throw new InvalidOperationException();
    private static readonly FieldInfo RealVersion = RealType.GetField("VERSION", BindingFlags.Public | BindingFlags.Static) ?? throw new InvalidOperationException();

    public static string? VERSION
    {
        get => RealVersion.GetValue(null) as string;
        set => RealVersion.SetValue(null, value);
    }

    private Game() => throw new NotSupportedException();
}