namespace SnIoGui.Services
{
    /// <summary>
    /// Interface for managing application settings at runtime.
    /// </summary>
    public interface IRuntimeSettingsManager
    {
        /// <summary>
        /// Gets the current settings.
        /// </summary>
        SnIoGuiSettings Settings { get; }

        /// <summary>
        /// Saves the settings to the persistent storage.
        /// </summary>
        void SaveSettings();

        /// <summary>
        /// Reloads the settings from the persistent storage.
        /// </summary>
        void ReloadSettings();
    }
}
