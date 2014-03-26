namespace LinqPadSpy
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Media;

    using ICSharpCode.ILSpy;
    using ICSharpCode.ILSpy.TextView;
    using ICSharpCode.ILSpy.TreeNodes;

    using Mono.Cecil;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Constructors and Destructors

        public MainWindow()
        {
            this.InitializeComponent();

            this.Content = GetDecompilerTextView(LINQPad.LinqPadSpyExtensions.GetLastLinqPadQueryAssembly());
        }

        public static DecompilerTextView GetDecompilerTextView(string assemblyPath)
        {
            return GetDecompilerTextView(assemblyPath, Application.Current);
        }

        public static DecompilerTextView GetDecompilerTextView(string assemblyPath, System.Windows.Application currenApplication)
        {
            var loadedAssembly = LoadAssembly(assemblyPath, currenApplication);

            var assemblyDefinition = GetAssemblyDefinition(loadedAssembly);

            var mainModule = assemblyDefinition.MainModule;

            var assemblyTreeNode = new AssemblyTreeNode(loadedAssembly);

            var typesToDecompile = GetModuleTypes(mainModule, assemblyTreeNode);

            var decompilerTextView = new DecompilerTextView(CompositionContainerBuilder.Container);

            var linqPadSelectedLanguage = LinqPadUtil.GetLanguageForQuery();

            decompilerTextView.Decompile(linqPadSelectedLanguage, typesToDecompile, new DecompilationOptions());

            return decompilerTextView;
        }

        static LoadedAssembly LoadAssembly(string assemblyPath, Application currenApplication)
        {
            var assemblyList = new AssemblyList("LINQPadList");

            return new LoadedAssembly(assemblyList, assemblyPath, currenApplication);
        }

        static AssemblyDefinition GetAssemblyDefinition(LoadedAssembly loadedAssembly)
        {
            var asmDef = loadedAssembly.AssemblyDefinition;

            if (asmDef == null)
            {
                throw new InvalidOperationException("Could not load for some reason.");
            }

            return asmDef;
        }

        static IEnumerable<TypeTreeNode> GetModuleTypes(ModuleDefinition mainModule, AssemblyTreeNode assemblyTreeNode)
        {
            return mainModule.Types.OrderBy(t => t.FullName).Select(type => new TypeTreeNode(type, assemblyTreeNode));
        }

        #endregion
    }
}
namespace LINQPad
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Net.Mime;
    using System.Windows;

    public static class LinqPadSpyExtensions
    {
        public static void DumpDasm(this object value)
        {
            if (value == null) return;

            var linqpadQueryAssemblyPath = GetLastLinqPadQueryAssembly();

            using (var decompilerTextView = LinqPadSpy.MainWindow.GetDecompilerTextView(linqpadQueryAssemblyPath, new Application()))
            {
                PanelManager.DisplayWpfElement(decompilerTextView, "Decompiled");
            }
        }

        public static string GetLastLinqPadQueryAssembly()
        {
            var tempPath = Path.Combine(Path.GetTempPath(), "linqpad");

            var linqpadDirectory = new DirectoryInfo(tempPath);

            var latestDirectory =
                linqpadDirectory.GetDirectories().OrderByDescending(dir => dir.LastWriteTime).First();

            var latestQuery =
                latestDirectory.GetFiles().OrderByDescending(file => file.LastWriteTimeUtc).First();

            return latestQuery.FullName;
        }
    }
}