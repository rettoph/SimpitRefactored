using KerbalSimpit.Core;
using KerbalSimpit.Core.KSP.Messages;
using KerbalSimpit.Core.Peers;
using KerbalSimpit.Debugger.Controls;
using System.Windows.Media;

namespace KerbalSimpit.Debugger.Services
{
    internal class CustomActionGroupsService : CheckboxGrid,
        ISimpitMessageSubscriber<Core.KSP.Messages.Vessel.Incoming.CustomActionGroupToggle>,
        ISimpitMessageSubscriber<Core.KSP.Messages.Vessel.Incoming.CustomActionGroupEnable>,
        ISimpitMessageSubscriber<Core.KSP.Messages.Vessel.Incoming.CustomActionGroupDisable>
    {
        private Core.KSP.Messages.Vessel.Outgoing.CustomActionGroups _cag;
        private bool _dirty;

        public CustomActionGroupsService() : base(nameof(Core.KSP.Messages.Vessel.Outgoing.CustomActionGroups), Enumerable.Range(1, Core.KSP.Messages.Vessel.Outgoing.CustomActionGroups.Length).Select(x => (object)x).ToArray(), 6)
        {
            _cag = new Core.KSP.Messages.Vessel.Outgoing.CustomActionGroups();

            CompositionTarget.Rendering += this.Update;

            MainWindow.Simpit.AddIncomingSubscriber<Vessel.Incoming.CustomActionGroupToggle>(this);
            MainWindow.Simpit.AddIncomingSubscriber<Vessel.Incoming.CustomActionGroupEnable>(this);
            MainWindow.Simpit.AddIncomingSubscriber<Vessel.Incoming.CustomActionGroupDisable>(this);
        }

        private void Update(object? sender, EventArgs e)
        {
            MainWindow.Simpit.SetOutgoingData(_cag, _dirty);
            _dirty = false;
        }

        protected unsafe override void Checked(object value)
        {
            int actionGroup = (int)value;

            this.Set(actionGroup, true);
        }

        protected unsafe override void Unchecked(object value)
        {
            int actionGroup = (int)value;

            this.Set(actionGroup, false);
        }

        public void Process(SimpitPeer peer, ISimpitMessage<Vessel.Incoming.CustomActionGroupToggle> message)
        {
            foreach (byte actionGroup in message.Data.GroupIds)
            {
                bool current = this.Get(actionGroup);
                this.Set(actionGroup, !current);
                this.SetIsChecked((int)actionGroup, !current);
            }
        }

        public void Process(SimpitPeer peer, ISimpitMessage<Vessel.Incoming.CustomActionGroupEnable> message)
        {
            foreach (byte actionGroup in message.Data.GroupIds)
            {
                this.Set(actionGroup, true);
                this.SetIsChecked((int)actionGroup, true);
            }
        }

        public void Process(SimpitPeer peer, ISimpitMessage<Vessel.Incoming.CustomActionGroupDisable> message)
        {
            foreach (byte actionGroup in message.Data.GroupIds)
            {
                this.Set(actionGroup, false);
                this.SetIsChecked((int)actionGroup, false);
            }
        }

        private unsafe bool Get(int group)
        {
            int index = group / 8;
            int maskBit = group % 8;
            byte mask = (byte)(1 << (maskBit));

            return (_cag.Status[index] & mask) != 0;
        }

        private unsafe void Set(int group, bool value)
        {
            int index = group / 8;
            int maskBit = group % 8;
            byte mask = (byte)(1 << (maskBit));

            if ((_cag.Status[index] & mask) != 0)
            {
                if (value == false)
                {
                    _cag.Status[index] &= (byte)~mask;
                    _dirty = true;
                }
            }
            else
            {
                if (value == true)
                {
                    _cag.Status[index] |= mask;
                    _dirty = true;
                }
            }
        }
    }
}
