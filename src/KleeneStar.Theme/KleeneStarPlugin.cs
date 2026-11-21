using KleeneStar.Core;
using KleeneStar.Portal;
using WebExpress.WebCore.WebAttribute;
using WebExpress.WebCore.WebPlugin;

namespace KleeneStar.Theme
{
    /// <summary>
    /// Plugin entry point of the theme library: the four seasons as colour schemes of KleeneStar
    /// and of its customer portal.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The plugin contributes no page, no endpoint and no manager - only theme classes
    /// (<c>WebTheme/</c>) and their stylesheets. WebExpress discovers every <c>public sealed</c>
    /// <c>ITheme</c> of a plugin's assembly and registers it for the applications the plugin is
    /// bound to, so nothing has to be registered here, and the core never names a theme of this
    /// plugin: its appearance page offers whatever the framework registered, and stores the pick
    /// per identity (<c>IAppearanceManager</c>). Removing the plugin removes the themes; a reader
    /// who had picked one falls back to the application's default.
    /// </para>
    /// <para>
    /// It names the core's application and the portal's rather than defining one, because a
    /// plugin without an application is refused by the framework and these themes dress those
    /// two. A theme is registered per application, so naming both is what puts the seasons on
    /// both sides; the reader's choice is one record, so a season picked on one side is in use
    /// on the other. The assets are mounted under the plugin id inside each application's route
    /// (<see cref="AssetRoot"/>: <c>/kleenestar/kleenestar.theme/…</c>, <c>/portal/kleenestar.theme/…</c>).
    /// </para>
    /// </remarks>
    [Name("kleenestar.theme:plugin.name")]
    [Description("kleenestar.theme:plugin.description")]
    [Application<KleeneStarApplication>()]
    [Application<KleeneStarPortalApplication>()]
    [Dependency("webexpress.webapp")]
    [Dependency("kleenestar.core")]
    [Dependency("kleenestar.portal")]
    public sealed class KleeneStarPlugin : IPlugin
    {
        /// <summary>
        /// The root of the plugin's assets relative to the application route.
        /// </summary>
        /// <remarks>
        /// A theme's style and image are combined with the application route only, while the
        /// asset manager mounts a contributing plugin's files under its id, so the id has to be
        /// part of the path the attributes name.
        /// </remarks>
        public const string AssetRoot = "kleenestar.theme/assets/";

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public KleeneStarPlugin()
        {
        }

        /// <summary>
        /// Called when the plugin starts working. The themes need no start.
        /// </summary>
        public void Run()
        {
        }

        /// <summary>
        /// Called when the host shuts down. The plugin holds no resources of its own.
        /// </summary>
        public void Dispose()
        {
        }
    }
}
