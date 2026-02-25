using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace NanoCADModuleManager
{
    public class Module
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public bool IsEnabled { get; set; }
        
        public Module(string name, string path, bool isEnabled)
        {
            Name = name;
            Path = path;
            IsEnabled = isEnabled;
        }
        
        public override string ToString()
        {
            return $"{Name} ({(IsEnabled ? "Enabled" : "Disabled")})";
        }
    }

    public class ModuleRepository
    {
        private readonly string _modulesDirectory;
        
        public ModuleRepository()
        {
            // Default nanoCAD modules directory - adjust as needed
            _modulesDirectory = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "nanoCAD", 
                "Modules"
            );
            
            // Create directory if it doesn't exist
            if (!Directory.Exists(_modulesDirectory))
            {
                Directory.CreateDirectory(_modulesDirectory);
            }
        }
        
        public List<Module> GetModules()
        {
            var modules = new List<Module>();
            
            if (!Directory.Exists(_modulesDirectory))
            {
                return modules;
            }
            
            var dllFiles = Directory.GetFiles(_modulesDirectory, "*.dll");
            
            foreach (var dllFile in dllFiles)
            {
                var fileName = Path.GetFileName(dllFile);
                var isEnabled = !IsModuleDisabled(dllFile);
                modules.Add(new Module(fileName, dllFile, isEnabled));
            }
            
            return modules;
        }
        
        private bool IsModuleDisabled(string modulePath)
        {
            var disabledPath = modulePath + ".disabled";
            return File.Exists(disabledPath);
        }
    }
}