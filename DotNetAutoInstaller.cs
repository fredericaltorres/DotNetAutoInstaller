/*
    DotNetAutoInstaller
  
    Copyright (c) 2013 Frederic Torres

    MIT License:
    ============
        Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation 
        files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, 
        modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software 
        is furnished to do so, subject to the following conditions:

        The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

        THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES 
        OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE 
        LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR 
        IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

    
    DotNetAutoInstaller.cs is a C# class to create auto installable C# Console and Windows application.
    See Readme.MARKDOWN

*/
using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Collections;
#if !MONOTOUCH
using System.Dynamic;
#endif
using System.Reflection;
using System.Drawing;
using System.Diagnostics;

namespace DotNetAutoInstaller
{
    /// <summary>
    /// Comes from the DynamicSugar.net library, to avoid any dependencies in DotNetAutoInstaller,
    /// we include this code.
    /// </summary>
    internal static class ExtensionMethods_Format
    {
        /// <summary>
        ///  Replaces the format item in the string with the string representation
        ///  of a corresponding object in a specified array.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="args">An object array that contains zero or more objects to format</param>
        /// <returns>A copy of format in which the format items have been replaced by the string
        /// representation of the corresponding objects in args.
        /// </returns>
        internal static string format(this string s, params object[] args)
        {
            return string.Format(s, args);
        }
    }
    /// <summary>
    /// Dynamic Sharp Helper Class, dedicated methods to work with text resource file
    /// Comes from the DynamicSugar.net library, to avoid any dependencies in DotNetAutoInstaller,
    /// we include this code.
    /// </summary>
    internal static partial class DS
    {
        internal static class Resources
        {
            /// <summary>
            /// Return the fully qualified name of the resource file
            /// </summary>
            /// <param name="resourceFileName">File name of the resource</param>
            /// <returns></returns>
            private static string GetResourceFullName(string resourceFileName, Assembly assembly)
            {
                foreach (var resource in assembly.GetManifestResourceNames())
                    if (resource.EndsWith("." + resourceFileName))
                        return resource;
                throw new System.ApplicationException("Resource '{0}' not find in assembly '{1}'".format(resourceFileName, Assembly.GetExecutingAssembly().FullName));
            }
            /// <summary>
            /// Return the content of a text file embed as a resource.
            /// The function takes care of finding the fully qualify name, in the current
            /// assembly.
            /// </summary>
            /// <param name="resourceFileName">The file name of the resource</param>
            /// <returns></returns>
            internal static string GetTextResource(string resourceFileName, Assembly assembly)
            {
                var resourceFullName = GetResourceFullName(resourceFileName, assembly);

                using (var _textStreamReader = new StreamReader(assembly.GetManifestResourceStream(resourceFullName)))
                    return _textStreamReader.ReadToEnd();
            }
            /// <summary>
            /// Return the content of a file embed as a resource.
            /// The function takes care of finding the fully qualify name, in the current
            /// assembly.
            /// </summary>
            /// <param name="resourceFileName"></param>
            /// <param name="assembly"></param>
            /// <returns></returns>
            internal static byte[] GetBinaryResource(string resourceFileName, Assembly assembly)
            {
                var resourceFullName = GetResourceFullName(resourceFileName, assembly);
                var stream = assembly.GetManifestResourceStream(resourceFullName);
                byte[] data = new Byte[stream.Length];
                stream.Read(data, 0, (int)stream.Length);
                return data;
            }
#if !MONOTOUCH
            /// <summary>
            /// Return a image embed as a resource.
            /// The function takes care of finding the fully qualify name, in the current
            /// assembly.
            /// </summary>
            internal static Bitmap GetBitmapResource(string resourceFileName, Assembly assembly)
            {
                return ByteArrayToBitMap(GetBinaryResource(resourceFileName, assembly));
            }
            /// <summary>
            /// Convert a byte array into a bitmap
            /// </summary>
            /// <param name="b"></param>
            /// <returns></returns>
            private static Bitmap ByteArrayToBitMap(byte[] b)
            {
                MemoryStream ms = new MemoryStream(b);
                var img = System.Drawing.Image.FromStream(ms);
                return img as System.Drawing.Bitmap;
            }
#endif
            /// <summary>
            /// Save a buffer of byte into a file
            /// </summary>
            /// <param name="byteArray"></param>
            /// <param name="fileName"></param>
            /// <returns></returns>
            private static bool SaveByteArrayToFile(byte[] byteArray, string fileName)
            {
                try
                {
                    using (Stream fileStream = File.Create(fileName))
                    {
                        fileStream.Write(byteArray, 0, byteArray.Length);
                        fileStream.Close();
                        return true;
                    }
                }
                catch
                {
                    return false;
                }
            }
            /// <summary>
            /// Save resources as a local files
            /// </summary>
            /// <param name="assembly">Assembly where to get the resource</param>
            /// <param name="path">Local folder</param>
            /// <param name="resourceFileNames">Resource name and filename</param>
            /// <returns></returns>
            internal static Dictionary<string, string> SaveBinaryResourceAsFiles(Assembly assembly, string path, params string[] resourceFileNames)
            {
                var dic = new Dictionary<string, string>();

                foreach (var r in resourceFileNames)
                    dic[r] = SaveBinaryResourceAsFile(assembly, path, r);

                return dic;
            }
            /// <summary>
            /// Save a resource as a local file
            /// </summary>
            /// <param name="resourceFileName">Resource name and filename</param>
            /// <param name="assembly">Assembly where to get the resource</param>
            /// <param name="path">Local folder</param>
            /// <returns></returns>
            internal static string SaveBinaryResourceAsFile(Assembly assembly, string path, string resourceFileName)
            {
                var outputFileName = String.Format(@"{0}\{1}", path, resourceFileName);
                if (System.IO.File.Exists(outputFileName))
                    return outputFileName;

                if (!System.IO.Directory.Exists(path))
                    System.IO.Directory.CreateDirectory(path);

                var buffer = GetBinaryResource(resourceFileName, assembly);

                SaveByteArrayToFile(buffer, outputFileName);
                return outputFileName;
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    internal class AutoInstaller
    {
        public enum AssemblyLocation
        {
            LocalFolder,
            TempFolder
        };
        private string GetExecutable()
        {
            return Assembly.GetExecutingAssembly().Location;
        }
        private string GetExecutableVersion()
        {
            Version v = Assembly.GetEntryAssembly().GetName().Version;
            var vs = "{0}-{1}-{2}-{3}-{4}".format(v.Major, v.Minor, v.MajorRevision, v.MinorRevision, v.Build);
            return vs;
        }
        private string GetInstallInfoFile()
        {
            var a = Assembly.GetExecutingAssembly();
            return "{0}.{1}.json".format(a.Location, this.GetExecutableVersion());
        }
        private bool IsFirstExecution()
        {
            return !System.IO.File.Exists(GetInstallInfoFile());
        }
        const string InstallInfoJsonTemplate = @"{{ 
            ""InstallTime"" : ""{0}"",
            ""Username"" : ""{1}"",
            ""Machine"" : ""{2}""
        }}";
        private void SaveInstallInfoFile()
        {
            System.IO.File.WriteAllText(GetInstallInfoFile(), InstallInfoJsonTemplate.format(DateTime.UtcNow, Environment.UserName, Environment.MachineName));
        }
        private string GetAssemblyPath()
        {
            return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        }
        public AutoInstaller DeployAssemblies(AssemblyLocation assemblyLocation, params string[] assemblyFilenames)
        {
            DS.Resources.SaveBinaryResourceAsFiles(Assembly.GetExecutingAssembly(), GetAssemblyPath(), assemblyFilenames);
            return this;
        }
        public AutoInstaller DeployAssemblies(params string[] assemblyFilenames)
        {
            DeployAssemblies(AssemblyLocation.LocalFolder, assemblyFilenames);
            return this;
        }
        public AutoInstaller DeployTextFile(params string[] textFiles)
        {
            DS.Resources.SaveBinaryResourceAsFiles(Assembly.GetExecutingAssembly(), GetAssemblyPath(), textFiles);
            return this;
        }
        public AutoInstaller RebootOnFirstExecution(int exitCode = 0)
        {
            if (IsFirstExecution())
            {
                this.SaveInstallInfoFile();
                var p = ExecuteProgram(this.GetExecutable(), System.Environment.CommandLine, false, false);
                System.Environment.Exit(exitCode);
            }
            return this;
        }
        private static Process ExecuteProgram(string program, string commandLine, Boolean minimize = false, bool wait = false)
        {
            try
            {
                var path                            = System.IO.Path.GetDirectoryName(program);
                var processStartInfo                = new ProcessStartInfo(program, commandLine);
                processStartInfo.ErrorDialog        = false;
                processStartInfo.UseShellExecute    = true;
                processStartInfo.WorkingDirectory   = path;
                //processStartInfo.CreateNoWindow   = true;
                processStartInfo.WindowStyle        = minimize ? ProcessWindowStyle.Minimized : ProcessWindowStyle.Normal;
                var process                         = new Process();
                process.StartInfo                   = processStartInfo;
                bool processStarted                 = process.Start();

                if (wait)
                    process.WaitForExit();

                return process;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
