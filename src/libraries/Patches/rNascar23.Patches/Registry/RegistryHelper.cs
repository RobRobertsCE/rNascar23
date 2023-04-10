using Microsoft.Win32;
using System;
using System.Linq;

namespace rNascar23.Patches.AppRegistry
{
    public static class RegistryHelper
    {
        private const string RegistryVersionKey = "SOFTWARE\\RacerData\\rNascar23\\Version";
        private const string RegistryPathKey = "SOFTWARE\\RacerData\\rNascar23\\Path";

        private const string VersionKey = "Version";
        private const string PathKey = "Path";

        public static string GetInstallFolder()
        {
            RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(RegistryPathKey, true);

            var keyValue = registryKey.GetValue(PathKey, String.Empty, RegistryValueOptions.None);

            if (keyValue != null)
                return keyValue.ToString();
            else
                return String.Empty;
        }
        public static void SetInstallFolder(string path)
        {
            RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(RegistryPathKey, true);

            if (registryKey == null)
                registryKey = registryKey.CreateSubKey(RegistryPathKey);

            registryKey.SetValue(PathKey, path);
        }

        public static string GetVersion()
        {
            RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(RegistryVersionKey, true);

            var keyValue = registryKey.GetValue(VersionKey, String.Empty, RegistryValueOptions.None);

            if (keyValue != null)
                return keyValue.ToString();
            else
                return String.Empty;
        }
        public static void SetVersion(string version)
        {
            RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(RegistryVersionKey, true);

            if (registryKey == null)
                registryKey = registryKey.CreateSubKey(RegistryVersionKey);

            registryKey.SetValue(VersionKey, version);
        }
    }
}
