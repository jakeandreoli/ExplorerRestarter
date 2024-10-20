using Microsoft.Win32;

namespace ExplorerRestarter.Utilities;

public static class SystemConfiguration
{
    public static bool DarkTheme()
    {
        using RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize");
        
        return key?.GetValue("AppsUseLightTheme") is 0;
    }
}