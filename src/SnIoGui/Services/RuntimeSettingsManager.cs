using System;
using System.IO;
using System.Text.Json;

namespace SnIoGui.Services
{
    /// <summary>
    /// Manages application settings at runtime with persistence to JSON file.
    /// </summary>
    public class RuntimeSettingsManager : IRuntimeSettingsManager
    {
        private readonly string _settingsFilePath;
        private SnIoGuiSettings _settings;
        private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true
        };

        public SnIoGuiSettings Settings => _settings;

        public RuntimeSettingsManager()
        {
            // Store settings in AppData folder
            var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var appFolder = Path.Combine(appDataPath, "SnIoGui");
            Directory.CreateDirectory(appFolder);
            _settingsFilePath = Path.Combine(appFolder, "settings.json");

            LoadSettings();
        }

        public void SaveSettings()
        {
            try
            {
                var json = JsonSerializer.Serialize(_settings, _jsonOptions);
                File.WriteAllText(_settingsFilePath, json);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to save settings to {_settingsFilePath}", ex);
            }
        }

        public void ReloadSettings()
        {
            LoadSettings();
        }

        private void LoadSettings()
        {
            if (File.Exists(_settingsFilePath))
            {
                try
                {
                    var json = File.ReadAllText(_settingsFilePath);
                    _settings = JsonSerializer.Deserialize<SnIoGuiSettings>(json, _jsonOptions) 
                        ?? CreateDefaultSettings();
                }
                catch
                {
                    _settings = CreateDefaultSettings();
                }
            }
            else
            {
                _settings = CreateDefaultSettings();
                SaveSettings(); // Create the file with defaults
            }
        }

        private SnIoGuiSettings CreateDefaultSettings()
        {
            return new SnIoGuiSettings
            {
                SnIO = "SnIO.exe",
                Cleaner = "Cleaner.exe",
                Targets = new List<Target>()
            };
        }
    }
}
