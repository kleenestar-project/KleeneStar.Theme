using WebExpress.WebApp.WebTheme;
using WebExpress.WebCore.WebAttribute;
using WebExpress.WebCore.WebTheme;

namespace KleeneStar.Theme.WebTheme
{
    /// <summary>
    /// The spring colour scheme: soft blossom colours with fresh green, lilac and a pink header, in light and dark.
    /// </summary>
    /// <remarks>
    /// The colours are tokens every control reads, so the theme reskins KleeneStar without
    /// touching a control; the sheet imports the framework's application layout first because
    /// a theme stylesheet replaces the default one instead of adding to it. The declared mode is
    /// only the default for a reader who has not chosen light or dark. The paths are relative
    /// to the application route, and this plugin's assets are mounted under its id
    /// (<c>/kleenestar/kleenestar.theme/assets/…</c>), because the application is the core's.
    /// </remarks>
    [Name("kleenestar.theme:theme.spring.name")]
    [Description("kleenestar.theme:theme.spring.description")]
    [Image(KleeneStarPlugin.AssetRoot + "img/spring.svg")]
    [ThemeMode(ThemeMode.Light)]
    [ThemeStyle(KleeneStarPlugin.AssetRoot + "css/spring.theme.css")]
    public sealed class SpringTheme : IThemeWebApp
    {
    }
}
