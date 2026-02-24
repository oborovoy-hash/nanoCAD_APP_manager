using System.Collections.Generic;
using System.Linq;

namespace KpblcNCadCfgIni.Data
{
    public class Section
    {
        public Section()
        {
            Data = new List<KeyValue>();
        }
        public string Name { get; set; }
        public List<KeyValue> Data { get; set; }

        public List<string> GetKeys()
        {
            return Data.Select(kvp => kvp.Key).ToList();
        }

        public object GetValue(string key)
        {
            var keyValue = Data.FirstOrDefault(kvp => kvp.Key.Equals(key, StringComparison.OrdinalIgnoreCase));
            return keyValue.Value;
        }

        public bool IsKeyExists(string key)
        {
            return Data.Exists(kvp => kvp.Key.Equals(key, StringComparison.OrdinalIgnoreCase));
        }

        public void SetValue(string key, object value)
        {
            // Check if key already exists
            var existingKey = Data.FindIndex(kvp => kvp.Key.Equals(key, StringComparison.OrdinalIgnoreCase));
            if (existingKey >= 0)
            {
                // Update existing key-value pair
                Data[existingKey] = new KeyValue(key, value.ToString());
            }
            else
            {
                // Add new key-value pair
                Data.Add(new KeyValue(key, value.ToString()));
            }
        }

        internal List<string> PrepareToSave()
        {
            List<string> res = new List<string>
            {
                $"[{Name}]"
            };

            if (Data.Any())
            {
                res.AddRange(Data.Select(o => o.ToString()));
            }

            return res;
        }
    }
}
