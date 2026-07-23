using System;
using System.Linq;
using System.Windows;

namespace BatUtils
{
    public static class ThemeManager
    {
        public static void SetTheme(bool isDark)
        {
            string themePath = isDark ? "Themes/DarkTheme.xaml" : "Themes/LightTheme.xaml";
            var newTheme = new ResourceDictionary { Source = new Uri(themePath, UriKind.Relative) };

            var oldTheme = Application.Current.Resources.MergedDictionaries
                .FirstOrDefault(d => d.Source != null && d.Source.OriginalString.Contains("Theme"));

            if (oldTheme != null)
                Application.Current.Resources.MergedDictionaries.Remove(oldTheme);

            Application.Current.Resources.MergedDictionaries.Add(newTheme);
        }
    }
}