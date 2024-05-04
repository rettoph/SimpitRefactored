using KerbalSimpit.Core;
using KerbalSimpit.Core.Enums;
using KerbalSimpit.Core.Extensions;
using KerbalSimpit.Core.Kerbal.Extensions;
using KerbalSimpit.Core.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
        public static Simpit Simpit { get; private set; }

        private readonly Thread _simpitThread;

        public MainWindow()
        {
            InitializeComponent();

            MainWindow.Simpit = new Simpit(this.Logger);
            MainWindow.Simpit
                .RegisterKerbal()
                .RegisterIncomingConsumer<CustomLog>(this.Logger)
                .RegisterSerial("COM3", 115200)
                .Start();


            _simpitThread = new Thread(() =>
            {
                while(true)
                {
                    MainWindow.Simpit.Flush();
                    Thread.Sleep(100);
                }
            });

            _simpitThread.Start();
        }
    }
}
