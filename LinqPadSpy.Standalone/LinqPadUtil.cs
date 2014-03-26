namespace LinqPadSpy
{
    using System.Linq;
    using System.Windows.Automation;

    using ICSharpCode.ILSpy;
    using ICSharpCode.ILSpy.VB;

    /// <summary>
    /// Provides utility methods for LINQPad.
    /// </summary>
    public static class LinqPadUtil
    {
        /// <summary>
        /// Gets the language which LINQPad is currently set to for the active tab.
        /// </summary>
        /// <returns></returns>
        public static Language GetLanguageForQuery()
        {
            // The code is qute verbose and ugly but it was the only way I know to look up 
            // the combo box selection without performing a deep traverse of the control tree (which takes far too long to be acceptable).
            var aeDesktop = AutomationElement.RootElement;

            var aeForm =
                aeDesktop.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.NameProperty, "LINQPad 4"));

            var windowPane = aeForm.FindAll(TreeScope.Children, Condition.TrueCondition)[0];

            var windowPanes = windowPane.FindAll(TreeScope.Children, Condition.TrueCondition);

            var rightOfSplitPane = windowPanes[1];

            var tabPane = rightOfSplitPane.FindAll(TreeScope.Children, Condition.TrueCondition)[1];

            var selectedTabPattern = (SelectionPattern)tabPane.GetCurrentPattern(SelectionPattern.Pattern);

            var selectedTab = selectedTabPattern.Current.GetSelection()[0];

            var combo =
                selectedTab.FindAll(TreeScope.Children, Condition.TrueCondition)[0]
                        .FindAll(TreeScope.Children, Condition.TrueCondition)[1]
                        .FindAll(TreeScope.Children, new PropertyCondition(AutomationElement.AutomationIdProperty, "cboLanguage"))[0];

            var selp = (SelectionPattern)combo.GetCurrentPattern(SelectionPattern.Pattern);

            if (selp.Current.GetSelection().First().Current.Name.Contains("VB")) return new VBLanguage();

            return new CSharpLanguage();
        }
    }
}