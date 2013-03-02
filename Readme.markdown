DotNetAutoInstaller
===================

# Overview
DotNetAutoInstaller.cs is a C# class to create auto installable C# Console and
Windows application.

By simply executing the application the first time, the application auto extract
assemblies dependencies and other files, including the application config file.

# Syntax

        [STAThread]
        static void Main()  
        {          
           new  AutoInstaller()
                .SetAssemblyLocation(Locations.ApplicationData)
                .SetDataLocation(Locations.ApplicationData)
                .DeployAssemblies("Newtonsoft.Json.dll", "DynamicSugar.dll")
                .DeployFiles(Locations.LocalFolder, "DotNetAutoInstallerTestWinApp.exe.config")
                .SetDataSubFolder("Help")
                .DeployFiles(@"Help.markdown")
                .Finish();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

# Road map
- Create entry in start menu
- Installation of the application in C:\Program Files (x86)
    Automatically switch to elevated privilege
- Support NT Service with auto registration on the first execution.

# License (MIT License)
DotNetAutoInstaller
Copyright (c) 2013 Frederic Torres

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
