using KerbalSimpit.Core;
using KerbalSimpit.Core.KSP.Enums;
using KerbalSimpit.Core.KSP.Messages;
using KerbalSimpit.Core.Peers;
using System.Windows.Media;

namespace KerbalSimpit.Debugger.Controls
{
    internal class AutoPilotModeService : RadioGrid,
        ISimpitMessageSubscriber<Vessel.Incoming.AutopilotMode>
    {
        private AutoPilotModeEnum _mode;

        public AutoPilotModeService() : base($"Current {nameof(AutoPilotModeEnum)}", Enum.GetValues<AutoPilotModeEnum>().Except([AutoPilotModeEnum.Disabled]).Select(x => (object)x).ToArray(), 3)
        {
            CompositionTarget.Rendering += this.Update;

            MainWindow.Simpit.AddIncomingSubscriber<Vessel.Incoming.AutopilotMode>(this);
        }

        private void Update(object? sender, EventArgs e)
        {
            MainWindow.Simpit.SetOutgoingData(new Vessel.Outgoing.SASInfo()
            {
                CurrentSASMode = _mode
            });
        }

        protected override void Checked(object value)
        {
            _mode = (AutoPilotModeEnum)value;
        }

        protected override void Unchecked(object value)
        {
            //
        }

        public void Process(SimpitPeer peer, ISimpitMessage<Vessel.Incoming.AutopilotMode> message)
        {
            _mode = message.Data.Value;
            this.SetIsChecked(_mode, true);
        }
    }
}
