using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;

namespace src
{
    public partial class Form1 : Form
    {
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ListBox listBoxVersions;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dataGridViewModules;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnModuleName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnConfigs;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnDescription;

        public Form1()
        {
            InitializeComponent();
        }

        // Функция для получения списка установленных версий nanoCAD
        public List<string> GetNanoCADVersions()
        {
            List<string> versions = new List<string>();
            
            try
            {
                // Открываем ветку реестра
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Nanosoft\nanoCAD x64"))
                {
                    if (key != null)
                    {
                        // Получаем все подключи (это будут версии)
                        string[] subKeys = key.GetSubKeyNames();
                        
                        foreach (string subKey in subKeys)
                        {
                            versions.Add(subKey);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при чтении реестра: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
            return versions;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadVersionsToUI();
        }

        private void LoadVersionsToUI()
        {
            List<string> versions = GetNanoCADVersions();
            listBoxVersions.Items.Clear();
            
            foreach (string version in versions)
            {
                listBoxVersions.Items.Add(version);
            }
            
            if (listBoxVersions.Items.Count > 0)
            {
                listBoxVersions.SelectedIndex = 0;
            }
        }

        private void listBoxVersions_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedVersion = listBoxVersions.SelectedItem?.ToString();
            if (!string.IsNullOrEmpty(selectedVersion))
            {
                LoadModulesForVersion(selectedVersion);
            }
        }

        // Функция для загрузки модулей для выбранной версии nanoCAD
        private void LoadModulesForVersion(string version)
        {
            // Очищаем таблицу перед заполнением
            dataGridViewModules.Rows.Clear();
            
            // Путь к файлу конфигурации
            string configPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + 
                               $@"\Nanosoft\nanoCAD x64 {version}\Config\cfg.ini";
            
            // Проверяем существование файла конфигурации
            if (!System.IO.File.Exists(configPath))
            {
                // Если файл не существует в пользовательской директории, проверяем примеры из WorkDir
                string examplePath23 = @"/workspace/WorkDir/Примеры CFG/Nanosoft/nanoCAD x64 23.1/Config/cfg.ini";
                string examplePath24 = @"/workspace/WorkDir/Примеры CFG/Nanosoft/nanoCAD x64 24.1/Config/cfg.ini";
                
                if (version.Contains("23")) 
                    configPath = examplePath23;
                else if (version.Contains("24")) 
                    configPath = examplePath24;
            }
            
            // Читаем конфигурации из файла
            List<string> configurations = ParseConfigurationsFromFile(configPath);
            
            // Пример данных - в реальности это будет получено из cfg файлов и сетевого репозитория
            var sampleModules = new[]
            {
                new { Name = "Пример модуля 1", Configs = string.Join(", ", configurations), Description = "Описание первого модуля" },
                new { Name = "Пример модуля 2", Configs = string.Join(", ", configurations.Take(2)), Description = "Описание второго модуля" },
                new { Name = "Пример модуля 3", Configs = string.Join(", ", configurations.Skip(1).Take(2)), Description = "Описание третьего модуля" }
            };

            foreach (var module in sampleModules)
            {
                int rowIndex = dataGridViewModules.Rows.Add();
                dataGridViewModules.Rows[rowIndex].Cells[0].Value = module.Name;
                dataGridViewModules.Rows[rowIndex].Cells[1].Value = module.Configs;
                dataGridViewModules.Rows[rowIndex].Cells[2].Value = module.Description;
            }
        }
        
        // Функция для парсинга конфигурационного файла
        private List<string> ParseConfigurationsFromFile(string configPath)
        {
            List<string> configurations = new List<string>();
            
            if (!System.IO.File.Exists(configPath))
            {
                return configurations;
            }
            
            try
            {
                string[] lines = System.IO.File.ReadAllLines(configPath);
                
                foreach (string line in lines)
                {
                    // Ищем строки, которые содержат секции конфигураций
                    // Формат: [\Configuration\... ]
                    if (line.StartsWith("[\\Configuration\\") && !line.Contains("\\Appload") && !line.Contains("\\Startup"))
                    {
                        // Извлекаем имя конфигурации
                        string configSection = line.TrimStart('[').TrimEnd(']').Trim();
                        if (configSection.StartsWith("\\Configuration\\"))
                        {
                            string configName = configSection.Substring("\\Configuration\\".Length);
                            
                            // Убираем все подпути (например, \Appload, \Startup)
                            if (!configName.Contains("\\")) // Только если это корневая конфигурация
                            {
                                configurations.Add(configName);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при чтении файла конфигурации: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
            return configurations;
        }
        
        // Функция для чтения настроек из settings.ini
        private Dictionary<string, Dictionary<string, string>> ReadSettings()
        {
            Dictionary<string, Dictionary<string, string>> settings = new Dictionary<string, Dictionary<string, string>>();
            string settingsPath = System.IO.Path.Combine(Application.StartupPath, "settings.ini");
            
            if (!System.IO.File.Exists(settingsPath))
            {
                // Если файла нет в папке приложения, создаем его из ресурсов
                string resourceSettingsPath = @"/workspace/src/settings.ini";
                if (System.IO.File.Exists(resourceSettingsPath))
                {
                    System.IO.File.Copy(resourceSettingsPath, settingsPath, true);
                }
                else
                {
                    return settings;
                }
            }
            
            try
            {
                string[] lines = System.IO.File.ReadAllLines(settingsPath);
                string currentSection = "";
                
                foreach (string line in lines)
                {
                    string trimmedLine = line.Trim();
                    
                    // Обработка секции
                    if (trimmedLine.StartsWith("[") && trimmedLine.EndsWith("]"))
                    {
                        currentSection = trimmedLine.Substring(1, trimmedLine.Length - 2);
                        if (!settings.ContainsKey(currentSection))
                        {
                            settings[currentSection] = new Dictionary<string, string>();
                        }
                    }
                    // Обработка параметров
                    else if (!string.IsNullOrEmpty(trimmedLine) && !trimmedLine.StartsWith(";") && currentSection != "")
                    {
                        string[] parts = trimmedLine.Split(new char[] { '=' }, 2);
                        if (parts.Length == 2)
                        {
                            string key = parts[0].Trim();
                            string value = parts[1].Trim();
                            settings[currentSection][key] = value;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при чтении файла настроек: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
            return settings;
        }
    }
}
