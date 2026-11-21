# KleeneStar.Theme

The plugin ships colour schemes for the KleeneStar application and its customer portal: **Spring**, **Summer**,
**Autumn** and **Winter**, taken over from the WebExpress UI tutorial
(`WebExpress.Tutorial.WebUI/Assets/css/*.theme.css`) and fitted to KleeneStar. It contributes no
page, no endpoint and no manager.

## How a theme gets in

WebExpress' `ThemeManager` scans every plugin assembly for **`public sealed`** classes
implementing `ITheme` and registers each for the applications the plugin is bound to. The plugin
names the core's application and the portal's (`[Application<KleeneStarApplication>]`,
`[Application<KleeneStarPortalApplication>]`), so every theme is registered twice, once per
application - a theme is registered per application, and naming only the core left the portal
without the seasons. The portal therefore declares a default of its own
(`KleeneStar.Portal/WebTheme/PortalLightTheme`, no stylesheet): without one it would take the first
theme registered for it, a season (`UnitTestThemes.EveryApplicationDeclaresItsDefault`). A class that is not public sealed
is **silently missing** - `UnitTestThemes.EveryThemeIsDiscoverable` guards it.

A theme is described by attributes only:

| Attribute      | Value                                                        |
|----------------|--------------------------------------------------------------|
| `Name`         | `kleenestar.theme:theme.<season>.name`                       |
| `Description`  | `kleenestar.theme:theme.<season>.description`                |
| `Image`        | `kleenestar.theme/assets/img/<season>.svg` (preview swatch)  |
| `ThemeMode`    | `Light` - only the default for a reader who chose no mode    |
| `ThemeStyle`   | `kleenestar.theme/assets/css/<season>.theme.css`             |

The theme id is the lower-cased type name (`kleenestar.theme.webtheme.springtheme`); that is what a
reader's choice stores.

## Asset paths

`ThemeStyle` and `Image` are combined with the **application route** only
(`/kleenestar/` + path), while the asset manager mounts the files of a plugin that contributes to
another plugin's application **under its plugin id** (`/kleenestar/kleenestar.theme/assets/…`,
`/portal/kleenestar.theme/assets/…`).
The attributes therefore start with `KleeneStarPlugin.AssetRoot` (`kleenestar.theme/assets/`). The
embedded resource names keep the folder separator
(`KleeneStar.Theme.Assets.css\spring.theme.css`), which is what the asset manager mounts by.

## The stylesheet

A theme stylesheet **replaces** the framework's default `webexpress.webapp.theme.css` instead of
adding to it, so every sheet starts with

```css
@import url("../../../webexpress.webapp/assets/css/webexpress.webapp.theme.css");
```

(three levels up from `/<application>/kleenestar.theme/assets/css/` is the application root, for
the core and the portal alike), then
redefines the `--wx-*` tokens under `:root, [data-wx-theme="light"]` and under
`[data-wx-theme="dark"]`. Fitted to KleeneStar beyond the tutorial:

- the header rules also cover KleeneStar's header entries (`#wx-header-appnavigator`,
  `#wx-header-search`, `#wx-header-avatar`);
- the signed-out start page (`.ks-welcome`) opens on a gradient from the season's header colour
  into the paper;
- in dark mode, which keeps the framework header, a hairline in the accent marks the header;
- on light pages `.text-secondary` and `.btn-outline-secondary` take the theme's
  `--wx-secondary-text-emphasis`: the framework writes them in `--wx-secondary`, which in these
  themes is a pastel *surface* colour - section captions, hints and the theme/mode selectors were
  barely readable (seen on the portal home and the profile cards).

Everything else follows from the tokens - the paper-white reading view of a record
(`kleenestar.css`) reads `--wx-body-bg` and becomes the season's paper.

## The reader's choice

The choice is the core's, not the plugin's (`KleeneStar.Core/docs/kleenestar.appearance.md`): the
profile page *Appearance* (and the portal profile) offers every theme WebExpress registered for the application, stores the
pick per identity, and lays the reader's adjustments (accent, font size, density, motion) over it.
Removing the plugin removes the themes; a stored id that names no registered theme is no choice,
and the application default (`LightTheme`) applies.

## Adding a theme

1. Add `WebTheme/<Name>Theme.cs` (`public sealed`, `IThemeWebApp`, the five attributes).
2. Add `Assets/css/<name>.theme.css` with the import line and both token blocks, and
   `Assets/img/<name>.svg`.
3. Add `theme.<name>.name` / `.description` to `Internationalization/en` and `de`.
4. Add the type to `UnitTestThemes.Themes` and run the tests.

## Not covered

- An **icon set** (the tutorial's Monkey Island drawings) is not part of this plugin; WebExpress
  supports one through `ThemePreferences.RegisterIconSets`.
