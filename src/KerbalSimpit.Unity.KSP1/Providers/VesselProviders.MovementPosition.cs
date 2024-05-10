using KerbalSimpit.Core.KSP.Enums;
using KerbalSimpit.Core.KSP.Utilities;
using System;
using UnityEngine;
using VesselMessages = KerbalSimpit.Core.KSP.Messages.Vessel;

namespace KerbalSimpit.Unity.KSP1.Providers
{
    public static partial class VesselProviders
    {
        public class AltitudeProvider : BaseVesselProvider<VesselMessages.Outgoing.Altitude>
        {
            protected override VesselMessages.Outgoing.Altitude GetOutgoingData()
            {
                return new VesselMessages.Outgoing.Altitude()
                {
                    Alt = (float)FlightGlobals.ActiveVessel.altitude,
                    SurfAlt = (float)FlightGlobals.ActiveVessel.radarAltitude
                };
            }
        }

        public class ApsidesProvider : BaseVesselProvider<VesselMessages.Outgoing.Apsides>
        {
            protected override VesselMessages.Outgoing.Apsides GetOutgoingData()
            {
                return new VesselMessages.Outgoing.Apsides()
                {
                    Apoapsis = (float)FlightGlobals.ActiveVessel.orbit.ApA,
                    Periapsis = (float)FlightGlobals.ActiveVessel.orbit.PeA
                };
            }
        }

        public class ApsidesTimeProvider : BaseVesselProvider<VesselMessages.Outgoing.ApsidesTime>
        {
            protected override VesselMessages.Outgoing.ApsidesTime GetOutgoingData()
            {
                return new VesselMessages.Outgoing.ApsidesTime()
                {
                    Apoapsis = (int)FlightGlobals.ActiveVessel.orbit.timeToAp,
                    Periapsis = (int)FlightGlobals.ActiveVessel.orbit.timeToPe
                };
            }
        }

        public class VelocityProvider : BaseVesselProvider<VesselMessages.Outgoing.Velocity>
        {
            protected override VesselMessages.Outgoing.Velocity GetOutgoingData()
            {
                return new VesselMessages.Outgoing.Velocity()
                {
                    Orbital = (float)FlightGlobals.ActiveVessel.obt_speed,
                    Surface = (float)FlightGlobals.ActiveVessel.srfSpeed,
                    Vertical = (float)FlightGlobals.ActiveVessel.verticalSpeed,
                };
            }
        }

        public class RotationProvider : BaseVesselProvider<VesselMessages.Outgoing.Rotation>
        {
            protected override VesselMessages.Outgoing.Rotation GetOutgoingData()
            {
                // Code from KSPIO to compute angles and velocities https://github.com/zitron-git/KSPSerialIO/blob/062d97e892077ea14737f5e79268c0c4d067f5b6/KSPSerialIO/KSPIO.cs#L929-L971
                Vector3d CoM, north, up, east;
                CoM = FlightGlobals.ActiveVessel.CoM;
                up = (CoM - FlightGlobals.ActiveVessel.mainBody.position).normalized;
                north = Vector3d.Exclude(up, (FlightGlobals.ActiveVessel.mainBody.position + FlightGlobals.ActiveVessel.mainBody.transform.up * (float)FlightGlobals.ActiveVessel.mainBody.Radius) - CoM).normalized;
                east = Vector3d.Cross(up, north);

                Vector3d attitude = Quaternion.Inverse(Quaternion.Euler(90, 0, 0) * Quaternion.Inverse(FlightGlobals.ActiveVessel.GetTransform().rotation) * Quaternion.LookRotation(north, up)).eulerAngles;

                WorldVecToNavHeading(FlightGlobals.ActiveVessel, FlightGlobals.ActiveVessel.srf_velocity.normalized, out float surfaceVelocityHeading, out float surfaceVelocityPitch);
                WorldVecToNavHeading(FlightGlobals.ActiveVessel, FlightGlobals.ActiveVessel.obt_velocity.normalized, out float orbitalVelocityHeading, out float orbitalVelocityPitch);

                return new VesselMessages.Outgoing.Rotation()
                {
                    Roll = (float)((attitude.z > 180) ? (attitude.z - 360.0) : attitude.z),
                    Pitch = (float)((attitude.x > 180) ? (360.0 - attitude.x) : -attitude.x),
                    Heading = (float)attitude.y,
                    SurfaceVelocityHeading = surfaceVelocityHeading,
                    SurfaceVelocityPitch = surfaceVelocityPitch,
                    OrbitalVelocityHeading = orbitalVelocityHeading,
                    OrbitalVelocityPitch = orbitalVelocityPitch
                };
            }
        }

