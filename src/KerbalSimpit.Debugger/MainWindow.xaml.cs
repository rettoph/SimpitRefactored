using KerbalSimpit.Core;
using KerbalSimpit.Core.Extensions;
using KerbalSimpit.Core.KSP.Extensions;
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
using Outgoing = KerbalSimpit.Core.KSP.Messages.Vessel.Outgoing;

namespace KerbalSimpit.Debugger
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IDisposable,
        ISimpitMessageSubscriber<CustomLog>
    {
        public static Simpit Simpit { get; private set; } = null!;

        private Outgoing.Altitude _alt;
        private Outgoing.Velocity _velocity;
        private Outgoing.DeltaV _deltaV;
        private Outgoing.Apsides _apsides;
        private Outgoing.ApsidesTime _apsideTime;
        private Outgoing.Maneuver _maneuver;
        private Outgoing.OrbitInfo _orbit;

        private Dictionary<SimpitPeer, PeerInfo> _peers;

        public MainWindow()
        {
            _peers = new Dictionary<SimpitPeer, PeerInfo>();

            SimpitConfiguration configuration = JsonSerializer.Deserialize<SimpitConfiguration>(File.ReadAllText("simpit.config.json"), new JsonSerializerOptions()
            {
                Converters =
                {
                    new JsonStringEnumConverter()
                }
            }) ?? new SimpitConfiguration();

            MainWindow.Simpit = new Simpit(new BasicSimpitLogger(configuration.LogLevel)).RegisterKerbal().AddIncomingSubscriber<CustomLog>(this);

            InitializeComponent();

            CompositionTarget.Rendering += this.Update;
            MainWindow.Simpit.OnPeerAdded += this.HandlePeerAdded;
            MainWindow.Simpit.OnPeerRemoved += this.HandlePeerRemoved;
            this.Closing += (sender, args) => this.Dispose();


            foreach (SimpitConfiguration.SerialConfiguration serial in configuration.Serial)
            {
                MainWindow.Simpit.AddSerialPeer(serial.Name, serial.BaudRate);
            }

            MainWindow.Simpit.Start();

            this.AddSimpleTextSubscriber<Core.KSP.Messages.Vessel.Incoming.Rotation>(rot => $"Pitch: {DebugHelper.Get(rot.Pitch)}, Yaw: {DebugHelper.Get(rot.Yaw)}, Roll: {DebugHelper.Get(rot.Roll)}, Mask: {rot.Mask}");
            this.AddSimpleTextSubscriber<Core.KSP.Messages.Vessel.Incoming.Translation>(tran => $"X: {DebugHelper.Get(tran.X)}, Y: {DebugHelper.Get(tran.Y)}, Z: {DebugHelper.Get(tran.Z)}, Mask: {tran.Mask}");
            this.AddSimpleTextSubscriber<Core.KSP.Messages.Vessel.Incoming.Throttle>(throttle => DebugHelper.Get(throttle.Value));
            this.AddSimpleTextSubscriber<Core.KSP.Messages.Vessel.Incoming.WheelControl>(wheel => $"Steer: {DebugHelper.Get(wheel.Steer)}, Throttle: {DebugHelper.Get(wheel.Throttle)}, Mask: {wheel.Mask}");

            this.OutgoingContent.Children.Add(new ActionGroupFlagsService());
            this.OutgoingContent.Children.Add(new AutoPilotModeService());
            this.OutgoingContent.Children.Add(new CustomActionGroupsService());


            this.OutgoingContent.Children.Add(new GenericValueInputService<float>(
                title: $"{nameof(Outgoing.Altitude)}.{nameof(Outgoing.Altitude.Alt)}",
                value: 0,
                deserialize: float.Parse,
                onChanged: v => _alt.Alt = v));

            this.OutgoingContent.Children.Add(new GenericValueInputService<float>(
                title: $"{nameof(Outgoing.Altitude)}.{nameof(Outgoing.Altitude.SurfAlt)}",
                value: 0,
                deserialize: float.Parse,
                onChanged: v => _alt.SurfAlt = v));

            this.OutgoingContent.Children.Add(new GenericValueInputService<float>(
                title: $"{nameof(Outgoing.Velocity)}.{nameof(Outgoing.Velocity.Orbital)}",
                value: 0,
                deserialize: float.Parse,
                onChanged: v => _velocity.Orbital = v));

            this.OutgoingContent.Children.Add(new GenericValueInputService<float>(
                title: $"{nameof(Outgoing.Velocity)}.{nameof(Outgoing.Velocity.Orbital)}",
                value: 0,
                deserialize: float.Parse,
                onChanged: v => _velocity.Surface = v));

            this.OutgoingContent.Children.Add(new GenericValueInputService<float>(
                title: $"{nameof(Outgoing.Velocity)}.{nameof(Outgoing.Velocity.Orbital)}",
                value: 0,
                deserialize: float.Parse,
                onChanged: v => _velocity.Vertical = v));

            this.OutgoingContent.Children.Add(new GenericValueInputService<float>(
                title: $"{nameof(Outgoing.DeltaV)}.{nameof(Outgoing.DeltaV.TotalDeltaV)}",
                value: 0,
                deserialize: float.Parse,
                onChanged: v => _deltaV.TotalDeltaV = v));

            this.OutgoingContent.Children.Add(new GenericValueInputService<float>(
                title: $"{nameof(Outgoing.DeltaV)}.{nameof(Outgoing.DeltaV.StageDeltaV)}",
                value: 0,
                deserialize: float.Parse,
                onChanged: v => _deltaV.StageDeltaV = v));

            this.OutgoingContent.Children.Add(new GenericValueInputService<float>(
                title: $"{nameof(Outgoing.Apsides)}.{nameof(Outgoing.Apsides.Apoapsis)}",
                value: 0,
                deserialize: float.Parse,
                onChanged: v => _apsides.Apoapsis = v));

            this.OutgoingContent.Children.Add(new GenericValueInputService<int>(
                title: $"{nameof(Outgoing.ApsidesTime)}.{nameof(Outgoing.ApsidesTime.Apoapsis)}",
                value: 0,
                deserialize: int.Parse,
                onChanged: v => _apsideTime.Apoapsis = v));

            this.OutgoingContent.Children.Add(new GenericValueInputService<float>(
                title: $"{nameof(Outgoing.Apsides)}.{nameof(Outgoing.Apsides.Periapsis)}",
                value: 0,
                deserialize: float.Parse,
                onChanged: v => _apsides.Periapsis = v));

            this.OutgoingContent.Children.Add(new GenericValueInputService<int>(
                title: $"{nameof(Outgoing.ApsidesTime)}.{nameof(Outgoing.ApsidesTime.Periapsis)}",
                value: 0,
                deserialize: int.Parse,
                onChanged: v => _apsideTime.Periapsis = v));

            this.OutgoingContent.Children.Add(new GenericValueInputService<float>(
                title: $"{nameof(Outgoing.Maneuver)}.{nameof(Outgoing.Maneuver.TimeToNextManeuver)}",
                value: 0,
                deserialize: float.Parse,
                onChanged: v => _maneuver.TimeToNextManeuver = v));

            this.OutgoingContent.Children.Add(new GenericValueInputService<float>(
                title: $"{nameof(Outgoing.Maneuver)}.{nameof(Outgoing.Maneuver.DeltaVNextManeuver)}",
                value: 0,
                deserialize: float.Parse,
                onChanged: v => _maneuver.DeltaVNextManeuver = v));

            this.OutgoingContent.Children.Add(new GenericValueInputService<float>(
                title: $"{nameof(Outgoing.Maneuver)}.{nameof(Outgoing.Maneuver.DurationNextManeuver)}",
                value: 0,
                deserialize: float.Parse,
                onChanged: v => _maneuver.DurationNextManeuver = v));

            this.OutgoingContent.Children.Add(new GenericValueInputService<float>(
                title: $"{nameof(Outgoing.Maneuver)}.{nameof(Outgoing.Maneuver.DeltaVTotal)}",
                value: 0,
                deserialize: float.Parse,
                onChanged: v => _maneuver.DeltaVTotal = v));

            this.OutgoingContent.Children.Add(new GenericValueInputService<float>(
                title: $"{nameof(Outgoing.Maneuver)}.{nameof(Outgoing.Maneuver.HeadingNextManeuver)}",
                value: 0,
                deserialize: float.Parse,
                onChanged: v => _maneuver.HeadingNextManeuver = v));

            this.OutgoingContent.Children.Add(new GenericValueInputService<float>(
                title: $"{nameof(Outgoing.Maneuver)}.{nameof(Outgoing.Maneuver.PitchNextManeuver)}",
                value: 0,
                deserialize: float.Parse,
                onChanged: v => _maneuver.PitchNextManeuver = v));

            this.OutgoingContent.Children.Add(new GenericValueInputService<float>(
                title: $"{nameof(Outgoing.OrbitInfo)}.{nameof(Outgoing.OrbitInfo.Eccentricity)}",
                value: 0,
                deserialize: float.Parse,
                onChanged: v => _orbit.Eccentricity = v));

            this.OutgoingContent.Children.Add(new GenericValueInputService<float>(
                title: $"{nameof(Outgoing.OrbitInfo)}.{nameof(Outgoing.OrbitInfo.SemiMajorAxis)}",
                value: 0,
                deserialize: float.Parse,
                onChanged: v => _orbit.SemiMajorAxis = v));

            this.OutgoingContent.Children.Add(new GenericValueInputService<float>(
                title: $"{nameof(Outgoing.OrbitInfo)}.{nameof(Outgoing.OrbitInfo.Inclination)}",
                value: 0,
                deserialize: float.Parse,
                onChanged: v => _orbit.Inclination = v));

            this.OutgoingContent.Children.Add(new GenericValueInputService<float>(
                title: $"{nameof(Outgoing.OrbitInfo)}.{nameof(Outgoing.OrbitInfo.LongAscendingNode)}",
                value: 0,
                deserialize: float.Parse,
                onChanged: v => _orbit.LongAscendingNode = v));

            this.OutgoingContent.Children.Add(new GenericValueInputService<float>(
                title: $"{nameof(Outgoing.OrbitInfo)}.{nameof(Outgoing.OrbitInfo.ArgPeriapsis)}",
                value: 0,
                deserialize: float.Parse,
                onChanged: v => _orbit.ArgPeriapsis = v));

            this.OutgoingContent.Children.Add(new GenericValueInputService<float>(
                title: $"{nameof(Outgoing.OrbitInfo)}.{nameof(Outgoing.OrbitInfo.TrueAnomaly)}",
                value: 0,
                deserialize: float.Parse,
                onChanged: v => _orbit.TrueAnomaly = v));

            this.OutgoingContent.Children.Add(new GenericValueInputService<float>(
                title: $"{nameof(Outgoing.OrbitInfo)}.{nameof(Outgoing.OrbitInfo.MeanAnomaly)}",
                value: 0,
                deserialize: float.Parse,
                onChanged: v => _orbit.MeanAnomaly = v));

            this.OutgoingContent.Children.Add(new GenericValueInputService<float>(
                title: $"{nameof(Outgoing.OrbitInfo)}.{nameof(Outgoing.OrbitInfo.Period)}",
                value: 0,
                deserialize: float.Parse,
                onChanged: v => _orbit.Period = v));
        }

        private void Update(object? sender, EventArgs e)
        {
            MainWindow.Simpit.Flush();

            MainWindow.Simpit.SetOutgoingData(_alt);
            MainWindow.Simpit.SetOutgoingData(_velocity);
            MainWindow.Simpit.SetOutgoingData(_deltaV);
            MainWindow.Simpit.SetOutgoingData(_apsides);
            MainWindow.Simpit.SetOutgoingData(_apsideTime);
            MainWindow.Simpit.SetOutgoingData(_maneuver);
            MainWindow.Simpit.SetOutgoingData(_orbit);
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
    }
}