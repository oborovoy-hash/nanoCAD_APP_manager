using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace NanoCADModuleManager
{
    public class IniFileParser
    {
        public Dictionary<string, Dictionary<string, string>> Sections { get; private set; } = new Dictionary<string, Dictionary<string, string>>();

        public IniFileParser(string filePath)
        {
            LoadFromFile(filePath);
        }

        public void LoadFromFile(string filePath)
        {
            Sections.Clear();
            string currentSection = "";

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"INI file not found: {filePath}");
            }

            string[] lines = File.ReadAllLines(filePath);

            foreach (string line in lines)
            {
                string trimmedLine = line.Trim();

                // Skip empty lines and comments
                if (string.IsNullOrEmpty(trimmedLine) || trimmedLine.StartsWith(";") || trimmedLine.StartsWith("#"))
                {
                    continue;
                }

                // Check for section headers
                Match sectionMatch = Regex.Match(trimmedLine, @"^\[(.*)\]$");
                if (sectionMatch.Success)
                {
                    currentSection = sectionMatch.Groups[1].Value;
                    if (!Sections.ContainsKey(currentSection))
                    {
                        Sections[currentSection] = new Dictionary<string, string>();
                    }
                    continue;
                }

                // Parse key-value pairs
                if (currentSection != "" && trimmedLine.Contains("="))
                {
                    int separatorIndex = trimmedLine.IndexOf('=');
                    string key = trimmedLine.Substring(0, separatorIndex).Trim();
                    string value = trimmedLine.Substring(separatorIndex + 1).Trim();

                    // Remove type prefixes (s for string, i for integer, etc.)
                    if (value.Length > 1 && value.StartsWith("s"))
                    {
                        value = value.Substring(1); // Remove 's' prefix
                    }
                    else if (value.Length > 1 && value.StartsWith("i"))
                    {
                        value = value.Substring(1); // Remove 'i' prefix
                    }
                    else if (value.Length > 1 && value.StartsWith("f"))
                    {
                        value = value.Substring(1); // Remove 'f' prefix
                    }
                    
                    Sections[currentSection][key] = value;
                }
            }
        }

        public List<string> GetConfigurationNames()
        {
            var configNames = new List<string>();
            
            foreach (var section in Sections.Keys)
            {
                // Look for sections that start with \Configuration\ but don't contain nested paths like \Appload or \Startup
                if (section.StartsWith("\\Configuration\\"))
                {
                    // Extract just the configuration name part after \Configuration\
                    string configPath = section.Substring("\\Configuration\\".Length);
                    
                    // Only include if it doesn't contain further nesting like \Appload or \Startup
                    if (!configPath.Contains("\\Appload") && !configPath.Contains("\\Startup"))
                    {
                        // Get just the first part which is the config name
                        string configName = configPath.Split(new char[] { '\\' }, 2)[0];
                        
                        if (!configNames.Contains(configName))
                        {
                            configNames.Add(configName);
                        }
                    }
                }
            }
            
            // Add the default configuration if it exists
            if (!configNames.Contains("<<Default>>"))
            {
                // Check if we have any configuration sections that might indicate a default
                foreach (var section in Sections.Keys)
                {
                    if (section == "\\Configuration")
                    {
                        configNames.Add("<<Default>>");
                        break;
                    }
                }
            }
            
            return configNames;
        }

        public bool ContainsSection(string sectionName)
        {
            return Sections.ContainsKey(sectionName);
        }

        public string GetValue(string sectionName, string key)
        {
            if (Sections.ContainsKey(sectionName) && Sections[sectionName].ContainsKey(key))
            {
                return Sections[sectionName][key];
            }
            return "";
        }
    }
}