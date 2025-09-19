using System.Text;
using Allumeria;
using Allumeria.Rendering;
using Allumeria.UI;
using Allumeria.UI.Menus;
using Allumeria.UI.UINodes;
using HarmonyLib;
using Ignitron.Loader.API;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Ignitron.Loader;

public class Entrypoint
{
    private sealed class GraphicalProgressDisplay(Game game) : IProgressDisplay
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

            _message = message;
        }
    }

    private sealed class GraphicalCrashHandler(Game game) : MenuController, ICrashHandler
    {
        public bool Crashed;

        private UIPanel panel_main = null!;
        private UIScrollPanel panel_infoScroll = null!;
        private UIVerticalList list_info = null!;
        private UIText text_title = null!;
        private UIText text_description = null!;
        private UIHorizontalList list_buttons = null!;
        private UIButton btn_copy = null!;
        private UIButton btn_mods = null!;
        private UIButton btn_quit = null!;

        public override void BuildMenu(UINode root)
        {
            panel_main = (UIPanel)root.RegisterNode(new UIPanel("panel_crashHandler")
            {
                autoFit = false,
                showColor = true,
                color = TextureBatcher.colorWhite,
                textureX = 16,
                textureY = 48
            });
            panel_infoScroll = (UIScrollPanel)panel_main.RegisterNode(new UIScrollPanel("list_info"));
            list_info = (UIVerticalList)panel_infoScroll.RegisterNode(new UIVerticalList("list_info"));
            panel_infoScroll.SetMainNode(list_info);
            text_title = (UIText)list_info.RegisterNode(new UIText("text_title", "Ignitron has failed to load!!!"));
            text_description = (UIText)list_info.RegisterNode(new UIText("text_description", "!!!"));
            list_buttons = (UIHorizontalList)panel_main.RegisterNode(new UIHorizontalList("list_buttons")
            {
                padding = 0,
                spacing = 4
            });
            btn_copy = (UIButton)list_buttons.RegisterNode(new UIButton("btn_copy", 0, 0, 60, 24, "Copy"));
            btn_mods = (UIButton)list_buttons.RegisterNode(new UIButton("btn_mods", 0, 0, 60, 24, "Mods..."));
            btn_quit = (UIButton)list_buttons.RegisterNode(new UIButton("btn_quit", 0, 0, 60, 24, "Quit"));
        }

        public override void Layout()
        {
            panel_main.SetSize(16, 16, UIManager.scaledWidth - 32, UIManager.scaledHeight - 32);
            list_buttons.SetSize(panel_main.x + 12, panel_main.y + panel_main.h - 36, 188, 24 * UIManager.scale);
            list_info.SetSize(panel_main.x + 12, panel_main.y + 12, panel_main.w - 24, panel_main.y + panel_main.h - 24 - list_buttons.h);
            panel_infoScroll.SetSize(list_info.x, list_info.y, list_info.w, list_info.h);
            base.Layout();
        }

        public override void Update()
        {
            base.Update();

            if (btn_copy.WasActivatedPrimary())
            {
                unsafe
                {
                    GLFW.SetClipboardString(game.WindowPtr, text_description.displayText);
                }
            }
            else if (btn_mods.WasActivatedPrimary())
            {
                Game.OpenLink(Path.Combine(Directory.GetCurrentDirectory(), "mods"));
            }
            else if (btn_quit.WasActivatedPrimary())
            {
                Environment.Exit(0);
            }

            panel_main.show = show;
        }

        public void HandleCrash(Exception exception, string? message)
        {
            text_description.displayText = message is not null ? $"-- {message}:\n{exception}" : exception.ToString();
            Crashed = true;
        }
    }

    [HarmonyPatch(typeof(Game), "SetupGame")]
    private static class GameSetupPatch
    {
        public static void Postfix(Game __instance, ref bool ___threadedLoadDone)
        {
            Logger.Init("not yet");
            ___threadedLoadDone = false;

            Logger.Init("testicular tortion");

            // get version field from loaded assembly
            string fullVersion = Game.VERSION;
            Logger.Init($"Game version: {fullVersion}");

            // append '/ignitron {ver}' so you can identify presence of the modloader
            Game.VERSION = fullVersion + $"/ignitron {ModLoader.Version}";

            // get just version from full version (game stage + version)
            Version version = Version.Parse(fullVersion.AsSpan(fullVersion.IndexOfAny(['0', '1', '2', '3', '4', '5', '6', '7', '8', '9'])));
            if (version.Revision < 0) version = new Version(version.Major, version.Minor, version.Build, 0);

            // initialize visuals
            _progressDisplay = new GraphicalProgressDisplay(__instance);
            _crashHandler = (GraphicalCrashHandler)Game.uiManager.RegisterMenuController(new GraphicalCrashHandler(__instance), Game.uiManager.rootNode);

            ModLoader.Load(
                _progressDisplay,
                _crashHandler,
                Path.Combine(Directory.GetCurrentDirectory(), "mods"),
                version);

            ___threadedLoadDone = true;
        }
    }

    [HarmonyPatch(typeof(Game), "OnUpdateFrame")]
    private static class GameUpdatePatch
    {
        private static bool _wasThreadedLoadDone;

        public static void Prefix(bool ___threadedLoadDone)
        {
            // hide all menus
            if (_crashHandler is { Crashed: true } && !_wasThreadedLoadDone && ___threadedLoadDone)
            {
                foreach (MenuController controller in Game.uiManager.menuControllers)
                {
                    controller.show = false;
                }

                _crashHandler.show = true;
            }

            _wasThreadedLoadDone = ___threadedLoadDone;
        }
    }

    private static GraphicalProgressDisplay _progressDisplay;
    private static GraphicalCrashHandler _crashHandler;

    public static void Init()
    {
        Harmony harmony = new("danilwaffle.Ignitron.Loader");
        harmony.PatchAll();
    }
}