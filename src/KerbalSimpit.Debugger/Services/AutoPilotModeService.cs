using KerbalSimpit.Core;
using KerbalSimpit.Core.KSP.Enums;
using KerbalSimpit.Core.KSP.Messages;
using KerbalSimpit.Core.Peers;
using System.Windows.Media;

namespace KerbalSimpit.Debugger.Controls
{
    internal class AutoPilotModeService : CheckboxGrid,
        ISimpitMessageSubscriber<Vessel.Incoming.ActionGroupActivate>,
        ISimpitMessageSubscriber<Vessel.Incoming.ActionGroupDeactivate>,
        ISimpitMessageSubscriber<Vessel.Incoming.ActionGroupToggle>
    {
        private ActionGroupFlags _flags;

        public AutoPilotModeService() : base(nameof(ActionGroupFlags), Enum.GetValues<ActionGroupFlags>().Select(x => (object)x).ToArray())
        {
            CompositionTarget.Rendering += this.Update;

            MainWindow.Simpit.AddIncomingSubscriber<Vessel.Incoming.ActionGroupActivate>(this);
            MainWindow.Simpit.AddIncomingSubscriber<Vessel.Incoming.ActionGroupDeactivate>(this);
            MainWindow.Simpit.AddIncomingSubscriber<Vessel.Incoming.ActionGroupToggle>(this);
        }

        private void Update(object? sender, EventArgs e)
        {
            MainWindow.Simpit.SetOutgoingData(new Vessel.Outgoing.ActionGroups()
            {
                Flags = _flags
            });
        }

        protected override void Checked(object value)
        {
            ActionGroupFlags flag = (ActionGroupFlags)value;

            _flags |= flag;
        }

        protected override void Unchecked(object value)
        {
            ActionGroupFlags flag = (ActionGroupFlags)value;

            _flags &= ~flag;
        }

        public void Process(SimpitPeer peer, ISimpitMessage<Vessel.Incoming.ActionGroupActivate> message)
        {
            _flags |= message.Data.Flags;

            foreach (ActionGroupFlags flag in Enum.GetValues<ActionGroupFlags>().Where(x => message.Data.Flags.HasFlag(x)))
            {
                this.SetIsChecked(flag, true);
            }
        }

        public void Process(SimpitPeer peer, ISimpitMessage<Vessel.Incoming.ActionGroupDeactivate> message)
        {
            _flags &= ~message.Data.Flags;

            foreach (ActionGroupFlags flag in Enum.GetValues<ActionGroupFlags>().Where(x => message.Data.Flags.HasFlag(x)))
            {
                this.SetIsChecked(flag, false);
            }
        }

        public void Process(SimpitPeer peer, ISimpitMessage<Vessel.Incoming.ActionGroupToggle> message)
        {
            foreach (ActionGroupFlags flag in Enum.GetValues<ActionGroupFlags>().Where(x => message.Data.Flags.HasFlag(x)))
            {
                if (_flags.HasFlag(flag))
                {
                    _flags &= ~flag;
                    this.SetIsChecked(flag, false);
                }
                else
                {
                    _flags |= flag;
                    this.SetIsChecked(flag, true);
                }
            }
        }
    }
}
