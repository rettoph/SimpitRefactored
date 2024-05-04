using KerbalSimpit.Core.Kerbal.Messages;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KerbalSimpit.Debugger.Controls
{
    /// <summary>
    /// Interaction logic for SceneChange.xaml
    /// </summary>
    public partial class SceneChangeProvider : UserControl
    {
        public SceneChangeProvider()
        {
            InitializeComponent();
        }

        private void checkBox_Checked(object sender, RoutedEventArgs e)
        {
            MainWindow.Simpit.Broadcast(new SceneChange()
            {
                Type = SceneChange.SceneChangeTypeEnum.Flight
            });
        }

        private void checkBox_Unchecked(object sender, RoutedEventArgs e)
        {
            MainWindow.Simpit.Broadcast(new SceneChange()
            {
                Type = SceneChange.SceneChangeTypeEnum.NotFlight
            });
        }
    }
}
