using Allumeria;
using HarmonyLib;
using Ignitron.Loader.API;
using OpenTK.Graphics.OpenGL4;

namespace Ignitron.Loader;

public class Entrypoint
{
    private sealed class LoadStateProgressDisplay(Game game) : IProgressDisplay
    {
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
                game.loadState = _category is null ? _message : $"{_category}: {_message}";
            }
            else if (_category is not null)
            {
                game.loadState = $"{_category}...";
            }
            else
            {
                game.loadState = string.Empty;
            }

            // now render loading screen
            GL.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit);
            game.RenderLoadScreen();
            game.SwapBuffers();

            _message = message;
        }
    }

    [HarmonyPatch(typeof(Game), "OnLoad")]
    private static class GameLoadPatch
    {
        public static void Postfix(Game __instance)
        {
            Logger.Init("testicular tortion");

            // get version field from loaded assembly
            string fullVersion = Game.VERSION;
            Logger.Init($"Game version: {fullVersion}");

            // append '/ignitron {ver}' so you can identify presence of the modloader
            Game.VERSION = fullVersion + $"/ignitron {ModLoader.Version}";

            // get just version from full version (game stage + version)
            Version version = Version.Parse(fullVersion.AsSpan(fullVersion.IndexOfAny(['0', '1', '2', '3', '4', '5', '6', '7', '8', '9'])));
            if (version.Revision < 0) version = new Version(version.Major, version.Minor, version.Build, 0);
            ModLoader.Load(new LoadStateProgressDisplay(__instance), Path.Combine(Directory.GetCurrentDirectory(), "mods"), version);
        }
    }

    public static void Init()
    {
        Harmony harmony = new("danilwaffle.Ignitron.Loader");
        harmony.PatchAll();
    }
}