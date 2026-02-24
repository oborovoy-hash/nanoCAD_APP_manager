using Microsoft.Win32;
using System;
using System.Collections.Generic;

namespace NanoCADModuleManager
{
    public static class NanoCADVersionsHelper
    {
        private const string NANO_CAD_REGISTRY_PATH = @"SOFTWARE\Nanosoft\nanoCAD x64";

        /// <summary>
        /// Gets a list of installed nanoCAD versions by scanning the registry
        /// </summary>
        /// <returns>List of version strings (e.g. "23.1", "24.1")</returns>
        public static List<string> GetInstalledNanoCADVersions()
        {
            var versions = new List<string>();
            
            try
            {
                // Open the registry key for nanoCAD installations
                using (var baseKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
                using (var nanoCadKey = baseKey.OpenSubKey(NANO_CAD_REGISTRY_PATH))
                {
                    if (nanoCadKey != null)
                    {
                        // Get all subkeys which represent the versions
                        foreach (var versionName in nanoCadKey.GetSubKeyNames())
                        {
                            versions.Add(versionName);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                Console.WriteLine($"Error reading nanoCAD versions from registry: {ex.Message}");
            }

            return versions;
        }
    }
}