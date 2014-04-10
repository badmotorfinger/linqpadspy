namespace LinqPadSpy
{
    using System.ComponentModel.Composition;

    using ICSharpCode.ILSpy;
    using ICSharpCode.ILSpy.TextView;
    using ICSharpCode.ILSpy.TreeNodes;

    using LinqPadSpy.Controls;

    using Mono.Cecil;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Constructors and Destructors

        public MainWindow()
        {
            this.InitializeComponent();

            this.Content = GetDecompilerTextView(LinqPadUtil.GetLastLinqPadQueryAssembly());
        }

        public static DecompilerTextView GetDecompilerTextView(string assemblyPath)
        {
            return GetDecompilerTextView(assemblyPath, Application.Current);
        }

        public static DecompilerTextView GetDecompilerTextView(string assemblyPath, Application currenApplication)
        {
            var loadedAssembly = LoadAssembly(assemblyPath, currenApplication);

            var assemblyDefinition = GetAssemblyDefinition(loadedAssembly);

            var mainModule = assemblyDefinition.MainModule;

            var assemblyTreeNode = new AssemblyTreeNode(loadedAssembly);

            var typesToDecompile = GetModuleTypes(mainModule, assemblyTreeNode);

            var linqPadSelectedLanguage = LinqPadUtil.GetLanguageForQuery();

            var decompilerTextView = new DecompilerTextView();

            CompositionContainerBuilder.Container.ComposeParts(decompilerTextView);

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
    using System.Windows;

    using LinqPadSpy;

    public static class LinqPadSpyExtensions
    {
        public static void DumpDasm(this object value)
        {
            if (value == null) return;

            value.Dump(); // Execute LINQPads standard dump.

            var linqpadQueryAssemblyPath = LinqPadUtil.GetLastLinqPadQueryAssembly();

            using (var decompilerTextView = LinqPadSpy.MainWindow.GetDecompilerTextView(linqpadQueryAssemblyPath, new Application()))
            {
                PanelManager.DisplayWpfElement(decompilerTextView, "Decompiled");
            }
        }
    }
}