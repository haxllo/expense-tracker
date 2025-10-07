using System;
using System.Linq;
using System.Windows;

namespace ExpenseTracker.Helpers
{
    public static class ThemeManager
    {
        public static void ApplyTheme(string themeName)
        {
            var app = Application.Current;
            if (app == null) return;

            var existingTheme = app.Resources.MergedDictionaries
                .FirstOrDefault(d => d.Source != null && 
                    (d.Source.OriginalString.Contains("LightTheme") || 
                     d.Source.OriginalString.Contains("DarkTheme")));

            if (existingTheme != null)
            {
                app.Resources.MergedDictionaries.Remove(existingTheme);
            }

            var themeUri = new Uri($"Themes/{themeName}Theme.xaml", UriKind.Relative);
            var themeDict = new ResourceDictionary { Source = themeUri };
            app.Resources.MergedDictionaries.Add(themeDict);

            SettingsManager.SetTheme(themeName);
        }

        public static void LoadSavedTheme()
        {
            var savedTheme = SettingsManager.GetTheme();
            ApplyTheme(savedTheme);
        }

        public static void ToggleTheme()
        {
            var currentTheme = SettingsManager.GetTheme();
            var newTheme = currentTheme == "Light" ? "Dark" : "Light";
            ApplyTheme(newTheme);
        }

        public static string GetCurrentTheme()
        {
            return SettingsManager.GetTheme();
        }
    }
}
