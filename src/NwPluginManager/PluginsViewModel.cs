using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Toolkit.Mvvm;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Win32;

namespace NwPluginManager
{
    public class PluginsViewModel : ObservableObject
    {
        public RelayCommand LoadCommand => new RelayCommand(() =>
         {
             OpenFileDialog openFileDialog = new OpenFileDialog();
             openFileDialog.Filter = "Assembly files (*.dll;*.exe)|*.dll;*.exe|All files|*.*||";
             if (openFileDialog.ShowDialog().Value)
             {
                 var plugin = NwPluginManager.Instance.LoadPlugin(openFileDialog.FileName);
                 if (plugin == null || !plugin.Children.Any())
                 {
                     System.Windows.MessageBox.Show("Load failed.");
                     return;
                 }
                 Plugins.Add(plugin);
                 return;
             }
             System.Windows.MessageBox.Show("Load canceled.");
         });

        public ObservableCollection<IPlugin> Plugins { get; set; } = new ObservableCollection<IPlugin>();

        public RelayCommand<Window> RunCommand => new RelayCommand<Window>((win) =>
        {
            NwPluginManager.Instance.ActivePlugin = SelectedPlugin as NwAddinPlugin;
            win.DialogResult = true;
        }, (win) => SelectedPlugin is NwAddinPlugin nwPlugin && !string.IsNullOrEmpty(nwPlugin.ClassName));

        public IPlugin SelectedPlugin
        {
            get => _selectedPlugin;
            set
            {
                SetProperty(ref _selectedPlugin, value);
                this.RunCommand.NotifyCanExecuteChanged();
            }
        }

        private IPlugin _selectedPlugin;
    }

}
