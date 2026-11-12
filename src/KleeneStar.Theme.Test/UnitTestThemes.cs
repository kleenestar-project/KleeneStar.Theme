using KleeneStar.Theme.WebTheme;
using System.Reflection;
using System.Text;
using WebExpress.WebApp.WebTheme;
using WebExpress.WebCore.WebAttribute;
using WebExpress.WebCore.WebTheme;

namespace KleeneStar.Theme.Test
{
    /// <summary>
    /// Tests the shape the framework needs to find a theme of this plugin and to serve its
    /// files: nothing here is registered, so a theme that breaks one of these rules is silently
    /// missing from the appearance page, and a wrong path loads an error page as stylesheet.
    /// </summary>
    public class UnitTestThemes
    {
        private static readonly Assembly _assembly = typeof(KleeneStarPlugin).Assembly;

        /// <summary>
        /// The four seasons, each a theme type of the plugin.
        /// </summary>
        public static TheoryData<Type> Themes => [typeof(SpringTheme), typeof(SummerTheme), typeof(AutumnTheme), typeof(WinterTheme)];

        /// <summary>
        /// WebExpress discovers only public sealed classes implementing a theme interface - and
        /// these are all the plugin ships.
        /// </summary>
        [Fact]
        public void EveryThemeIsDiscoverable()
        {
            var discovered = _assembly.GetTypes()
                .Where(x => x.IsClass && x.IsSealed && x.IsPublic && typeof(ITheme).IsAssignableFrom(x))
                .ToHashSet();

            Assert.Equal(new HashSet<Type> { typeof(SpringTheme), typeof(SummerTheme), typeof(AutumnTheme), typeof(WinterTheme) }, discovered);
            Assert.All(discovered, x => Assert.True(typeof(IThemeWebApp).IsAssignableFrom(x)));
        }

        /// <summary>
        /// A theme is registered per application, so the plugin names both it dresses: the
        /// core and the customer portal.
        /// </summary>
        [Fact]
        public void PluginDressesCoreAndPortal()
        {
            var applications = typeof(KleeneStarPlugin).CustomAttributes
                .Where(x => x.AttributeType.IsGenericType && x.AttributeType.GetGenericTypeDefinition() == typeof(ApplicationAttribute<>))
                .Select(x => x.AttributeType.GetGenericArguments()[0])
                .ToHashSet();

            Assert.Equal(new HashSet<Type> { typeof(KleeneStar.Core.KleeneStarApplication), typeof(KleeneStar.Portal.KleeneStarPortalApplication) }, applications);
        }

        /// <summary>
        /// Each application declares a default of its own; without one the portal would take the
        /// first theme registered for it - a season of this plugin.
        /// </summary>
        /// <param name="application">The application type.</param>
        [Theory]
        [InlineData(typeof(KleeneStar.Core.KleeneStarApplication))]
        [InlineData(typeof(KleeneStar.Portal.KleeneStarPortalApplication))]
        public void EveryApplicationDeclaresItsDefault(Type application)
        {
            var declared = application.CustomAttributes
                .Single(x => x.AttributeType.IsGenericType && x.AttributeType.GetGenericTypeDefinition() == typeof(ThemeAttribute<>))
                .AttributeType.GetGenericArguments()[0];

            Assert.NotEqual(_assembly, declared.Assembly);
            Assert.DoesNotContain(declared.CustomAttributes, x => x.AttributeType == typeof(ThemeStyleAttribute));
        }

        /// <summary>
        /// The asset root names the plugin id, under which the asset manager mounts the files of
        /// a plugin that contributes to another plugin's application.
        /// </summary>
        [Fact]
        public void AssetRootNamesThePluginId()
        {
            Assert.StartsWith(_assembly.GetName().Name.ToLowerInvariant() + "/assets/", KleeneStarPlugin.AssetRoot);
        }

