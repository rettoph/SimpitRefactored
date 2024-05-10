using KerbalSimpit.Unity.Common;
using VesselMessages = KerbalSimpit.Core.KSP.Messages.Vessel;

namespace KerbalSimpit.Unity.KSP1.Controllers
{
    public partial class VesselController : SimpitBehaviour
    {
        private Vessel _vessel;

        public void Start()
        {
            // Movement/Position
            this.simpit.AddIncomingSubscriber<VesselMessages.Incoming.Rotation>(this);
            this.simpit.AddIncomingSubscriber<VesselMessages.Incoming.Translation>(this);
            this.simpit.AddIncomingSubscriber<VesselMessages.Incoming.WheelControl>(this);
            this.simpit.AddIncomingSubscriber<VesselMessages.Incoming.CustomAxix>(this);
            this.simpit.AddIncomingSubscriber<VesselMessages.Incoming.Throttle>(this);
            this.simpit.AddIncomingSubscriber<VesselMessages.Incoming.AutopilotMode>(this);

            GameEvents.onVesselChange.Add(this.OnVesselChangeHandler);

            this.Clean(FlightGlobals.ActiveVessel);
        }

        public void OnDestroy()
        {
            // Movement/Position
            this.simpit.RemoveIncomingSubscriber<VesselMessages.Incoming.Rotation>(this);
            this.simpit.RemoveIncomingSubscriber<VesselMessages.Incoming.Translation>(this);
            this.simpit.RemoveIncomingSubscriber<VesselMessages.Incoming.WheelControl>(this);
            this.simpit.RemoveIncomingSubscriber<VesselMessages.Incoming.CustomAxix>(this);
            this.simpit.RemoveIncomingSubscriber<VesselMessages.Incoming.Throttle>(this);
            this.simpit.RemoveIncomingSubscriber<VesselMessages.Incoming.AutopilotMode>(this);

            GameEvents.onVesselChange.Remove(this.OnVesselChangeHandler);

            this.Clean(null);
        }

        private void Clean(Vessel vessel)
        {
            if (_vessel == vessel)
            {
                return;
            }

            if (_vessel != null)
            {
                _vessel.OnPostAutopilotUpdate -= this.OnPostAutopilotUpdateHandler;
            }

            if (vessel != null)
            {
                vessel.OnPostAutopilotUpdate += this.OnPostAutopilotUpdateHandler;
            }

            _vessel = vessel;
        }

        private void OnVesselChangeHandler(Vessel data)
        {
            this.Clean(FlightGlobals.ActiveVessel);
        }
    }
}
