using KerbalSimpit.Core;
using KerbalSimpit.Core.Enums;
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

namespace KerbalSimpit.Debugger
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            // Simpit simpit = new Simpit(new ConsoleLogger(SimpitLogLevelEnum.Verbose));
            // simpit
            //     .RegisterKerbal()
            //     .RegisterIncomingConsumer(new CustomLogSubscriber(simpit.Logger))
            //     .RegisterSerial("COM4", 115200)
            //     .Start();

            InitializeComponent();
        }
    }
}
