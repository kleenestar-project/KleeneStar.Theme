![KleeneStar](https://raw.githubusercontent.com/kleenestar-project/.github/main/docs/assets/img/banner.png)

# KleeneStar.Theme - Colour for Every Season

**KleeneStar.Theme** is the theme library of the **KleeneStar** platform. It dresses the application and its customer portal in four seasonal colour schemes - **Spring**, **Summer**, **Autumn** and **Winter** - each in a light and a dark mode.

Built on the **WebExpress-Framework**, every theme is nothing but a set of colour tokens the controls already read, so the whole application changes its look without a single control being touched.

## What It Powers

**KleeneStar.Theme** adds themes, not features. WebExpress discovers the theme classes in the plugin and registers them for the KleeneStar application and the service portal; the core never names one of them. Install the plugin and the themes appear, remove it and they are gone - a reader who had picked one simply falls back to the default.

Which theme a page is drawn in is each reader's own choice. On the profile page *Appearance* - and on the portal profile - every signed-in user picks a theme and light or dark, and lays their own adjustments over it: a custom accent colour, the font size, a compact density and reduced motion. The choice is stored with the account, so it follows the person to every browser and device, and from the operator side to the portal.

|Theme  |Character                                                              
|-------|------------------------------------------------------------------------
|Spring |Soft blossom colours with fresh green, lilac and a pink header
|Summer |Light sandy surfaces with turquoise, sky blue, sunshine yellow and coral
|Autumn |Warm cream surfaces with apricot, sage green and terracotta accents
|Winter |Snow-white surfaces with ice blue, cool grey and subtle berry accents

## For Developers

This repository is the starting point for adding colour schemes of your own. A theme is a `public sealed` class implementing `IThemeWebApp`, described by attributes (`Name`, `Description`, `Image`, `ThemeMode`, `ThemeStyle`), plus one stylesheet that imports the framework layout and redefines the `--wx-*` tokens for light and dark. Nothing has to be registered.

The concept, the asset paths and the rules a new theme has to follow are described in [`docs/kleenestar.theme.md`](docs/kleenestar.theme.md); the tests in `KleeneStar.Theme.Test` check every shipped theme against them.

## Legal & Licensing

**KleeneStar.Theme** is released under the `MIT License`, a permissive open-source license that allows reuse, modification, and distribution with minimal restrictions. You're free to use **KleeneStar** in personal, academic, or commercial projects, just include the original copyright notice.

The system is designed to be GDPR-compliant:
- No tracking
- No monetization
- No hidden dependencies
- Full transparency and infrastructure control

**KleeneStar** respects your data and your autonomy. It's built for clarity, not surveillance.

## Contributing

We welcome contributions in many areas:
- New colour schemes and icon sets
- Accessibility reviews of contrast and motion
- UI design and frontend components (CSS/JS)
- Documentation and onboarding flows

Feel free to fork the repository, open issues, or submit pull requests. For larger contributions, please reach out via kleenestar.project@gmail.com.

# AI transparency notice

Parts of this software, its documentation, and its assets were created with the assistance of AI-based tools, including large language models. AI-assisted contributions are reviewed by the project maintainer before they are included.

---

Become part of the **KleeneStar** community and contribute to a modular, open-source future.
