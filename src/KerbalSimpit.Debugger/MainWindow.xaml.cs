using KerbalSimpit.Core;
using KerbalSimpit.Core.Enums;
using KerbalSimpit.Core.Extensions;
using KerbalSimpit.Core.Kerbal.Extensions;
using KerbalSimpit.Core.Kerbal.Messages;
using KerbalSimpit.Core.Messages;
using KerbalSimpit.Core.Peers;
using KerbalSimpit.Core.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
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
    public partial class MainWindow : Window, 
        ISimpitMessageConsumer<CustomLog>,
        ISimpitMessageConsumer<Vessel.Incoming.Throttle>,
        ISimpitMessageConsumer<Vessel.Incoming.Translation>,
        ISimpitMessageConsumer<Vessel.Incoming.Rotation>
    {
        public static Simpit Simpit { get; private set; }
        public readonly ISimpitLogger Logger;

        private readonly Thread _simpitThread;

        public MainWindow()
        {
            InitializeComponent();

            this.Logger = new ConsoleLogger(SimpitLogLevelEnum.Verbose);

            MainWindow.Simpit = new Simpit(this.Logger);
            MainWindow.Simpit
                .RegisterKerbal()
                .RegisterIncomingConsumer<CustomLog>(this)
                .RegisterIncomingConsumer<Vessel.Incoming.Throttle>(this)
                .RegisterIncomingConsumer<Vessel.Incoming.Translation>(this)
                .RegisterIncomingConsumer<Vessel.Incoming.Rotation>(this);

            foreach(string config in File.ReadAllText("ports.txt").Split(','))
            {
                string[] args = config.Split(':');

                MainWindow.Simpit.RegisterSerial(args[0], int.Parse(args[1]));
            }

            MainWindow.Simpit.Start();


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

        public void Consume(SimpitPeer peer, ISimpitMessage<CustomLog> message)
        {
            this.Logger.LogInformation($"{nameof(CustomLog)} - {message.Content.Flags}:{message.Content.Value}");
        }

        public void Consume(SimpitPeer peer, ISimpitMessage<Vessel.Incoming.Throttle> message)
        {
            this.Dispatcher.Invoke(() =>
            {
                this.ThrottleValue.Content = $"Throttle: {message.Content.Value}";
            });
        }

        public void Consume(SimpitPeer peer, ISimpitMessage<Vessel.Incoming.Translation> message)
        {
            this.Dispatcher.Invoke(() =>
            {
                this.TranslationValue.Content = $"Translation: {message.Content.X}, {message.Content.Y}, {message.Content.Z}, Mask: {message.Content.Mask}";
            });
        }

        public void Consume(SimpitPeer peer, ISimpitMessage<Vessel.Incoming.Rotation> message)
        {
            this.Dispatcher.Invoke(() =>
            {
                this.RotationValue.Content = $"Pitch: {message.Content.Pitch}, Yaw: {message.Content.Yaw}, Roll: {message.Content.Roll}, Mask: {message.Content.Mask}";
            });
        }
    }
}
