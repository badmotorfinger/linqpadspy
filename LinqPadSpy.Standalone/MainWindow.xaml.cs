namespace LinqPadSpy
{
    using LinqPadSpy.Controls;

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

            this.Content = new LinqPadSpyContainer(Application.Current, LinqPadUtil.GetLanguageForQuery());
        }

        #endregion
    }
}
namespace LINQPad
{
    using System.Windows;

    using ICSharpCode.ILSpy;

    using LinqPadSpy;
    using LinqPadSpy.Controls;

    public static class LinqPadSpyExtensions
    {
        public static void DumpDasm(this object value)
        {
            if (value == null) return;

            value.Dump(); // Execute LINQPads standard dump.

            // Determine the language (Doesn't work when two copies of LINQPad are open)
            Language linqPadSelectedLanguage = LinqPadUtil.GetLanguageForQuery();

            var linqpadSpyPanel = new LinqPadSpyContainer(new Application(), linqPadSelectedLanguage);

            PanelManager.DisplayWpfElement(linqpadSpyPanel, "Decompiled");
        }
    }
}