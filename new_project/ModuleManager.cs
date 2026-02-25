using System;
using System.IO;

namespace NanoCADModuleManager
{
    public class ModuleManager
    {
        public void EnableModule(Module module)
        {
            var disabledPath = module.Path + ".disabled";
            
            if (File.Exists(disabledPath))
            {
                File.Delete(disabledPath);
            }
        }
        
        public void DisableModule(Module module)
        {
            var disabledPath = module.Path + ".disabled";
            
            if (!File.Exists(disabledPath))
            {
                File.Create(disabledPath).Dispose();
            }
        }
    }
}