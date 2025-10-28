using System.Reflection;
using System.Reflection.Emit;
using Allumeria;
using Allumeria.Rendering.Profiling;
using HarmonyLib;
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;

namespace Ignitron.Aluminium.Events;

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
[HarmonyPatch(typeof(Game))]
public static partial class AllumeriaEvents
{
    private static readonly FieldInfo GameThreadedLoadDone = AccessTools.DeclaredField(typeof(Game), nameof(Game.threadedLoadDone));
    private static readonly FieldInfo GameDeltaTime = AccessTools.DeclaredField(typeof(Game), nameof(Game.deltaTime));

    private static bool _wasThreadedLoadedDone;

    [HarmonyPrefix]
    [HarmonyPatch("OnLoad")]
    private static void ImplBeforeLoaded(Game __instance) => BeforeLoaded?.Invoke(__instance);

    [HarmonyPostfix]
    [HarmonyPatch("OnLoad")]
    private static void ImplLoaded(Game __instance) => Loaded?.Invoke(__instance);

    [HarmonyPrefix]
    [HarmonyPatch(nameof(Game.SetupGame))]
    private static void ImplBeforeLoadedThreaded(Game __instance) => BeforeLoadedThreaded?.Invoke(__instance);

    [HarmonyPostfix]
    [HarmonyPatch(nameof(Game.SetupGame))]
    private static void ImplLoadedThreaded(Game __instance) => LoadedThreaded?.Invoke(__instance);

    [HarmonyPrefix]
    [HarmonyPatch("OnUpdateFrame")]
    private static void ImplLoadedEverything(Game __instance, bool ___threadedLoadDone)
    {
        if (_wasThreadedLoadedDone) return;
        if (!_wasThreadedLoadedDone && ___threadedLoadDone)
        {
            LoadedEverything?.Invoke(__instance);
        }

        _wasThreadedLoadedDone = ___threadedLoadDone;
    }

    [HarmonyPrefix]
    [HarmonyPatch(nameof(Game.OnTick))]
    private static void ImplBeforeTicked(Game __instance) => BeforeTicked?.Invoke(__instance);

    [HarmonyPostfix]
    [HarmonyPatch(nameof(Game.OnTick))]
    private static void ImplTicked(Game __instance) => Ticked?.Invoke(__instance);

    [HarmonyTranspiler]
    [HarmonyPatch("OnUpdateFrame")]
    private static IEnumerable<CodeInstruction> ImplBeforeUpdated(IEnumerable<CodeInstruction> instructions)
    {
        return new CodeMatcher(instructions)
            .MatchStartForward(CodeMatch.Calls(() => default(Profiler)!.StartFrame()))
            .ThrowIfInvalid("couldn't find 'profiler.StartFrame()'")
            .InsertAfter(
                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Ldsfld, GameDeltaTime),
                CodeInstruction.Call(() => ImplBeforeUpdatedInvoke(default, default)))
            .Instructions();
    }

    private static void ImplBeforeUpdatedInvoke(Game game, double time) => BeforeUpdated?.Invoke(game, time);

    [HarmonyTranspiler]
    [HarmonyPatch("OnUpdateFrame")]
    private static IEnumerable<CodeInstruction> ImplLoadingUpdated(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
    {
        CodeMatcher cm = new CodeMatcher(instructions, generator)
            .MatchStartForward(
                new CodeMatch(OpCodes.Ldsfld, GameThreadedLoadDone),
                new CodeMatch(OpCodes.Brfalse))
            .ThrowIfInvalid("couldn't find 'if (!this.threadedLoadDone)'");

        cm
            .Advance() // ldfld bool Allumeria.Game::threadedLoadDone
            .RemoveInstruction() // brfalse IL_04af
            .Insert(
                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Ldsfld, GameDeltaTime),
                CodeInstruction.Call(() => ImplLoadingUpdatedInvoke(default, default)),
                new CodeInstruction(OpCodes.Ret))
            .InsertBranch(OpCodes.Brtrue, cm.Pos + 4);

        return cm.Instructions();
    }

    private static void ImplLoadingUpdatedInvoke(Game instance, double time) => LoadingUpdated?.Invoke(instance, time);

    [HarmonyPostfix]
    [HarmonyPatch("OnUpdateFrame")]
    private static void ImplUpdated(Game __instance, FrameEventArgs e) => Updated?.Invoke(__instance, e.Time);

    [HarmonyTranspiler]
    [HarmonyPatch("OnRenderFrame")]
    private static IEnumerable<CodeInstruction> ImplBeforeRendered(IEnumerable<CodeInstruction> instructions)
    {
        return new CodeMatcher(instructions)
            .MatchStartForward(CodeMatch.Calls(() => GL.Clear(default)))
            .ThrowIfInvalid("couldn't find 'GL.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit)'")
            .InsertAfter(
                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Ldsfld, GameDeltaTime),
                CodeInstruction.Call(() => ImplBeforeRenderedInvoke(default, default)))
            .Instructions();
    }

    private static void ImplBeforeRenderedInvoke(Game game, double time) => BeforeRendered?.Invoke(game, time);

    [HarmonyPostfix]
    [HarmonyPatch(nameof(Game.RenderLoadScreen))]
    private static void ImplLoadingRendered(Game __instance) => LoadingRendered?.Invoke(__instance, Game.deltaTime);

    [HarmonyTranspiler]
    [HarmonyPatch("OnRenderFrame")]
    private static IEnumerable<CodeInstruction> ImplRendered(IEnumerable<CodeInstruction> instructions)
    {
        return new CodeMatcher(instructions)
            .MatchStartForward(CodeMatch.Calls(() => default(Game)!.SwapBuffers()))
            .ThrowIfInvalid("couldn't find 'this.SwapBuffers()'")
            .Insert(
                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Ldsfld, GameDeltaTime),
                CodeInstruction.Call(() => ImplRendered(default, default)))
            .Instructions();
    }

    private static void ImplRendered(Game game, double time) => Rendered?.Invoke(game, time);

    [HarmonyPrefix]
    [HarmonyPatch("OnUnload")]
    private static void ImplUnloaded(Game __instance) => Unloaded?.Invoke(__instance);
}