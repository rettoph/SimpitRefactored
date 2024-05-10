using VesselMessages = KerbalSimpit.Core.KSP.Messages.Vessel;

namespace KerbalSimpit.Unity.KSP1.Providers
{
    public static partial class VesselProviders
    {
        public class DeltaVEnvProvider : BaseVesselProvider<VesselMessages.Outgoing.DeltaVEnv>
        {
            protected override VesselMessages.Outgoing.DeltaVEnv GetOutgoingData()
            {
                DeltaVStageInfo currentStageInfo = getCurrentStageDeltaV();
                if (currentStageInfo == null)
                {
                    return default;
                }

                return new VesselMessages.Outgoing.DeltaVEnv()
                {
                    StageDeltaVASL = (float)currentStageInfo.deltaVatASL,
                    StageDeltaVVac = (float)currentStageInfo.deltaVinVac,
                    TotalDeltaVASL = (float)FlightGlobals.ActiveVessel.VesselDeltaV.TotalDeltaVASL,
                    TotalDeltaVVac = (float)FlightGlobals.ActiveVessel.VesselDeltaV.TotalDeltaVVac,
                };
            }
        }

        public class DeltaVProvider : BaseVesselProvider<VesselMessages.Outgoing.DeltaV>
        {
            protected override VesselMessages.Outgoing.DeltaV GetOutgoingData()
            {
                DeltaVStageInfo currentStageInfo = getCurrentStageDeltaV();
                if (currentStageInfo == null)
                {
                    return default;
                }

                return new VesselMessages.Outgoing.DeltaV()
                {
                    StageDeltaV = (float)currentStageInfo.deltaVActual,
                    TotalDeltaV = (float)FlightGlobals.ActiveVessel.VesselDeltaV.TotalDeltaVActual
                };
            }
        }
    }
}