        public class OrbitInfoProvider : BaseVesselProvider<VesselMessages.Outgoing.OrbitInfo>
        {
            protected override VesselMessages.Outgoing.OrbitInfo GetOutgoingData()
            {
                return new VesselMessages.Outgoing.OrbitInfo()
                {
                    Eccentricity = (float)FlightGlobals.ActiveVessel.orbit.eccentricity,
                    SemiMajorAxis = (float)FlightGlobals.ActiveVessel.orbit.semiMajorAxis,
                    Inclination = (float)FlightGlobals.ActiveVessel.orbit.inclination,
                    LongAscendingNode = (float)FlightGlobals.ActiveVessel.orbit.LAN,
                    ArgPeriapsis = (float)FlightGlobals.ActiveVessel.orbit.argumentOfPeriapsis,
                    TrueAnomaly = (float)FlightGlobals.ActiveVessel.orbit.trueAnomaly,
                    MeanAnomaly = (float)FlightGlobals.ActiveVessel.orbit.meanAnomaly,
                    Period = (float)FlightGlobals.ActiveVessel.orbit.period,
                };
            }
        }

        public class AirspeedProvider : BaseVesselProvider<VesselMessages.Outgoing.Airspeed>
        {
            protected override VesselMessages.Outgoing.Airspeed GetOutgoingData()
            {
                return new VesselMessages.Outgoing.Airspeed()
                {
                    IndicatedAirSpeed = (float)FlightGlobals.ActiveVessel.indicatedAirSpeed,
                    MachNumber = (float)FlightGlobals.ActiveVessel.mach,
                    GeeForce = (float)FlightGlobals.ActiveVessel.geeForce,
                };
            }
        }

