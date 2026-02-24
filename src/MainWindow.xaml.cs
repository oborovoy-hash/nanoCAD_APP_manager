using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NanoCADModuleManager;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private ModuleRepository _moduleRepository;
    private ModuleManager _moduleManager;
    private string _selectedVersion;
    private string _selectedConfig;
    
    public MainWindow()
    {
        InitializeComponent();
        InitializeApplication();
    }

    private void InitializeApplication()
    {
        // Initialize module repository
        _moduleRepository = new ModuleRepository();
        
        // Load nanoCAD versions into the left panel
        LoadNanoCADVersions();
    }

    private void LoadNanoCADVersions()
    {
        var versions = NanoCADVersionsHelper.GetInstalledNanoCADVersions();
        
        VersionsListBox.ItemsSource = versions;
    }

    private void VersionsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (VersionsListBox.SelectedItem is string selectedVersion)
        {
            _selectedVersion = selectedVersion;
            LoadModulesForVersion(selectedVersion);
        }
    }

    private void LoadModulesForVersion(string version)
    {
        // Construct the path to the config file for this version
        string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        string configPath = System.IO.Path.Combine(appDataPath, $"Nanosoft\\nanoCAD x64 {version}\\Config\\cfg.ini");

        // Initialize the module manager for this config file
        _moduleManager = new ModuleManager(configPath);

        // Parse the configuration file to get available configurations
        if (System.IO.File.Exists(configPath))
        {
            var iniParser = new IniFileParser(configPath);
            var configNames = iniParser.GetConfigurationNames();
            
            // For demonstration purposes, we'll use the first configuration
            // In a real implementation, you might want to let the user select one
            _selectedConfig = configNames.Count > 0 ? configNames[0] : "<<Default>>";
            
            // Get modules that are compatible with these configurations
            var modules = _moduleRepository.GetModulesForConfigs(configNames);
            
            // Update the modules with their load status
            foreach (var module in modules)
            {
                module.IsLoaded = _moduleManager.IsModuleLoaded(_selectedConfig, module.PackagePath);
            }
            
            // Update the UI with the modules
            ModulesDataGrid.ItemsSource = modules;
        }
        else
        {
            // Handle case where config file doesn't exist
            MessageBox.Show($"Configuration file not found for nanoCAD version {version} at {configPath}", 
                          "Configuration Not Found", MessageBoxButton.OK, MessageBoxImage.Warning);
            
            // Show empty list
            ModulesDataGrid.ItemsSource = new List<ModuleInfo>();
        }
    }
    
    private void ModulesDataGrid_CellEditEnding(object? sender, DataGridCellEditEndingEventArgs e)
    {
        if (e.Row.Item is ModuleInfo module)
        {
            if (_moduleManager != null && !string.IsNullOrEmpty(_selectedConfig))
            {
                if (module.IsLoaded)
                {
                    // Module should be loaded
                    _moduleManager.AddModule(_selectedConfig, module.PackagePath, module.Name);
                }
                else
                {
                    // Module should be unloaded
                    _moduleManager.RemoveModule(_selectedConfig, module.PackagePath);
                }
            }
        }
    }
}