using Allumeria;
using OpenTK.Windowing.Desktop;

namespace Ignitron.Aluminium.Events;

/// <summary>
/// Hooks for common game events (such as loading, rendering, ticking, etc.)
/// </summary>
public static partial class AllumeriaEvents
{
    /// <summary>
    /// Invoked before the game starts loading
    /// </summary>
    public static event Action<Game>? BeforeLoaded;

    /// <summary>
    /// Invoked when the game has finished loading
    /// </summary>
    public static event Action<Game>? Loaded;

    /// <summary>
    /// Invoked before the game's <i>background thread</i> starts loading assets. You must <b>not</b> invoke rendering methods inside the callback.
    /// </summary>
    public static event Action<Game>? BeforeLoadedThreaded;

    /// <summary>
    /// Invoked when the game's <i>background thread</i> has finished loading assets. You must <b>not</b> invoke rendering methods inside the callback.
    /// </summary>
    public static event Action<Game>? LoadedThreaded;

    /// <summary>
    /// Invoked before game events perform a tick
    /// </summary>
    public static event Action<Game>? BeforeTicked;

    /// <summary>
    /// Invoked after the game has finished a tick
    /// </summary>
    public static event Action<Game>? Ticked;

    /// <summary>
    /// Invoked before the game updates a frame
    /// </summary>
    public static event Action<Game, double>? BeforeUpdated;

    /// <summary>
    /// Invoked every frame during loading screen
    /// </summary>
    public static event Action<Game, double>? LoadingUpdated;

    /// <summary>
    /// Invoked after the game has finished updating a frame
    /// </summary>
    public static event Action<Game, double>? Updated;

    /// <summary>
    /// Invoked before the game renders a frame
    /// </summary>
    public static event Action<Game, double>? BeforeRendered;

    /// <summary>
    /// Invoked every frame during loading screen
    /// </summary>
    public static event Action<Game, double>? LoadingRendered;

    /// <summary>
    /// Invoked after the game has just finished rendering a frame, but before <see cref="GameWindow.SwapBuffers"/>
    /// </summary>
    public static event Action<Game, double>? Rendered;

    /// <summary>
    /// Invoked before the game starts unloading content. Great place to unload your previously loaded assets
    /// </summary>
    public static event Action<Game>? Unloaded;
}