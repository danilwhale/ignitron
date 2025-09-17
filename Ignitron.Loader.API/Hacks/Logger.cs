using System.Reflection;

namespace Ignitron.Loader.API.Hacks;

public static class Logger
{
    private static readonly Type RealType = ModLoader.Allumeria.GetType("Allumeria.Logger") ?? throw new InvalidOperationException();
    private static readonly MethodInfo RealInit = RealType.GetMethod("Init", BindingFlags.Public | BindingFlags.Static) ?? throw new InvalidOperationException();
    private static readonly MethodInfo RealError = RealType.GetMethod("Error", BindingFlags.Public | BindingFlags.Static) ?? throw new InvalidOperationException();

    public static void Init(string message) => RealInit.Invoke(null, [message]);
    public static void Error(string message) => RealError.Invoke(null, [message]);
}