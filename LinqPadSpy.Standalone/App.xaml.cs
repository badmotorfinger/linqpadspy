namespace LinqPadSpy
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition.Hosting;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Threading;

    using ICSharpCode.ILSpy.Debugger.Services;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        #region Static Fields

        internal static IList<ExceptionData> StartupExceptions = new List<ExceptionData>();

        private static CompositionContainer compositionContainer;

        #endregion

        static App()
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
                try
                {
                    var asm = Assembly.Load(shortName);
                    asm.GetTypes();
                    catalog.Catalogs.Add(new AssemblyCatalog(asm));
                }
                catch (Exception ex)
                {
                    // Cannot show MessageBox here, because WPF would crash with a XamlParseException
                    // Remember and show exceptions in text output, once MainWindow is properly initialized
                    StartupExceptions.Add(new ExceptionData { Exception = ex, PluginName = shortName });
                }
            }

            compositionContainer = new CompositionContainer(catalog);

            if (!System.Diagnostics.Debugger.IsAttached)
            {
                AppDomain.CurrentDomain.UnhandledException += ShowErrorBox;
                Dispatcher.CurrentDispatcher.UnhandledException += Dispatcher_UnhandledException;
            }

            try
            {
                DebuggerService.SetDebugger(compositionContainer.GetExport<IDebugger>());
            }
            catch
            {
                // unable to find a IDebugger
            }
        }

        #region Constructors and Destructors

        public App()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Public Properties

        public static CompositionContainer CompositionContainer
        {
            get
            {
                return compositionContainer;
            }
        }

        #endregion

        #region Methods

        private static void Dispatcher_UnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            Debug.WriteLine(e.Exception.ToString());
            MessageBox.Show(e.Exception.ToString(), "Sorry, we crashed");
            e.Handled = true;
        }

        private static void LoadAssemblyByShortName(AggregateCatalog catalog, string ilspy)
        {
            var ilspyasm = Assembly.Load(ilspy);
            ilspyasm.GetTypes();
            catalog.Catalogs.Add(new AssemblyCatalog(ilspyasm));
        }

        private static void ShowErrorBox(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = e.ExceptionObject as Exception;
            if (ex != null)
            {
                Debug.WriteLine(ex.ToString());
                MessageBox.Show(ex.ToString(), "Sorry, we crashed");
            }
        }

        #endregion

        internal class ExceptionData
        {
            #region Fields

            public Exception Exception;

            public string PluginName;

            #endregion
        }
    }
}