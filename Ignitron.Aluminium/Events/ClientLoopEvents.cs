using Allumeria;
using OpenTK.Windowing.Desktop;

namespace Ignitron.Aluminium.Events;

/// <summary>
/// Hooks for common <see cref="Game"/> events (such as loading, rendering, ticking, etc.)
/// </summary>
public static partial class ClientLoopEvents
{
    /// <summary>
    /// Occurs before the game starts loading
    /// </summary>
    public static event Action<Game>? Loading;

    /// <summary>
    /// Occurs when the game has finished loading
    /// </summary>
    public static event Action<Game>? Loaded;

    /// <summary>
    /// Occurs before the game's <i>background thread</i> starts loading assets. You must <b>not</b> invoke rendering methods inside the callback.
    /// </summary>
    public static event Action<Game>? LoadingThreaded;

    /// <summary>
    /// Occurs when the game's <i>background thread</i> has finished loading assets. You must <b>not</b> invoke rendering methods inside the callback.
    /// </summary>
    public static event Action<Game>? LoadedThreaded;

    /// <summary>
    /// Occurs when the game finishes loading assets and components
    /// </summary>
    public static event Action<Game>? LoadedEverything;

    /// <summary>
    /// Occurs before game events perform a tick
    /// </summary>
    public static event Action<Game>? Ticking;

    /// <summary>
    /// Occurs after the game has finished a tick
    /// </summary>
    public static event Action<Game>? Ticked;

    /// <summary>
    /// Occurs before the game updates a frame
    /// </summary>
    public static event Action<Game, double>? Updating;

    /// <summary>
    /// Occurs every frame during loading screen
    /// </summary>
    public static event Action<Game, double>? LoadingUpdated;

    /// <summary>
    /// Occurs after the game has finished updating a frame
    /// </summary>
    public static event Action<Game, double>? Updated;

    /// <summary>
    /// Occurs before the game renders a frame
    /// </summary>
    public static event Action<Game, double>? Rendering;

    /// <summary>
    /// Occurs every frame during loading screen
    /// </summary>
    public static event Action<Game, double>? LoadingRendered;

    /// <summary>
    /// Occurs after the game has just finished rendering a frame, but before <see cref="GameWindow.SwapBuffers"/>
    /// </summary>
    public static event Action<Game, double>? Rendered;

    /// <summary>
    /// Occurs before the game starts unloading content. Great place to unload your previously loaded assets
    /// </summary>
    public static event Action<Game>? Unloaded;
}