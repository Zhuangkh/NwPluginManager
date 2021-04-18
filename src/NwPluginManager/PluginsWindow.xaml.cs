using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace NwPluginManager
{
    /// <summary>
    /// PluginsWindow.xaml 的交互逻辑
    /// </summary>
    public partial class PluginsWindow : Window
    {
        public PluginsWindow()
        {
            InitializeComponent();
        }

        private void TreeView_OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            (this.DataContext as PluginsViewModel).SelectedPlugin = (e.NewValue as NwAddinPlugin);
        }
    }
}
