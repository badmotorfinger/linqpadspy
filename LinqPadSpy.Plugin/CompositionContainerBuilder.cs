namespace LinqPadSpy.Plugin
{
    using System.ComponentModel.Composition.Hosting;
    using System.IO;
    using System.Reflection;

    using ICSharpCode.ILSpy;

    public static class CompositionContainerBuilder
    {
        static CompositionContainer container;

        public static CompositionContainer Container
        {
            get
            {
                if (container == null)
                {

                    var catalog = new AggregateCatalog();

                    catalog.Catalogs.Add(new AssemblyCatalog(typeof(App).Assembly));

                    // Don't use DirectoryCatalog, that causes problems if the plugins are from the Internet zone
                    // see http://stackoverflow.com/questions/8063841/mef-loading-plugins-from-a-network-shared-folder
                    string appPath = Path.GetDirectoryName(typeof(App).Module.FullyQualifiedName);

                    LoadAssemblyByShortName(catalog, "ILSpy");
                    LoadAssemblyByShortName(catalog, "ICSharpCode.AvalonEdit");

                    foreach (string plugin in Directory.GetFiles(appPath, "*.Plugin.dll"))
                    {
                        string shortName = Path.GetFileNameWithoutExtension(plugin);

                        var asm = Assembly.Load(shortName);
                        asm.GetTypes();
                        catalog.Catalogs.Add(new AssemblyCatalog(asm));

                    }
                    container = new CompositionContainer(catalog);
                }

                return container;
            }
        }

        static void LoadAssemblyByShortName(AggregateCatalog catalog, string ilspy)
        {
            var ilspyasm = Assembly.Load(ilspy);
            ilspyasm.GetTypes();
            catalog.Catalogs.Add(new AssemblyCatalog(ilspyasm));
        }
    }
}