        /// <summary>
        /// The stylesheet and the image a theme names are embedded under the logical name the
        /// asset manager mounts at that path.
        /// </summary>
        /// <param name="theme">The theme type.</param>
        [Theory]
        [MemberData(nameof(Themes))]
        public void StyleAndImageAreEmbedded(Type theme)
        {
            foreach (var path in new[] { Argument<ThemeStyleAttribute>(theme), Argument<ImageAttribute>(theme) })
            {
                Assert.StartsWith(KleeneStarPlugin.AssetRoot, path);

                var resource = ResourceName(path);

                Assert.Contains(resource, _assembly.GetManifestResourceNames());
            }
        }

        /// <summary>
        /// A theme stylesheet replaces the framework's default instead of adding to it, so it
        /// imports the application layout first - three levels up from the plugin's css folder
        /// is the application root - and defines its tokens for light and for dark.
        /// </summary>
        /// <param name="theme">The theme type.</param>
        [Theory]
        [MemberData(nameof(Themes))]
        public void StylesheetImportsTheLayoutAndCoversBothModes(Type theme)
        {
            var css = Read(ResourceName(Argument<ThemeStyleAttribute>(theme)));
            var firstRule = css.Split('\n').Select(x => x.Trim()).First(x => x.StartsWith('@') || x.Contains('{'));

            Assert.Equal("@import url(\"../../../webexpress.webapp/assets/css/webexpress.webapp.theme.css\");", firstRule);
            Assert.Contains(":root, [data-wx-theme=\"light\"] {", css);
            Assert.Contains("[data-wx-theme=\"dark\"] {", css);
            Assert.Contains("--wx-primary:", css);
            Assert.Contains("--wx-body-bg:", css);

            // the pastel secondary is a surface colour; as text it has to take the emphasized ink
            Assert.Contains("html:not([data-wx-theme=\"dark\"]) .text-secondary {", css);
        }

        /// <summary>
        /// Name and description are keys of the plugin, translated in English and German.
        /// </summary>
        /// <param name="theme">The theme type.</param>
        [Theory]
        [MemberData(nameof(Themes))]
        public void NameAndDescriptionAreTranslated(Type theme)
        {
            foreach (var language in new[] { "en", "de" })
            {
                var keys = Read($"KleeneStar.Theme.Internationalization.{language}")
                    .Split('\n')
                    .Select(x => x.Trim())
                    .Where(x => x.Length > 0 && !x.StartsWith('#') && x.Contains('='))
                    .ToDictionary(x => x[..x.IndexOf('=')], x => x[(x.IndexOf('=') + 1)..]);

                foreach (var key in new[] { Argument<NameAttribute>(theme), Argument<DescriptionAttribute>(theme) })
                {
                    Assert.StartsWith("kleenestar.theme:", key);

                    var local = key["kleenestar.theme:".Length..];

                    Assert.True(keys.TryGetValue(local, out var text) && !string.IsNullOrWhiteSpace(text), $"{language}: {local}");
                }
            }
        }

        /// <summary>
        /// Reads the first constructor argument of a theme attribute.
        /// </summary>
        /// <typeparam name="TAttribute">The attribute.</typeparam>
        /// <param name="theme">The theme type.</param>
        /// <returns>The argument.</returns>
        private static string Argument<TAttribute>(Type theme)
        {
            return theme.CustomAttributes
                .Single(x => x.AttributeType == typeof(TAttribute))
                .ConstructorArguments
                .First()
                .Value?
                .ToString();
        }

        /// <summary>
        /// Maps an asset path to its embedded resource name.
        /// </summary>
        /// <param name="path">The path below the asset root, e.g. <c>kleenestar.theme/assets/css/spring.theme.css</c>.</param>
        /// <returns>The resource name, e.g. <c>KleeneStar.Theme.Assets.css\spring.theme.css</c>.</returns>
        private static string ResourceName(string path)
        {
            var relative = path[KleeneStarPlugin.AssetRoot.Length..];

            return "KleeneStar.Theme.Assets." + relative.Replace('/', '\\');
        }

        /// <summary>
        /// Reads an embedded resource as text.
        /// </summary>
        /// <param name="name">The resource name.</param>
        /// <returns>The text.</returns>
        private static string Read(string name)
        {
            using var stream = _assembly.GetManifestResourceStream(name);

            Assert.NotNull(stream);

            using var reader = new StreamReader(stream, Encoding.UTF8);

            return reader.ReadToEnd();
        }
    }
}
