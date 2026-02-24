using KpblcNCadCfgIni;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NanoCADModuleManager
{
    public class ModuleManager
    {
        private readonly NCadConfig _ncadConfig;
        private readonly string _configFilePath;

        public ModuleManager(string configFilePath)
        {
            _configFilePath = configFilePath;
            _ncadConfig = new NCadConfig(configFilePath);
        }

        /// <summary>
        /// Checks if a module is currently loaded in the specified configuration
        /// </summary>
        public bool IsModuleLoaded(string configName, string modulePath)
        {
            string sectionPath = $"\\Configuration\\{configName}\\Appload\\Startup";
            
            if (!_ncadConfig.IsSectionExists(sectionPath))
            {
                return false;
            }

            // Iterate through all keys in the startup section to find the module
            var startupSection = _ncadConfig.GetSection(sectionPath);
            foreach (var key in startupSection.GetKeys())
            {
                var keyValue = startupSection.GetValue(key);
                if (keyValue.ToString().Equals(modulePath, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Adds a module to the specified configuration's auto-load list
        /// </summary>
        public void AddModule(string configName, string modulePath, string moduleName)
        {
            string sectionPath = $"\\Configuration\\{configName}\\Appload\\Startup";
            
            // Create the section if it doesn't exist
            if (!_ncadConfig.IsSectionExists(sectionPath))
            {
                _ncadConfig.CreateSection(sectionPath);
            }

            // Find the next available app index (app0, app1, etc.)
            var startupSection = _ncadConfig.GetSection(sectionPath);
            int nextIndex = 0;
            while (startupSection.IsKeyExists($"app{nextIndex}"))
            {
                nextIndex++;
            }

            string appKey = $"app{nextIndex}";
            
            // Create the subsection for this app
            var appSection = _ncadConfig.CreateSection($"{sectionPath}\\{appKey}");
            
            // Add the required properties
            appSection.SetValue("Application", moduleName);
            appSection.SetValue("Loader", modulePath);
            appSection.SetValue("Type", "APP_PACKAGE");
            appSection.SetValue("Enabled", 1);

            SaveChanges();
        }

        /// <summary>
        /// Removes a module from the specified configuration's auto-load list
        /// </summary>
        public void RemoveModule(string configName, string modulePath)
        {
            string sectionPath = $"\\Configuration\\{configName}\\Appload\\Startup";
            
            if (!_ncadConfig.IsSectionExists(sectionPath))
            {
                return; // Nothing to remove
            }

            // Find the app entry with the matching loader path
            var startupSection = _ncadConfig.GetSection(sectionPath);
            var keysToRemove = new List<string>();
            
            foreach (var key in startupSection.GetKeys())
            {
                // Check if this is an app entry (starts with "app")
                if (key.StartsWith("app"))
                {
                    var subSection = _ncadConfig.GetSection($"{sectionPath}\\{key}");
                    var loaderValue = subSection.GetValue("Loader")?.ToString();
                    
                    if (loaderValue?.Equals(modulePath, StringComparison.OrdinalIgnoreCase) == true)
                    {
                        keysToRemove.Add(key);
                    }
                }
            }

            // Remove the matching entries
            foreach (var key in keysToRemove)
            {
                _ncadConfig.DeleteSection($"{sectionPath}\\{key}");
            }

            if (keysToRemove.Count > 0)
            {
                SaveChanges();
            }
        }

        /// <summary>
        /// Gets all modules currently loaded in the specified configuration
        /// </summary>
        public List<ModuleInfo> GetLoadedModules(string configName)
        {
            var loadedModules = new List<ModuleInfo>();
            string sectionPath = $"\\Configuration\\{configName}\\Appload\\Startup";
            
            if (!_ncadConfig.IsSectionExists(sectionPath))
            {
                return loadedModules;
            }

            var startupSection = _ncadConfig.GetSection(sectionPath);
            foreach (var key in startupSection.GetKeys())
            {
                if (key.StartsWith("app"))
                {
                    var appSection = _ncadConfig.GetSection($"{sectionPath}\\{key}");
                    var loaderPath = appSection.GetValue("Loader")?.ToString();
                    var appName = appSection.GetValue("Application")?.ToString();
                    var isEnabled = Convert.ToInt32(appSection.GetValue("Enabled")) == 1;

                    if (!string.IsNullOrEmpty(loaderPath))
                    {
                        var moduleInfo = new ModuleInfo
                        {
                            PackagePath = loaderPath,
                            Name = appName ?? Path.GetFileNameWithoutExtension(loaderPath),
                            IsLoaded = isEnabled
                        };
                        
                        loadedModules.Add(moduleInfo);
                    }
                }
            }

            return loadedModules;
        }

        /// <summary>
        /// Saves all changes back to the configuration file
        /// </summary>
        public void SaveChanges()
        {
            _ncadConfig.SaveAs(_configFilePath);
        }
    }
}