using KerbalSimpit.Core;
using KerbalSimpit.Core.Extensions;
using KerbalSimpit.Core.KSP.Extensions;
using KerbalSimpit.Core.KSP.Messages;
using KerbalSimpit.Core.Messages;
using KerbalSimpit.Core.Peers;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Media;

namespace KerbalSimpit.Debugger
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IDisposable,
        ISimpitMessageConsumer<CustomLog>,
        ISimpitMessageConsumer<Vessel.Incoming.Throttle>,
        ISimpitMessageConsumer<Vessel.Incoming.Translation>,
        ISimpitMessageConsumer<Vessel.Incoming.Rotation>
    {
        private readonly Simpit _simpit;
        private readonly CancellationTokenSource _cancellationTokenSource;

        public MainWindow()
        {
            InitializeComponent();

            _simpit = new Simpit(new BasicSimpitLogger()).RegisterKerbal().AddIncomingConsumer<CustomLog>(this);

            SimpitConfiguration configuration = JsonSerializer.Deserialize<SimpitConfiguration>(File.ReadAllText("simpit.config.json")) ?? new SimpitConfiguration();
            foreach (SimpitConfiguration.SerialConfiguration serial in configuration.Serial)
            {
                _simpit.AddSerialPeer(serial.Name, serial.BaudRate);
            }

            CompositionTarget.Rendering += this.Update;
            this.Closing += (sender, args) => this.Dispose();

            _simpit.Start();
        }

        private void Update(object? sender, EventArgs e)
        {
            _simpit.Flush();
        }

        public void Dispose()
        {
            CompositionTarget.Rendering -= this.Update;

            _simpit.Stop();

            Application.Current.Shutdown();
        }

        public void Consume(SimpitPeer peer, ISimpitMessage<CustomLog> message)
        {
            // MainWindow.Simpit.Logger.LogInformation($"{nameof(CustomLog)} - {message.Content.Flags}:{message.Content.Value}");
        }

        public void Consume(SimpitPeer peer, ISimpitMessage<Vessel.Incoming.Throttle> message)
        {
            // this.Dispatcher.Invoke(() =>
            // {
            //     this.ThrottleValue.Content = $"Throttle: {message.Content.Value}";
            // });
        }

        public void Consume(SimpitPeer peer, ISimpitMessage<Vessel.Incoming.Translation> message)
        {
            // this.Dispatcher.Invoke(() =>
            // {
            //     this.TranslationValue.Content = $"Translation: {message.Content.X}, {message.Content.Y}, {message.Content.Z}, Mask: {message.Content.Mask}";
            // });
        }

        public void Consume(SimpitPeer peer, ISimpitMessage<Vessel.Incoming.Rotation> message)
        {
            // this.Dispatcher.Invoke(() =>
            // {
            //     this.RotationValue.Content = $"Pitch: {message.Content.Pitch}, Yaw: {message.Content.Yaw}, Roll: {message.Content.Roll}, Mask: {message.Content.Mask}";
            // });
        }
    }
}