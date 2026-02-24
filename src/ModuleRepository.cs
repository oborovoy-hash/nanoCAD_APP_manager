using System;
using System.Collections.Generic;
using System.IO;

namespace NanoCADModuleManager
{
    public class ModuleInfo
    {
        public string FolderName { get; set; }
        public string PackagePath { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Version { get; set; }
        public List<string> CompatibleConfigs { get; set; } = new List<string>();
        public bool IsLoaded { get; set; } = false; // Whether this module is currently loaded in the selected nanoCAD config
    }

    public class ModuleRepository
    {
        private string _repositoryPath;
        private string _settingsFilePath = "settings.ini";
        private IniFileParser _settingsParser;

        public ModuleRepository()
        {
            LoadSettings();
        }

        private void LoadSettings()
        {
            _settingsParser = new IniFileParser(_settingsFilePath);
            _repositoryPath = _settingsParser.GetValue("General", "repository_path"); // Assuming settings.ini has a General section with repository_path
            
            // Fallback to default path if not specified
            if (string.IsNullOrEmpty(_repositoryPath))
            {
                _repositoryPath = @"\\server\shared\nanoCAD\Modules"; // Default path
            }
        }

        public void UpdateRepositoryPath(string newPath)
        {
            _repositoryPath = newPath;
        }

        public List<ModuleInfo> GetModulesForConfigs(List<string> configNames)
        {
            var modules = new List<ModuleInfo>();

            if (!Directory.Exists(_repositoryPath))
            {
                throw new DirectoryNotFoundException($"Module repository not found: {_repositoryPath}");
            }

            // Scan each folder in the repository
            foreach (var moduleDir in Directory.GetDirectories(_repositoryPath))
            {
                var dirInfo = new DirectoryInfo(moduleDir);
                string moduleName = dirInfo.Name;

                // Look for .package file in the directory
                var packageFiles = Directory.GetFiles(moduleDir, "*.package");
                
                if (packageFiles.Length > 0)
                {
                    string packagePath = packageFiles[0]; // Take the first .package file
                    
                    // Look for manifest file (info.ini or similar)
                    string manifestPath = Path.Combine(moduleDir, "info.ini");
                    if (!File.Exists(manifestPath))
                    {
                        // If info.ini doesn't exist, look for other common manifest names
                        var manifestFiles = Directory.GetFiles(moduleDir, "*.ini");
                        foreach (var iniFile in manifestFiles)
                        {
                            if (Path.GetFileName(iniFile).ToLower() != "settings.ini") // Exclude our own settings
                            {
                                manifestPath = iniFile;
                                break;
                            }
                        }
                    }

                    var moduleInfo = new ModuleInfo
                    {
                        FolderName = moduleName,
                        PackagePath = packagePath,
                        Name = moduleName // Default name to folder name
                    };

                    // Parse manifest file if it exists
                    if (File.Exists(manifestPath))
                    {
                        var manifestParser = new IniFileParser(manifestPath);
                        
                        // Extract module information from manifest
                        foreach (var section in manifestParser.Sections.Keys)
                        {
                            // Common section names for module info: Module, Info, General, etc.
                            if (section.ToLower() == "module" || section.ToLower() == "info" || section.ToLower() == "general")
                            {
                                moduleInfo.Name = manifestParser.GetValue(section, "name");
                                moduleInfo.Description = manifestParser.GetValue(section, "description");
                                moduleInfo.Version = manifestParser.GetValue(section, "version");
                                
                                // Parse compatible configurations
                                string configsStr = manifestParser.GetValue(section, "configs");
                                if (!string.IsNullOrEmpty(configsStr))
                                {
                                    string[] configs = configsStr.Split(',');
                                    foreach (string config in configs)
                                    {
                                        string trimmedConfig = config.Trim();
                                        if (!string.IsNullOrEmpty(trimmedConfig))
                                        {
                                            moduleInfo.CompatibleConfigs.Add(trimmedConfig);
                                        }
                                    }
                                }
                                
                                break; // Only process the first matching section
                            }
                        }
                    }

                    modules.Add(moduleInfo);
                }
            }

            return modules;
        }

        public List<ModuleInfo> GetModulesForConfig(string configName)
        {
            return GetModulesForConfigs(new List<string> { configName });
        }
    }
}