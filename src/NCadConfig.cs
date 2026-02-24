using KpblcNCadCfgIni.Data;
using KpblcNCadCfgIni.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace KpblcNCadCfgIni
{
    public class NCadConfig
    {
        public NCadConfig(string ConfigIniFileName)
        {
            _iniFileName = ConfigIniFileName;

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            List<string> data = File.ReadAllLines(_iniFileName, Encoding.GetEncoding(1251))
                .ToList()
                .Select(o =>
                {
                    string value = o.Trim('\t');
                    if (value.StartsWith("["))
                    {
                        return value.Trim(' ');
                    }
                    return value;

                })
                .Where(o => !string.IsNullOrWhiteSpace(o))
                .ToList();

            ConfigurationList = new List<NCadConfiguration>(
                data.Where(o =>
                {
                    string name = o.Trim(new char[] { '\\', '[', ']' });
                    if (!name.ToUpper().StartsWith(_configurationHeader.ToUpper()))
                    {
                        return false;
                    }

                    var splitted = name.Split(new char[] { '\\' });
                    return splitted.Length < 3;
                }
                    )
                    .Select(o => new NCadConfiguration()
                    {
                        ConfigurationName = o.Trim(new char[] { '\\', '[', ']', ' ' }).Substring(_configurationHeader.Length).Trim('\\'),
                    }
                    )
                    .Distinct()
            );

            int pos = 0;

            while (pos < data.Count)
            {
                if (data[pos].Equals("[\\]"))
                {
                    UnnamedSection = new Section()
                    {
                        Name = "\\",
                    };
                    while (!data[pos + 1].StartsWith("["))
                    {
                        GetKeyValueFromString(data[pos + 1], out string key, out string value);
                        if (!string.IsNullOrWhiteSpace(key))
                        {
                            UnnamedSection.Data.Add(new KeyValue(key, value));
                        }
                        pos++;
                    }

                    pos--;
                }
                else if (GetSectionName(data[pos]).Equals("Inccfgdirs", StringComparison.InvariantCultureIgnoreCase))
                {
                    IncludeConfigurationFolders = new Section()
                    {
                        Name = GetSectionName(data[pos]),
                    };
                    while (!data[pos + 1].StartsWith("["))
                    {
                        GetKeyValueFromString(data[pos + 1], out string key, out string value);
                        if (!string.IsNullOrWhiteSpace(key))
                        {
                            IncludeConfigurationFolders.Data.Add(new KeyValue(key, value));
                        }
                        pos++;
                    }

                    pos--;
                }
                else if (GetSectionName(data[pos]).ToUpper().StartsWith(_configurationHeader.ToUpper()))
                {
                    string confName = GetConfigurationName(data[pos]);

                    if (confName.Contains('\\'))
                    {
                        string[] splitted = confName.Split('\\');
                        confName = splitted[0];
                    }

                    NCadConfiguration configuration = ConfigurationList.First(o =>
                        o.ConfigurationName.Equals(confName) || o.ConfigurationNameForAppload.Equals(confName));

                    if (!data[pos].ToUpper().Contains("APPLOAD"))
                    {
                        string key, value;
                        while (pos < (data.Count - 1) && !data[pos + 1].StartsWith("["))
                        {
                            GetKeyValueFromString(data[pos + 1], out key, out value);
                            if (key.Equals("cfgfile", StringComparison.InvariantCultureIgnoreCase))
                            {
                                configuration.CfgFileName = value.Substring(1);
                            }
                            else if (key.Equals("pgpfile", StringComparison.InvariantCultureIgnoreCase))
                            {
                                configuration.PgpFileName = value.Substring(1);
                            }
                            else if (key.Equals("nplat", StringComparison.InvariantCultureIgnoreCase))
                            {
                                configuration.Plat = value.Substring(1);
                            }

                            pos++;
                        }
                    }
                    else if (data[pos].ToUpper().Contains("APPLOAD") &&
                             Regex.IsMatch(data[pos], @"^(.*)app\d+\](.*)$", RegexOptions.CultureInvariant))
                    {
                        string sOrder = data[pos].Split('\\').Last().Trim('\\', 'a', 'p', ']');
                        int.TryParse(sOrder, out int order);

                        StartupApplication app = new StartupApplication()
                        {
                            LoadOrder = order,
                        };

                        string key, value;
                        while (pos < data.Count - 1 && !data[pos + 1].StartsWith("["))
                        {
                            GetKeyValueFromString(data[pos + 1], out key, out value);
                            if (key.Equals("Loader", StringComparison.InvariantCultureIgnoreCase))
                            {
                                app.LoaderName = value.Substring(1);
                            }
                            else if (key.Equals("Type", StringComparison.InvariantCultureIgnoreCase))
                            {
                                AppTypeEnum appType = AppTypeEnum.Unknown;
                                AppTypeEnum.TryParse(value.Substring(1), out appType);
                                app.AppType = appType;
                            }
                            else if (key.Equals("Enabled", StringComparison.InvariantCultureIgnoreCase))
                            {
                                app.Enabled = value.Substring(1) == "1";
                            }
                            pos++;
                        }

                        configuration.StartupApplicationList.Add(app);
                    }
                }

                pos++;
            }
        }

        /// <summary> Сохранение в файл </summary>
        public void Save()
        {
            List<string> saveList = new List<string>();
            saveList.AddRange(UnnamedSection.PrepareToSave());
            saveList.AddRange(IncludeConfigurationFolders.PrepareToSave());
            foreach (NCadConfiguration item in ConfigurationList)
            {
                saveList.AddRange(item.PrepareToSave().ToList());
            }

            File.WriteAllLines(_iniFileName, saveList.ToArray(), Encoding.GetEncoding(1251));
        }

        public Section UnnamedSection { get; private set; }
        public Section IncludeConfigurationFolders { get; private set; }

        public List<NCadConfiguration> ConfigurationList { get; private set; }

        public bool IsSectionExists(string sectionPath)
        {
            // Normalize the section path by removing leading/trailing slashes and brackets
            string normalizedPath = sectionPath.Replace("\\\\", "\\").Trim('\\');
            
            // Check if the section exists in the parsed sections
            return GetAllSections().Any(s => NormalizeSectionPath(s.Name).Equals(normalizedPath, System.StringComparison.OrdinalIgnoreCase));
        }

        public Section GetSection(string sectionPath)
        {
            // Normalize the section path by removing leading/trailing slashes and brackets
            string normalizedPath = sectionPath.Replace("\\\\", "\\").Trim('\\');
            
            // Find the section by normalized name
            var section = GetAllSections().FirstOrDefault(s => NormalizeSectionPath(s.Name).Equals(normalizedPath, System.StringComparison.OrdinalIgnoreCase));
            
            if (section != null)
            {
                return section;
            }
            
            // If not found, return null or throw exception based on requirements
            // For now, we'll create a new section if it doesn't exist to match typical behavior
            var newSection = new Section() { Name = normalizedPath };
            return newSection;
        }

        public Section CreateSection(string sectionPath)
        {
            // Normalize the section path
            string normalizedPath = sectionPath.Replace("\\\\", "\\").Trim('\\');
            
            // First check if it already exists
            var existingSection = GetSection(normalizedPath);
            if (existingSection != null)
            {
                return existingSection;
            }
            
            // Create a new section
            var newSection = new Section() { Name = normalizedPath };
            
            // Add to the appropriate collection based on the path
            if (normalizedPath.Equals("\\", System.StringComparison.OrdinalIgnoreCase))
            {
                UnnamedSection = newSection;
            }
            else if (normalizedPath.Equals("Inccfgdirs", System.StringComparison.OrdinalIgnoreCase))
            {
                IncludeConfigurationFolders = newSection;
            }
            else
            {
                // For configuration sections, we need to handle differently
                // This implementation might need to be adjusted based on actual structure needs
            }
            
            return newSection;
        }

        public void DeleteSection(string sectionPath)
        {
            // Normalize the section path
            string normalizedPath = sectionPath.Replace("\\\\", "\\").Trim('\\');
            
            // Remove from collections based on the path
            if (UnnamedSection != null && NormalizeSectionPath(UnnamedSection.Name).Equals(normalizedPath, System.StringComparison.OrdinalIgnoreCase))
            {
                UnnamedSection = null;
            }
            else if (IncludeConfigurationFolders != null && NormalizeSectionPath(IncludeConfigurationFolders.Name).Equals(normalizedPath, System.StringComparison.OrdinalIgnoreCase))
            {
                IncludeConfigurationFolders = null;
            }
            // For other sections, we would need to remove from ConfigurationList appropriately
        }

        public void SaveAs(string fileName)
        {
            List<string> saveList = new List<string>();
            
            if (UnnamedSection != null)
                saveList.AddRange(UnnamedSection.PrepareToSave());
                
            if (IncludeConfigurationFolders != null)
                saveList.AddRange(IncludeConfigurationFolders.PrepareToSave());
                
            foreach (NCadConfiguration item in ConfigurationList)
            {
                saveList.AddRange(item.PrepareToSave().ToList());
            }

            System.Text.Encoding encoding = System.Text.Encoding.GetEncoding(1251);
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            System.IO.File.WriteAllLines(fileName, saveList.ToArray(), encoding);
        }

        private IEnumerable<Section> GetAllSections()
        {
            var sections = new List<Section>();
            
            if (UnnamedSection != null)
                sections.Add(UnnamedSection);
                
            if (IncludeConfigurationFolders != null)
                sections.Add(IncludeConfigurationFolders);
            
            // Add configuration sections if needed
            foreach (var config in ConfigurationList)
            {
                // We need to add individual sections for each configuration
                // This might require parsing the configuration sections properly
            }
            
            return sections;
        }

        private string NormalizeSectionPath(string path)
        {
            if (string.IsNullOrEmpty(path))
                return string.Empty;
                
            return path.Trim('[', ']', '\\', '/');
        }

        private string GetSectionName(string Name)
        {
            return Name.Trim('\t', '[', ']', '\\');
        }

        private string GetConfigurationName(string Name)
        {
            return Name.Trim('\\', '[', ']', '\t').Substring(_configurationHeader.Length).Trim('\\');
        }

        private void GetKeyValueFromString(string Line, out string Key, out string Value)
        {
            if (!Line.Contains(_separator))
            {
                Key = string.Empty;
                Value = string.Empty;
                return;
            }

            int index = Line.IndexOf(_separator);
            Key = Line.Substring(0, index);
            Value = Line.Substring(index + 1);
        }

        private string _iniFileName;
        private readonly string _separator = "=";
        private readonly string _configurationHeader = "Configuration";
    }
}
