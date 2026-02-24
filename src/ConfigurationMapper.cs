using System;
using System.Collections.Generic;
using System.IO;

namespace NanoCADModuleManager
{
    public class ConfigurationInfo
    {
        public string TechnicalName { get; set; }
        public string DisplayName { get; set; }
        public string IconPath { get; set; }
        
        public ConfigurationInfo(string technicalName)
        {
            TechnicalName = technicalName;
            DisplayName = technicalName; // Default to technical name if no mapping found
            IconPath = "Resources/default.ico"; // Default icon
        }
    }

    public static class ConfigurationMapper
    {
        private static readonly Dictionary<string, ConfigurationInfo> _configMap = new Dictionary<string, ConfigurationInfo>();
        private static string _settingsFilePath = "settings.ini";

        static ConfigurationMapper()
        {
            LoadSettings();
        }

        public static void SetSettingsFilePath(string path)
        {
            _settingsFilePath = path;
            LoadSettings();
        }

        private static void LoadSettings()
        {
            _configMap.Clear();

            if (!File.Exists(_settingsFilePath))
            {
                // If settings file doesn't exist, we'll work with default values
                return;
            }

            var parser = new IniFileParser(_settingsFilePath);

            foreach (var section in parser.Sections.Keys)
            {
                // Section names correspond to configuration technical names (like AEC, Mech, etc.)
                if (!string.IsNullOrEmpty(section) && !section.StartsWith(";") && !section.StartsWith("["))
                {
                    string displayName = parser.GetValue(section, "name");
                    string iconPath = parser.GetValue(section, "ico");

                    var configInfo = new ConfigurationInfo(section);
                    
                    if (!string.IsNullOrEmpty(displayName))
                    {
                        configInfo.DisplayName = displayName;
                    }
                    
                    if (!string.IsNullOrEmpty(iconPath))
                    {
                        configInfo.IconPath = iconPath;
                    }
                    
                    _configMap[section] = configInfo;
                }
            }
        }

        public static ConfigurationInfo GetConfigurationInfo(string technicalName)
        {
            if (_configMap.ContainsKey(technicalName))
            {
                return _configMap[technicalName];
            }
            
            // Return default configuration info if no mapping exists
            return new ConfigurationInfo(technicalName);
        }

        public static List<ConfigurationInfo> GetAllConfigurationInfos(List<string> technicalNames)
        {
            var result = new List<ConfigurationInfo>();
            
            foreach (var techName in technicalNames)
            {
                result.Add(GetConfigurationInfo(techName));
            }
            
            return result;
        }
    }
}