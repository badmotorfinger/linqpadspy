namespace LINQPad
{
    using System.Windows;

    using ICSharpCode.ILSpy;

    using LinqPadSpy.Plugin;
    using LinqPadSpy.Plugin.Controls;

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