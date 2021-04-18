using System.Collections.Generic;

namespace NwPluginManager
{
    public interface IPlugin
    {
        string Name { get; set; }
        string AssemblyPath { get; set; }
        IPlugin Parent { get; set; }
        IList<IPlugin> Children { get; set; }
    }
    public class NwAddinPlugin : IPlugin
    {
        public string AssemblyPath { get; set; }
        public string Name { get; set; }
        public string ClassName { get; set; }
        public IPlugin Parent { get; set; }
        public IList<IPlugin> Children { get; set; }

    }
}