        public class ManeuverProvider : BaseVesselProvider<VesselMessages.Outgoing.Maneuver>
        {
            protected override VesselMessages.Outgoing.Maneuver GetOutgoingData()
            {
                var maneuver = new VesselMessages.Outgoing.Maneuver();
                maneuver.TimeToNextManeuver = 0.0f;
                maneuver.DeltaVNextManeuver = 0.0f;
                maneuver.DurationNextManeuver = 0.0f;
                maneuver.DeltaVTotal = 0.0f;
                maneuver.HeadingNextManeuver = 0.0f;
                maneuver.PitchNextManeuver = 0.0f;

                if (FlightGlobals.ActiveVessel.patchedConicSolver != null)
                {
                    if (FlightGlobals.ActiveVessel.patchedConicSolver.maneuverNodes != null)
                    {
                        System.Collections.Generic.List<ManeuverNode> maneuverNodes = FlightGlobals.ActiveVessel.patchedConicSolver.maneuverNodes;

                        if (maneuverNodes.Count > 0)
                        {
                            maneuver.TimeToNextManeuver = (float)(maneuverNodes[0].UT - Planetarium.GetUniversalTime());
                            maneuver.DeltaVNextManeuver = (float)maneuverNodes[0].GetPartialDv().magnitude;

                            WorldVecToNavHeading(FlightGlobals.ActiveVessel, maneuverNodes[0].GetBurnVector(maneuverNodes[0].patch), out float headingNextManeuver, out float pitchNextManeuver);
                            maneuver.HeadingNextManeuver = headingNextManeuver;
                            maneuver.PitchNextManeuver = pitchNextManeuver;

                            DeltaVStageInfo currentStageInfo = getCurrentStageDeltaV();
                            if (currentStageInfo != null)
                            {
                                //Old method, use a simple crossmultiplication to compute the estimated burn time based on the current stage only
                                //myManeuver.durationNextManeuver = (float)(maneuvers[0].DeltaV.magnitude * currentStageInfo.stageBurnTime) / currentStageInfo.deltaVActual;

                                // The estimation based on the startBurnIn seems to be more accurate than using the previous method of crossmultiplication
                                maneuver.DurationNextManeuver = (float)((maneuverNodes[0].UT - Planetarium.GetUniversalTime() - maneuverNodes[0].startBurnIn) * 2);
                            }

                            foreach (ManeuverNode maneuverNode in maneuverNodes)
                            {
                                maneuver.DeltaVTotal += (float)maneuverNode.DeltaV.magnitude;
                            }
                        }
                    }
                }

                return maneuver;
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

        public class SASInfoProvider : BaseVesselProvider<VesselMessages.Outgoing.SASInfo>
        {
            protected override bool ShouldCleanOutgoingData()
            {
                if (base.ShouldCleanOutgoingData() == false)
                {
                    return false;
                }

                return FlightGlobals.ActiveVessel.Autopilot != null;
            }

            protected override VesselMessages.Outgoing.SASInfo GetOutgoingData()
            {
                VesselMessages.Outgoing.SASInfo sasInfo = new VesselMessages.Outgoing.SASInfo();

                if (FlightGlobals.ActiveVessel.Autopilot.Enabled)
                {
                    sasInfo.CurrentSASMode = (AutoPilotModeEnum)FlightGlobals.ActiveVessel.Autopilot.Mode;
                }
                else
                {
                    sasInfo.CurrentSASMode = AutoPilotModeEnum.Disabled;
                }

                sasInfo.SASModeAvailability = 0;
                foreach (VesselAutopilot.AutopilotMode i in Enum.GetValues(typeof(VesselAutopilot.AutopilotMode)))
                {
                    if (FlightGlobals.ActiveVessel.Autopilot.CanSetMode(i))
                    {
                        sasInfo.SASModeAvailability |= AutoPilotHelper.ModeEnumToFlag((AutoPilotModeEnum)i);
                    }
                }

                return sasInfo;
            }
        }

        // Convert a direction given in world space v into a heading and a pitch, relative to the vessel passed as a paramater
        private static void WorldVecToNavHeading(Vessel activeVessel, Vector3d v, out float heading, out float pitch)
        {
            Vector3d CoM, north, up, east;
            CoM = activeVessel.CoM;
            up = (CoM - activeVessel.mainBody.position).normalized;
            north = Vector3d.Exclude(up, (activeVessel.mainBody.position + activeVessel.mainBody.transform.up * (float)activeVessel.mainBody.Radius) - CoM).normalized;
            east = Vector3d.Cross(up, north);

            // Code from KSPIO to do angle conversions : https://github.com/zitron-git/KSPSerialIO/blob/062d97e892077ea14737f5e79268c0c4d067f5b6/KSPSerialIO/KSPIO.cs#L1301-L1313
            pitch = (float)-((Vector3d.Angle(up, v)) - 90.0f);
            Vector3d progradeFlat = Vector3d.Exclude(up, v);
            float NAngle = (float)Vector3d.Angle(north, progradeFlat);
            float EAngle = (float)Vector3d.Angle(east, progradeFlat);
            if (EAngle < 90)
                heading = NAngle;
            else
                heading = -NAngle + 360;
        }

        //Return the DeltaVStageInfo of the first stage to consider for deltaV and burn time computation
        //Can return null when no deltaV is available (for instance in EVA).
        private static DeltaVStageInfo getCurrentStageDeltaV()
        {
            if (FlightGlobals.ActiveVessel.VesselDeltaV == null)
            {
                return null; //This happen in EVA for instance.
            }
            DeltaVStageInfo currentStageInfo = null;

            try
            {
                if (FlightGlobals.ActiveVessel.currentStage == FlightGlobals.ActiveVessel.VesselDeltaV.OperatingStageInfo.Count)
                {
                    // Rocket has not taken off, use first stage with deltaV (to avoid stage of only stabilizer)
                    for (int i = FlightGlobals.ActiveVessel.VesselDeltaV.OperatingStageInfo.Count - 1; i >= 0; i--)
                    {
                        currentStageInfo = FlightGlobals.ActiveVessel.VesselDeltaV.GetStage(i);
                        if (currentStageInfo.deltaVActual > 0)
                        {
                            break;
                        }
                    }
                }
                else
                {
                    currentStageInfo = FlightGlobals.ActiveVessel.VesselDeltaV.GetStage(FlightGlobals.ActiveVessel.currentStage);
                }
            }
            catch (NullReferenceException)
            {
                // This happens when reverting a flight.
                // FlightGlobals.ActiveVessel.VesselDeltaV.OperatingStageInfo is not null but using it produce a
                // NullReferenceException in KSP code. This is probably due to the fact that the rocket is not fully initialized.
            }

            return currentStageInfo;
        }
    }
}
