using KerbalSimpit.Core;
using System;

namespace KerbalSimpit.Unity.Common.Providers
{
    [KSPAddon(KSPAddon.Startup.Flight, false)]
    public abstract class GenericProvider<T> : SimpitBehaviour
        where T : ISimpitMessageData
    {
        private OutgoingData<T> _outgoing;

        public virtual void Start()
        {
            if (this.simpit.Messages.TryGetOutgoingType<T>(out SimpitMessageType<T> type) == false)
            {
                throw new InvalidOperationException($"{this.GetType().Name}::{nameof(Start)} - Unknown message type {typeof(T).Name}");
            }

            _outgoing = this.simpit.GetOutgoingData<T>(type);
        }

        protected abstract T GetOutgoingData();

        protected virtual void CleanOutgoingData()
        {
            if (_outgoing.Subscribers.Count == 0)
            {
                return;
            }

            if (this.ShouldCleanOutgoingData() == false)
            {
                return;
            }

            this.simpit.SetOutgoingData(this.GetOutgoingData());
        }

        protected virtual bool ShouldCleanOutgoingData()
        {
            return true;
        }
    }
}
