namespace LinqPadSpy.Standalone
{
    using System.Windows;

    using LinqPadSpy.Plugin;
    using LinqPadSpy.Plugin.Controls;

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