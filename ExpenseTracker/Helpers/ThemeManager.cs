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
                    (d.Source.OriginalString.Contains("Theme.xaml")));

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
            // Default to Modern theme (light theme with color scheme)
            var savedTheme = SettingsManager.GetTheme();
            
            // If saved theme is Dark or Light (old themes), use Modern instead
            if (savedTheme == "Dark" || savedTheme == "Light")
            {
                savedTheme = "Modern";
                SettingsManager.SetTheme("Modern");
            }
            
            ApplyTheme(savedTheme);
        }

        public static void ToggleTheme()
        {
            // Only Modern theme is available now
            var currentTheme = SettingsManager.GetTheme();
            // Keep it as Modern (no toggle needed with single theme)
            ApplyTheme("Modern");
        }

        public static string GetCurrentTheme()
        {
            return SettingsManager.GetTheme();
        }
    }
}
