namespace Clac.UI.Configuration;

/// <summary>
/// Provides centralized access to application settings.
/// </summary>
public static class SettingsManager
{
    /// <summary>
    /// Gets the UI settings.
    /// </summary>
    public static UISettings UI { get; } = new UISettings();
}

