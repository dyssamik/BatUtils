using BatUtils.Services;
using System.Windows;

namespace BatUtils
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            LocalizationManager.SetLanguage(
                global::BatUtils.Properties.Settings.Default.Language);

            ThemeManager.SetTheme(
                global::BatUtils.Properties.Settings.Default.Theme == "Dark");
        }
    }
}