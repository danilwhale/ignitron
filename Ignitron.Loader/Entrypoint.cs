using Allumeria;
using System.Reflection;
using HarmonyLib;
using Ignitron.Loader.API;

namespace Ignitron.Loader;

public class Entrypoint
{
    private sealed class LoadStateProgressDisplay(object game) : IProgressDisplay
    {
        private static readonly FieldInfo LoadStateField = AccessTools.DeclaredField("Allumeria.Game:loadState");
        private static readonly MethodInfo GlClear = AccessTools.DeclaredMethod("OpenTK.Graphics.OpenGL4.GL:Clear");
        private static readonly MethodInfo RenderLoadScreenMethod = AccessTools.DeclaredMethod("Allumeria.Game:RenderLoadScreen");
        private static readonly MethodInfo SwapBuffersMethod = AccessTools.DeclaredMethod("OpenTK.Windowing.Desktop.GameWindow:SwapBuffers");

        private string? _category;
        private string? _message;

        public void UpdateCategory(string? category)
        {
            _category = category;
            UpdateMessage(null);
        }

        public void UpdateMessage(string? message)
        {
            if (_message is not null)
            {
                LoadStateField.SetValue(game, _category is null ? _message : $"{_category}: {_message}");
            }
            else if (_category is not null)
            {
                LoadStateField.SetValue(game, $"{_category}...");
            }
            else
            {
                LoadStateField.SetValue(game, null);
            }

            // now render loading screen
            GlClear.Invoke(null, [0x100 | 0x4000 /* GL_DEPTH_BUFFER_BIT | GL_COLOR_BUFFER_BIT */]);
            RenderLoadScreenMethod.Invoke(game, null);
            SwapBuffersMethod.Invoke(game, null);

            _message = message;
        }
    }

    private static class GamePatch
    {
        [HarmonyPatch]
        private static class OnLoadPatch
        {
            public static MethodBase TargetMethod()
            {
                return AccessTools.DeclaredMethod("Allumeria.Game:OnLoad");
            }

            public static void Postfix(object __instance)
            {
                Logger.Init("testicular tortion");

                // get version field from loaded assembly
                string fullVersion = Game.VERSION!;
                Logger.Init($"Game version: {fullVersion}");

                // append '/ignitron {ver}' so you can identify presence of the modloader
                Game.VERSION = fullVersion + $"/ignitron {ModLoader.Version}";

                // get just version from full version (game stage + version)
                Version version = Version.Parse(fullVersion.AsSpan(fullVersion.IndexOfAny(['0', '1', '2', '3', '4', '5', '6', '7', '8', '9'])));
                if (version.Revision < 0) version = new Version(version.Major, version.Minor, version.Build, 0);
                ModLoader.Load(new LoadStateProgressDisplay(__instance), Path.Combine(Directory.GetCurrentDirectory(), "mods"), version);
            }
        }
    }

    public static void Init()
    {
        Harmony harmony = new("danilwaffle.Ignitron.Loader");
        harmony.PatchAll();
    }
}