using Autodesk.Navisworks.Api;
using Autodesk.Navisworks.Api.Plugins;

namespace NwPluginManager
{
    [Plugin("NwPluginManagerManual", "Zhuangkh")]
    [AddInPlugin(AddInLocation.AddIn)]
    public class NwPluginManagerManual : AddInPlugin
    {
        public override int Execute(params string[] parameters)
        {
            return NwPluginManager.Instance.Execute(false, parameters);
        }
    }

    [Plugin("NwPluginManagerFaceless", "Zhuangkh")]
    [AddInPlugin(AddInLocation.AddIn)]
    public class NwPluginManagerFaceless : AddInPlugin
    {
        public override int Execute(params string[] parameters)
        {
            return NwPluginManager.Instance.Execute(true, parameters);
        }
    }
}
