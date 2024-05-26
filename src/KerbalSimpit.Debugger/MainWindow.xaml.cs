using KerbalSimpit.Core;
using KerbalSimpit.Core.KSP.Extensions;
using KerbalSimpit.Core.KSP.Messages;
using KerbalSimpit.Core.Messages;
using KerbalSimpit.Core.Peers;
using KerbalSimpit.Core.Utilities;
using KerbalSimpit.Debugger.Controls;
using KerbalSimpit.Debugger.Services;
using KerbalSimpit.Debugger.Utilities;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows;
using System.Windows.Media;
using WindowsInput.Native;
using Outgoing = KerbalSimpit.Core.KSP.Messages.Vessel.Outgoing;
using Resource = KerbalSimpit.Core.KSP.Messages.Resource;

namespace KerbalSimpit.Debugger
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IDisposable,
        ISimpitMessageSubscriber<CustomLog>,
        ISimpitMessageSubscriber<KeyboardEmulator>,
        ISimpitMessageSubscriber<EchoRequest>
    {
        public static Simpit Simpit { get; private set; } = null!;

        private Dictionary<SimpitPeer, PeerInfo> _peers;

        public unsafe MainWindow()
        {
            _peers = new Dictionary<SimpitPeer, PeerInfo>();

            SimpitConfiguration configuration = JsonSerializer.Deserialize<SimpitConfiguration>(File.ReadAllText("simpit.config.json"), new JsonSerializerOptions()
            {
                IncludeFields = true,
                Converters =
                {
                    new JsonStringEnumConverter()
                }
            }) ?? new SimpitConfiguration();

            MainWindow.Simpit = new Simpit(new BasicSimpitLogger(configuration.LogLevel)).RegisterKerbal().AddIncomingSubscribers(this);

            InitializeComponent();

            CompositionTarget.Rendering += this.Update;
            MainWindow.Simpit.OnPeerAdded += this.HandlePeerAdded;
            MainWindow.Simpit.OnPeerRemoved += this.HandlePeerRemoved;
            this.Closing += (sender, args) => this.Dispose();

            MainWindow.Simpit.Start(configuration);

            this.AddSimpleTextSubscriber<Core.KSP.Messages.Vessel.Incoming.Rotation>(rot => $"Pitch: {DebugHelper.Get(rot.Pitch)}, Yaw: {DebugHelper.Get(rot.Yaw)}, Roll: {DebugHelper.Get(rot.Roll)}, Mask: {rot.Mask}");
            this.AddSimpleTextSubscriber<Core.KSP.Messages.Vessel.Incoming.Translation>(tran => $"X: {DebugHelper.Get(tran.X)}, Y: {DebugHelper.Get(tran.Y)}, Z: {DebugHelper.Get(tran.Z)}, Mask: {tran.Mask}");
            this.AddSimpleTextSubscriber<Core.KSP.Messages.Vessel.Incoming.Throttle>(throttle => DebugHelper.Get(throttle.Value));
            this.AddSimpleTextSubscriber<Core.KSP.Messages.Vessel.Incoming.WheelControl>(wheel => $"Steer: {DebugHelper.Get(wheel.Steer)}, Throttle: {DebugHelper.Get(wheel.Throttle)}, Mask: {wheel.Mask}");

            this.OutgoingContent.Children.Add(new ActionGroupFlagsService());
            this.OutgoingContent.Children.Add(new AutoPilotModeService());
            this.OutgoingContent.Children.Add(new CustomActionGroupsService());

            this.OutgoingContent.Children.Add(new OutgoingMessageService<Outgoing.Altitude>());
            this.OutgoingContent.Children.Add(new OutgoingMessageService<Outgoing.Velocity>());
            this.OutgoingContent.Children.Add(new OutgoingMessageService<Outgoing.DeltaV>());
            this.OutgoingContent.Children.Add(new OutgoingMessageService<Outgoing.DeltaVEnv>());
            this.OutgoingContent.Children.Add(new OutgoingMessageService<Outgoing.Apsides>());
            this.OutgoingContent.Children.Add(new OutgoingMessageService<Outgoing.ApsidesTime>());
            this.OutgoingContent.Children.Add(new OutgoingMessageService<Outgoing.Maneuver>());
            this.OutgoingContent.Children.Add(new OutgoingMessageService<Outgoing.OrbitInfo>());
            this.OutgoingContent.Children.Add(new OutgoingMessageService<Outgoing.Rotation>());
            this.OutgoingContent.Children.Add(new OutgoingMessageService<Outgoing.Airspeed>());
            this.OutgoingContent.Children.Add(new OutgoingMessageService<Outgoing.BurnTime>());
            this.OutgoingContent.Children.Add(new OutgoingMessageService<Outgoing.TempLimit>());

            this.OutgoingContent.Children.Add(new OutgoingMessageService<Resource.LiquidFuel>());
            this.OutgoingContent.Children.Add(new OutgoingMessageService<Resource.LiquidFuelStage>());
            this.OutgoingContent.Children.Add(new OutgoingMessageService<Resource.Methane>());
            this.OutgoingContent.Children.Add(new OutgoingMessageService<Resource.MethaneStage>());
            this.OutgoingContent.Children.Add(new OutgoingMessageService<Resource.OxidizerStage>());
            this.OutgoingContent.Children.Add(new OutgoingMessageService<Resource.SolidFuel>());
            this.OutgoingContent.Children.Add(new OutgoingMessageService<Resource.SolidFuelStage>());
            this.OutgoingContent.Children.Add(new OutgoingMessageService<Resource.XenonGas>());
            this.OutgoingContent.Children.Add(new OutgoingMessageService<Resource.XenonGasStage>());
            this.OutgoingContent.Children.Add(new OutgoingMessageService<Resource.MonoPropellant>());
            this.OutgoingContent.Children.Add(new OutgoingMessageService<Resource.EvaPropellant>());
            this.OutgoingContent.Children.Add(new OutgoingMessageService<Resource.ElectricCharge>());
            this.OutgoingContent.Children.Add(new OutgoingMessageService<Resource.Ore>());
            this.OutgoingContent.Children.Add(new OutgoingMessageService<Resource.Ablator>());
            this.OutgoingContent.Children.Add(new OutgoingMessageService<Resource.AblatorStage>());
        }

        private void Update(object? sender, EventArgs e)
        {
            MainWindow.Simpit.Flush();
        }

        public void Dispose()
        {
            CompositionTarget.Rendering -= this.Update;

            MainWindow.Simpit.Stop();

            Application.Current.Shutdown();
        }

        private void AddSimpleTextSubscriber<T>(Func<T?, string> text)
            where T : ISimpitMessageData
        {
            SimpleTextSubscriber<T> subscriber = new SimpleTextSubscriber<T>(text);

            this.IncomingContent.Children.Add(subscriber.Control);
            MainWindow.Simpit.AddIncomingSubscriber(subscriber);
        }

        public void Process(SimpitPeer peer, ISimpitMessage<CustomLog> message)
        {
            MainWindow.Simpit.Logger.LogInformation($"{peer} {nameof(CustomLog)} - {message.Data.Flags}: {message.Data.Value}");
        }

        private void HandlePeerAdded(object? sender, SimpitPeer e)
        {
            PeerInfo info = new PeerInfo(e);
            _peers.Add(e, info);

            this.InfoContent.Children.Add(info);
        }

        private void HandlePeerRemoved(object? sender, SimpitPeer e)
        {
            if (_peers.Remove(e, out PeerInfo? info) == false)
            {
                throw new InvalidOperationException();
            }

            this.InfoContent.Children.Remove(info);
            info.Dispose();
        }

        private void ToggleFlightScene_Checked(object sender, RoutedEventArgs e)
        {
            MainWindow.Simpit.SetOutgoingData(new Core.KSP.Messages.Environment.SceneChange()
            {
                Type = Core.KSP.Messages.Environment.SceneChange.SceneChangeTypeEnum.Flight
            });
        }

        private void ToggleFlightScene_Unchecked(object sender, RoutedEventArgs e)
        {
            MainWindow.Simpit.SetOutgoingData(new Core.KSP.Messages.Environment.SceneChange()
            {
                Type = Core.KSP.Messages.Environment.SceneChange.SceneChangeTypeEnum.NotFlight
            });
        }

        private void ToggleActionGroup_Checked(object sender, RoutedEventArgs e)
        {
            MainWindow.Simpit.SetOutgoingData(new Core.KSP.Messages.Environment.SceneChange()
            {
                Type = Core.KSP.Messages.Environment.SceneChange.SceneChangeTypeEnum.Flight
            });
        }

        private void ToggleActionGroup_Unchecked(object sender, RoutedEventArgs e)
        {
            MainWindow.Simpit.SetOutgoingData(new Core.KSP.Messages.Environment.SceneChange()
            {
                Type = Core.KSP.Messages.Environment.SceneChange.SceneChangeTypeEnum.NotFlight
            });
        }

        private void ToggleRatio_Checked(object sender, RoutedEventArgs e)
        {
            DebugHelper.Ratio = true;
        }

        private void ToggleRatio_Unchecked(object sender, RoutedEventArgs e)
        {
            DebugHelper.Ratio = false;
        }

        public void Process(SimpitPeer peer, ISimpitMessage<KeyboardEmulator> message)
        {
            Simpit.Logger.LogInformation($"{nameof(KeyboardEmulator)} - {(VirtualKeyCode)message.Data.Key}, {message.Data.Modifier}");
        }

        public void Process(SimpitPeer peer, ISimpitMessage<EchoRequest> message)
        {
            Simpit.Logger.LogVerbose("Echo request on peer {0}. Replying.", peer);
            peer.EnqueueOutgoing(new EchoResponse()
            {
                Data = message.Data.Data
            });
        }
    }
}