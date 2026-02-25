using System;
using System.Windows.Forms;

namespace NanoCADModuleManager
{
    public class Program
    {
        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            var form = new ModuleManagerForm();
            var mainModuleManager = new MainModuleManager(form);
            mainModuleManager.Initialize();

            Application.Run(form);
        }
    }
}

