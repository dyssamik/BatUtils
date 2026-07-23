using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using BatUtils.Services;

namespace BatUtils.Views
{
    public partial class SettingsView : UserControl
    {
        public SettingsView()
        {
            InitializeComponent();
            UpdateThemeSelection();
            UpdateLanguageSelection();
        }

        private void UpdateThemeSelection()
        {
            ThemeComboBox.SelectedIndex =
                Properties.Settings.Default.Theme == "Dark"
                    ? 0
                    : 1;
        }

        private void UpdateLanguageSelection()
        {
            EnglishRadio.IsChecked =
                Properties.Settings.Default.Language == "en";

            RussianRadio.IsChecked =
                Properties.Settings.Default.Language == "ru";
        }

        private void ThemeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ThemeComboBox.SelectedItem is ComboBoxItem selected)
            {
                ThemeManager.SetTheme(selected.Tag.ToString() == "Dark");

                Properties.Settings.Default.Theme = selected.Tag.ToString();
                Properties.Settings.Default.Save();
            }
        }

        private void EnglishRadio_Checked(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Language = "en";
            Properties.Settings.Default.Save();
        }

        private void RussianRadio_Checked(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Language = "ru";
            Properties.Settings.Default.Save();
        }
    }
}