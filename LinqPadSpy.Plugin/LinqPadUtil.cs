namespace LinqPadSpy.Plugin
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Windows.Automation;

    using ICSharpCode.ILSpy;
    using ICSharpCode.ILSpy.VB;

    /// <summary>
    /// Provides utility methods for LINQPad.
    /// </summary>
    public static class LinqPadUtil
    {

        static readonly Lazy<SelectionPattern> ActiveTabSelectionPattern = new Lazy<SelectionPattern>(
            () =>
            {
                // The code is qute verbose and ugly but it was the only way I know to look up 
                // the combo box selection without performing a deep traverse of the control tree (which takes far too long to be acceptable).
                var aeDesktop = AutomationElement.RootElement;

                var aeForm =
                    aeDesktop.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.NameProperty, "LINQPad 4"));

                if (aeForm == null)
                {
                    return null;
                }

                var windowPane = aeForm.FindAll(TreeScope.Children, Condition.TrueCondition)[0];

                var windowPanes = windowPane.FindAll(TreeScope.Children, Condition.TrueCondition);

                var rightOfSplitPane = windowPanes[1];

                var tabPane = rightOfSplitPane.FindAll(TreeScope.Children, Condition.TrueCondition)[1];

                return (SelectionPattern)tabPane.GetCurrentPattern(System.Windows.Automation.SelectionPattern.Pattern);
            });

        static SelectionPattern SelectedLinqPadTab
        {
            get
            {
                return ActiveTabSelectionPattern.Value;
            }
        }

        /// <summary>
        /// Gets the language which LINQPad is currently set to for the active tab.
        /// </summary>
        /// <returns></returns>
        public static Language GetLanguageForQuery()
        {
            try
            {
                if (SelectedLinqPadTab != null)
                {
                    var selectedTab = SelectedLinqPadTab.Current.GetSelection()[0];

                    var combo =
                        selectedTab.FindAll(TreeScope.Children, Condition.TrueCondition)[0].FindAll(
                            TreeScope.Children,
                            Condition.TrueCondition)[1].FindAll(
                                TreeScope.Children,
                                new PropertyCondition(AutomationElement.AutomationIdProperty, "cboLanguage"))[0];

                    var selp = (SelectionPattern)combo.GetCurrentPattern(System.Windows.Automation.SelectionPattern.Pattern);

                    if (selp.Current.GetSelection().First().Current.Name.Contains("VB")) return new VBLanguage();
                }
            }
            catch { } // This line disgusts me.

            return new CSharpLanguage();
        }

        /// <summary>
        /// Gets the last compiled and executed LINQPad query assembly.
        /// </summary>
        /// <returns>A path to the assembly.</returns>
        public static string GetLastLinqPadQueryAssembly()
        {
            var tempPath = Path.Combine(Path.GetTempPath(), "linqpad");

            var linqpadDirectory = new DirectoryInfo(tempPath);

            IQueryable<DirectoryInfo> latestDirectory =
                linqpadDirectory.GetDirectories().AsQueryable();

            if (latestDirectory.Any())
            {
                latestDirectory = latestDirectory.OrderByDescending(dir => dir.LastWriteTime);
            }
            else
            {
                throw new ApplicationException("Could not find LINQPad's last executed query.");
            }


            var latestQuery =
                latestDirectory.First().GetFiles("*.dll").OrderByDescending(file => file.LastWriteTimeUtc).First();

            return latestQuery.FullName;
        }
    }
}