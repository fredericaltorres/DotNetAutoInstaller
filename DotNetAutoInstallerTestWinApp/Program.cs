using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DotNetAutoInstaller;

namespace DotNetAutoInstallerTestWinApp
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {          
            //System.Diagnostics.Debugger.Break();

            new AutoInstaller(Locations.LocalFolder)
                .CopyToProgramFiles()
                .DeployAssemblies("Newtonsoft.Json.dll", "DynamicSugar.dll")
                .DeployFiles("DotNetAutoInstallerTestWinApp.exe.config")
                .SetDataSubFolder("Help").DeployFiles(@"Help.markdown")
                .CreateShortcutToDesktop()
                .Finish();

            /*
            new AutoInstaller(Locations.LocalFolder)
                .DeployAssemblies("Newtonsoft.Json.dll", "DynamicSugar.dll")
                .DeployFiles(Locations.LocalFolder, "DotNetAutoInstallerTestWinApp.exe.config")
                .SetDataSubFolder("Help").DeployFiles(@"Help.markdown")
                .CreateShortcutToDesktop()
                .Finish();
             */
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
