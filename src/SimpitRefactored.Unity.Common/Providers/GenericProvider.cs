using SimpitRefactored.Common.Core;
using SimpitRefactored.Core;
using System;
using System.Linq;

namespace SimpitRefactored.Unity.Common.Providers
{
    [KSPAddon(KSPAddon.Startup.Flight, false)]
    public abstract class GenericProvider<T> : SimpitBehaviour
        where T : unmanaged, ISimpitMessageData
    {
        private OutgoingData<T> _outgoing;

        public override void Start()
        {
            base.Start();

            if (this.simpit.Messages.TryGetOutgoingType<T>(out SimpitMessageType<T> type) == false)
            {
                throw new InvalidOperationException($"{this.GetType().Name}::{nameof(Start)} - Unknown message type {typeof(T).Name}");
            }

            _outgoing = this.simpit.GetOutgoingData<T>(type);
        }

        protected abstract T GetOutgoingData();

        protected virtual void CleanOutgoingData()
        {
            if (_outgoing?.Subscribers == null)
            {
                return;
            }

            if (_outgoing.Subscribers.Count() == 0)
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
