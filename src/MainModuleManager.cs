using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NanoCADModuleManager
{
    public class MainModuleManager
    {
        private readonly string _settingsPath = "settings.ini";
        private readonly Dictionary<string, List<ModuleManagerForm.ModuleModel>> _modulesByVersion = new Dictionary<string, List<ModuleManagerForm.ModuleModel>>();
        private readonly string _iniParserFilePath = "settings.ini";
        private readonly string _configMapperFilePath = "settings.ini";
        private ModuleManagerForm _form;

        public MainModuleManager(ModuleManagerForm form)
        {
            _form = form;
            _form.OnVersionSelected += HandleVersionSelected;
            _form.OnModuleToggled += HandleModuleToggled;
            LoadSettings();
        }

        private IniFileParser GetIniParser()
        {
            return new IniFileParser(_iniParserFilePath);
        }

        private List<string> GetCompatibleConfigs(string cfgPath)
        {
            var parser = new IniFileParser(cfgPath);
            return parser.GetConfigurationNames();
        }

        public void Initialize()
        {
            var versions = GetAvailableVersions();
            _form.SetVersions(versions);
        }

        private void LoadSettings()
        {
            if (!File.Exists(_settingsPath))
            {
                throw new FileNotFoundException($"Settings file not found: {_settingsPath}");
            }

            var parser = GetIniParser();
            parser.LoadFromFile(_settingsPath);
            var settings = parser.Sections;
            if (!settings.ContainsKey("Paths") || !settings["Paths"].ContainsKey("BasePath"))
            {
                throw new InvalidOperationException("BasePath not defined in settings.ini");
            }

            string basePath = settings["Paths"]["BasePath"];
            LoadModulesFromDirectory(basePath);
        }

        private void LoadModulesFromDirectory(string basePath)
        {
            if (!Directory.Exists(basePath))
            {
                throw new DirectoryNotFoundException($"Base directory not found: {basePath}");
            }

            foreach (var dir in Directory.GetDirectories(basePath))
            {
                string version = Path.GetFileName(dir);
                var modules = LoadModulesForVersion(dir);
                if (modules.Any())
                {
                    _modulesByVersion[version] = modules;
                }
            }
        }

        private List<ModuleManagerForm.ModuleModel> LoadModulesForVersion(string versionPath)
        {
            var modules = new List<ModuleManagerForm.ModuleModel>();
            string[] cfgFiles = Directory.GetFiles(versionPath, "*.cfg", SearchOption.AllDirectories);

            foreach (string cfgPath in cfgFiles)
            {
            var iniParser = new IniFileParser(cfgPath);
            iniParser.LoadFromFile(cfgPath);
            var iniDataDict = iniParser.Sections;
            var iniData = iniParser.Sections;
            if (iniDataDict.ContainsKey("StartUp") && iniDataDict["StartUp"].ContainsKey("ModuleName"))
                {
                    string moduleName = iniData["StartUp"]["ModuleName"];
                    string description = iniData["StartUp"].ContainsKey("Description") 
                        ? iniData["StartUp"]["Description"] 
                        : "";
                    
                    // Find corresponding .ncm file to check load status
                    string ncmFileName = moduleName + ".ncm";
                    bool isLoaded = false;
                    string ncmFilePath = FindNcmFile(Path.GetDirectoryName(cfgPath), ncmFileName);
                    if (!string.IsNullOrEmpty(ncmFilePath))
                    {
                        KpblcNCadCfgIni.NCadConfig ncadConfig = new KpblcNCadCfgIni.NCadConfig(ncmFilePath);
                        isLoaded = ncadConfig.IsModuleLoaded(moduleName);
                    }

                    var compatibleConfigs = GetCompatibleConfigs(cfgPath);

                    modules.Add(new ModuleManagerForm.ModuleModel
                    {
                        Name = moduleName,
                        Description = description,
                        IsLoaded = isLoaded,
                        CompatibleConfigs = compatibleConfigs
                    });
                }
            }

            return modules.Distinct().ToList();
        }

        private string FindNcmFile(string startPath, string fileName)
        {
            try
            {
                foreach (var file in Directory.GetFiles(startPath, fileName, SearchOption.AllDirectories))
                {
                    return file;
                }
            }
            catch (UnauthorizedAccessException)
            {
                // Skip inaccessible directories
            }

            return null;
        }

        public List<string> GetAvailableVersions()
        {
            return _modulesByVersion.Keys.ToList();
        }

        public List<ModuleManagerForm.ModuleModel> GetModulesByVersion(string version)
        {
            if (_modulesByVersion.ContainsKey(version))
            {
                return _modulesByVersion[version];
            }
            return new List<ModuleManagerForm.ModuleModel>();
        }

        public void UpdateModuleStatus(string version, string moduleName, bool loadStatus)
        {
            if (_modulesByVersion.ContainsKey(version))
            {
                var module = _modulesByVersion[version].FirstOrDefault(m => m.Name == moduleName);
                if (module != null)
                {
                    string ncmFileName = moduleName + ".ncm";
                    string ncmFilePath = FindNcmFile(Path.GetDirectoryName(version), ncmFileName);

                    if (!string.IsNullOrEmpty(ncmFilePath))
                    {
                        KpblcNCadCfgIni.NCadConfig ncadConfig = new KpblcNCadCfgIni.NCadConfig(ncmFilePath);

                        if (loadStatus)
                        {
                            ncadConfig.LoadModule(moduleName, module.Name + ".cfg"); // Using module name as config path placeholder
                        }
                        else
                        {
                            ncadConfig.UnloadModule(moduleName);
                        }
                        ncadConfig.Save();
                    }
                }
            }
        }

        private void HandleVersionSelected(string version)
        {
            var modules = GetModulesByVersion(version);
            _form.SetModules(modules);
        }

        private void HandleModuleToggled(string moduleName, bool loadStatus)
        {
            // Access the selected version from the form
            var field = _form.GetType().GetField("versionsListBox", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (field != null)
            {
                var listBox = field.GetValue(_form) as System.Windows.Forms.ListBox;
                if (listBox != null && listBox.SelectedItem != null)
                {
                    string selectedVersion = listBox.SelectedItem.ToString();
                    UpdateModuleStatus(selectedVersion, moduleName, loadStatus);
                }
            }
        }
    }
}