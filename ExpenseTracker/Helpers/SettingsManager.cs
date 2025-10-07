using System;
using System.IO;
using System.Text.Json;

namespace ExpenseTracker.Helpers
{
    public class SettingsManager
    {
        private static readonly string SettingsFolder = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "ExpenseTracker"
        );
        
        private static readonly string SettingsFile = Path.Combine(SettingsFolder, "settings.json");

        public class AppSettings
        {
            public string Theme { get; set; } = "Light";
            public string Currency { get; set; } = "₹";
            public string DateFormat { get; set; } = "dd/MM/yyyy";
        }

        public static AppSettings LoadSettings()
        {
            try
            {
                if (File.Exists(SettingsFile))
                {
                    string json = File.ReadAllText(SettingsFile);
                    return JsonSerializer.Deserialize<AppSettings>(json) ?? new AppSettings();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading settings: {ex.Message}");
            }

            return new AppSettings();
        }

        public static void SaveSettings(AppSettings settings)
        {
            try
            {
                Directory.CreateDirectory(SettingsFolder);
                
                var options = new JsonSerializerOptions { WriteIndented = true };
                string json = JsonSerializer.Serialize(settings, options);
                File.WriteAllText(SettingsFile, json);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error saving settings: {ex.Message}");
            }
        }

        public static string GetTheme()
        {
            return LoadSettings().Theme;
        }

        public static void SetTheme(string theme)
        {
            var settings = LoadSettings();
            settings.Theme = theme;
            SaveSettings(settings);
        }
    }
}
