using KerbalSimpit.Unity.Common;

namespace KerbalSimpit.Unity.KSP1.Controllers
{
    public partial class VesselController : SimpitBehaviour
    {
        private Vessel _vessel;

        public override void Start()
        {
            base.Start();

            GameEvents.onVesselChange.Add(this.OnVesselChangeHandler);

            this.Clean(FlightGlobals.ActiveVessel);
        }

        public override void OnDestroy()
        {
            base.OnDestroy();

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
