using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using Autodesk.Navisworks.Api.Plugins;

namespace NwPluginManager
{
    public class PluginLoader
    {

        public void HookAssemblyResolve()
        {
            AppDomain.CurrentDomain.AssemblyResolve += this.CurrentDomain_AssemblyResolve;
        }

        public void UnhookAssemblyResolve()
        {
            AppDomain.CurrentDomain.AssemblyResolve -= this.CurrentDomain_AssemblyResolve;
        }

        private Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            AssemblyName assemblyName = new AssemblyName(args.Name);
            return null;
        }


        public Assembly LoadAddinsToTempFolder(string filePath)
        {
            if (string.IsNullOrEmpty(filePath) || filePath.StartsWith("\\") || !File.Exists(filePath))
            {
                return null;
            }
            StringBuilder stringBuilder = new StringBuilder(Path.GetFileNameWithoutExtension(filePath));
            string tempFolder = FileUtils.CreateTempFolder(stringBuilder.ToString());
            Assembly assembly = this.CopyAndLoadAddin(filePath, tempFolder);
            
            return assembly;
        }

        private Assembly CopyAndLoadAddin(string srcFilePath, string tempFolder)
        {
            string text = string.Empty;
            if (!FileUtils.FileExistsInFolder(srcFilePath, tempFolder))
            {
                List<FileInfo> list = new List<FileInfo>();
                text = FileUtils.CopyFileToFolder(srcFilePath, tempFolder, list);
                if (string.IsNullOrEmpty(text))
                {
                    return null;
                }
            }
            return this.LoadAddin(text);
        }

        private Assembly LoadAddin(string filePath)
        {
            Assembly result = null;
            try
            {
                Monitor.Enter(this);
                result = Assembly.LoadFile(filePath);
            }
            finally
            {
                Monitor.Exit(this);
            }
            return result;
        }

        public List<IPlugin> LoadItems(Assembly assembly, string originalAssemblyFilePath)
        {
            List<IPlugin> list = new List<IPlugin>();
            Type[] array = null;
            try
            {
                array = assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException ex)
            {
                array = ex.Types;
                if (array == null)
                {
                    return list;
                }
            }
            foreach (Type type in array)
            {
                try
                {
                    if (!(null == type) && type.IsSubclassOf(typeof(AddInPlugin)))
                    {
                        string name = string.Empty;
                        Attribute[] customAttributes = Attribute.GetCustomAttributes(type, false);
                        foreach (Attribute attribute in customAttributes)
                        {
                            if (attribute is PluginAttribute pluginAttribute)
                            {
                                name = string.IsNullOrEmpty(pluginAttribute.DisplayName) ? pluginAttribute.Name : pluginAttribute.DisplayName;
                            }
                        }
                        IPlugin item = new NwAddinPlugin()
                        {
                            Name = name,
                            ClassName = type.FullName,
                            AssemblyPath = assembly.Location
                        };
                        list.Add(item);
                    }
                }
                catch (Exception)
                {
                }
            }
            return list;
        }

    }
}