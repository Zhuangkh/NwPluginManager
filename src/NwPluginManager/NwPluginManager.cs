using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Windows;
using Autodesk.Navisworks.Api.Plugins;

namespace NwPluginManager
{
    public sealed class NwPluginManager
    {
        private NwPluginManager() { }

        public static NwPluginManager Instance { get; } = new NwPluginManager();

        public NwAddinPlugin ActivePlugin { get;  set; }
        public int Execute(bool faceless, params string[] parameters)
        {
            if (this.ActivePlugin != null && faceless)
            {
                return Execute(parameters);
            }
            PluginsWindow window = new PluginsWindow();
            if (window.ShowDialog().Value && this.ActivePlugin != null)
            {
                return this.Execute(parameters);
            }
            return 0;
        }

        private int Execute(params string[] parameters)
        {
            string filePath = this.ActivePlugin.AssemblyPath;
            PluginLoader pluginLoader = new PluginLoader();
            int result;
            try
            {
                pluginLoader.HookAssemblyResolve();
                Assembly assembly = pluginLoader.LoadAddinsToTempFolder(filePath);
                if (null == assembly)
                {
                    result = 0;
                }
                else
                {
                    //this.ActiveTempFolder = pluginLoader.TempFolder;
                    AddInPlugin addInPlugin = assembly.CreateInstance(this.ActivePlugin.ClassName) as AddInPlugin;
                    if (addInPlugin == null)
                    {
                        result = 0;
                    }
                    else
                    {
                        result = addInPlugin.Execute(parameters);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                result = 0;
            }
            finally
            {
                pluginLoader.UnhookAssemblyResolve();
            }

            return result;
        }

        public IPlugin LoadPlugin(string filePath)
        {
            PluginLoader pluginLoader = new PluginLoader();
            List<IPlugin> list = new List<IPlugin>();
            try
            {
                pluginLoader.HookAssemblyResolve();
                Assembly assembly = pluginLoader.LoadAddinsToTempFolder(filePath);
                if (null == assembly)
                {
                    return null;
                }
                list = pluginLoader.LoadItems(assembly, filePath);
            }
            catch (Exception)
            {
            }
            finally
            {
                pluginLoader.UnhookAssemblyResolve();
            }

            return new NwAddinPlugin()
            {
                Name = filePath,
                AssemblyPath = filePath,
                Children = list
            };
        }
    }
}