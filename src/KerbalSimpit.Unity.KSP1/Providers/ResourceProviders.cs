using KerbalSimpit.Core.KSP.Enums;
using System;
using VesselMessages = KerbalSimpit.Core.KSP.Messages.Vessel;

namespace KerbalSimpit.Unity.KSP1.Providers
{
    public static partial class VesselProviders
    {
        public class ActionGroupsProvider : BaseVesselProvider<VesselMessages.Outgoing.ActionGroups>
        {
            protected override VesselMessages.Outgoing.ActionGroups GetOutgoingData()
            {
                ActionGroupFlags flags = ActionGroupFlags.None;
                if (FlightGlobals.ActiveVessel.ActionGroups[KSPActionGroup.Stage])
                {
                    flags |= ActionGroupFlags.Stage;
                }
                if (FlightGlobals.ActiveVessel.ActionGroups[KSPActionGroup.Gear])
                {
                    flags |= ActionGroupFlags.Gear;
                }
                if (FlightGlobals.ActiveVessel.ActionGroups[KSPActionGroup.Light])
                {
                    flags |= ActionGroupFlags.Light;
                }
                if (FlightGlobals.ActiveVessel.ActionGroups[KSPActionGroup.RCS])
                {
                    flags |= ActionGroupFlags.RCS;
                }
                if (FlightGlobals.ActiveVessel.ActionGroups[KSPActionGroup.SAS])
                {
                    flags |= ActionGroupFlags.SAS;
                }
                if (FlightGlobals.ActiveVessel.ActionGroups[KSPActionGroup.Brakes])
                {
                    flags |= ActionGroupFlags.Brakes;
                }
                if (FlightGlobals.ActiveVessel.ActionGroups[KSPActionGroup.Abort])
                {
                    flags |= ActionGroupFlags.Abort;
                }

                return new VesselMessages.Outgoing.ActionGroups()
                {
                    Flags = flags
                };
            }
        }

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

        public class BurnTimeProvider : BaseVesselProvider<VesselMessages.Outgoing.BurnTime>
        {
            protected override VesselMessages.Outgoing.BurnTime GetOutgoingData()
            {
                DeltaVStageInfo currentStageInfo = getCurrentStageDeltaV();
                if (currentStageInfo == null)
                {
                    return default;
                }

                return new VesselMessages.Outgoing.BurnTime()
                {
                    StageBurnTime = (float)currentStageInfo.stageBurnTime,
                    TotalBurnTime = (float)FlightGlobals.ActiveVessel.VesselDeltaV.TotalBurnTime
                };
            }
        }

        public class CustomActionGroupsProvider : BaseVesselProvider<VesselMessages.Outgoing.CustomActionGroups>
        {
            private static KSPActionGroup[] ActionGroupIDs = new KSPActionGroup[] {
                KSPActionGroup.None,
                KSPActionGroup.Custom01,
                KSPActionGroup.Custom02,
                KSPActionGroup.Custom03,
                KSPActionGroup.Custom04,
                KSPActionGroup.Custom05,
                KSPActionGroup.Custom06,
                KSPActionGroup.Custom07,
                KSPActionGroup.Custom08,
                KSPActionGroup.Custom09,
                KSPActionGroup.Custom10
            };

            protected unsafe override VesselMessages.Outgoing.CustomActionGroups GetOutgoingData()
            {
                VesselMessages.Outgoing.CustomActionGroups result = new VesselMessages.Outgoing.CustomActionGroups();

                for (int i = 1; i < ActionGroupIDs.Length; i++) //Ignoring 0 since there is no Action Group 0
                {
                    if (FlightGlobals.ActiveVessel.ActionGroups[ActionGroupIDs[i]])
                    {
                        result.Status[i / 8] |= (byte)(1 << (i % 8)); //Set the selected bit to 1
                    }
                }

                // TODO: Investigate AGX integration

                return result;
            }
        }

        public class TempLimitProvider : BaseVesselProvider<VesselMessages.Outgoing.TempLimit>
        {
            protected override VesselMessages.Outgoing.TempLimit GetOutgoingData()
            {
                double maxTempPercentage = 0.0;
                double maxSkinTempPercentage = 0.0;

                // Iterate on a copy ?
                foreach (Part part in FlightGlobals.ActiveVessel.Parts)
                {
                    maxTempPercentage = Math.Max(maxTempPercentage, 100.0 * part.temperature / part.maxTemp);
                    maxSkinTempPercentage = Math.Max(maxSkinTempPercentage, 100.0 * part.skinTemperature / part.skinMaxTemp);
                }

                //Prevent the byte to overflow in case of extremely hot vessel
                if (maxTempPercentage > 255) maxTempPercentage = 255;
                if (maxSkinTempPercentage > 255) maxSkinTempPercentage = 255;

                return new VesselMessages.Outgoing.TempLimit()
                {
                    TempLimitPercentage = (byte)Math.Round(maxTempPercentage),
                    SkinTempLimitPercentage = (byte)Math.Round(maxSkinTempPercentage),
                };
            }
        }
    }
}
