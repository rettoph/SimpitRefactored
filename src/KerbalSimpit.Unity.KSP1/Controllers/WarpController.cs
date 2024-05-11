using KerbalSimpit.Core;
using KerbalSimpit.Core.KSP.Messages;
using KerbalSimpit.Core.Peers;
using KerbalSimpit.Core.Utilities;
using KerbalSimpit.Unity.Common;
using KerbalSimpit.Unity.KSP1.Helpers;
using System;
using System.Collections.Generic;

namespace KerbalSimpit.Unity.KSP1.Controllers
{
    [KSPAddon(KSPAddon.Startup.Flight, false)]
    public class WarpController : SimpitBehaviour,
        ISimpitMessageSubscriber<Warp.WarpChange>,
        ISimpitMessageSubscriber<Warp.TimewarpTo>
    {
        private const bool USE_INSTANT_WARP = false;
        private const bool DISPLAY_MESSAGE = false; //When true, each call to Timewarp.SetRate crashes KSP on my computer

        public void Process(SimpitPeer peer, ISimpitMessage<Warp.WarpChange> message)
        {
            this.logger.LogVerbose("Receveid TW command {0}", message.Data.Rate);
            int currentRate = TimeWarp.CurrentRateIndex;
            switch (message.Data.Rate)
            {
                case Warp.WarpChange.RateEnum.WarpRate1:
                    TimeWarp.SetRate(0, USE_INSTANT_WARP, DISPLAY_MESSAGE);
                    break;
                case Warp.WarpChange.RateEnum.WarpRate2:
                case Warp.WarpChange.RateEnum.WarpRate3:
                case Warp.WarpChange.RateEnum.WarpRate4:
                case Warp.WarpChange.RateEnum.WarpRate5:
                case Warp.WarpChange.RateEnum.WarpRate6:
                case Warp.WarpChange.RateEnum.WarpRate7:
                case Warp.WarpChange.RateEnum.WarpRate8:
                    SetWarpRate((int)message.Data.Rate, false);
                    break;
                case Warp.WarpChange.RateEnum.WarpRatePhys1:
                    TimeWarp.SetRate(0, USE_INSTANT_WARP, DISPLAY_MESSAGE);
                    break;
                case Warp.WarpChange.RateEnum.WarpRatePhys2:
                case Warp.WarpChange.RateEnum.WarpRatePhys3:
                case Warp.WarpChange.RateEnum.WarpRatePhys4:
                    SetWarpRate(message.Data.Rate - Warp.WarpChange.RateEnum.WarpRatePhys1, true);
                    break;
                case Warp.WarpChange.RateEnum.WarpRateUp:
                    int MaxRateIndex = 0;
                    if (TimeWarp.fetch.Mode == TimeWarp.Modes.HIGH)
                    {
                        MaxRateIndex = TimeWarp.fetch.warpRates.Length;
                    }
                    else
                    {
                        MaxRateIndex = TimeWarp.fetch.physicsWarpRates.Length;
                    }

                    if (currentRate < MaxRateIndex)
                    {
                        TimeWarp.SetRate(currentRate + 1, USE_INSTANT_WARP, DISPLAY_MESSAGE);
                    }
                    else
                    {
                        this.logger.LogDebug("Already at max warp rate.");
                    }
                    break;
                case Warp.WarpChange.RateEnum.WarpRateDown:
                    if (currentRate > 0)
                    {
                        TimeWarp.SetRate(currentRate - 1, USE_INSTANT_WARP, DISPLAY_MESSAGE);
                    }
                    else
                    {
                        this.logger.LogDebug("Already at min warp rate.");
                    }
                    break;
                case Warp.WarpChange.RateEnum.WarpCancelAutoWarp:
                    TimeWarp.fetch.CancelAutoWarp();
                    TimeWarp.SetRate(0, USE_INSTANT_WARP, DISPLAY_MESSAGE);
                    break;
                default:
                    this.logger.LogDebug("Received an unrecognized Warp control command : {0}. Ignoring it.", message.Data.Rate);
                    break;
            }
        }

        public void Process(SimpitPeer peer, ISimpitMessage<Warp.TimewarpTo> message)
        {
            // In those cases, we need to timewarp to a given time. Let's compute this time (UT)
            double timeToWarp = -1;

            switch (message.Data.Instant)
            {
                case Warp.TimewarpTo.InstanceEnum.TimewarpToNow:
                    timeToWarp = Planetarium.GetUniversalTime();
                    break;
                case Warp.TimewarpTo.InstanceEnum.TimewarpToManeuver:
                    if (FlightGlobals.ActiveVessel.patchedConicSolver != null)
                    {
                        if (FlightGlobals.ActiveVessel.patchedConicSolver.maneuverNodes != null)
                        {
                            List<ManeuverNode> maneuvers = FlightGlobals.ActiveVessel.patchedConicSolver.maneuverNodes;

                            if (maneuvers.Count > 0 && maneuvers[0] != null)
                            {
                                timeToWarp = maneuvers[0].UT;
                            }
                            else
                            {
                                this.logger.LogDebug("There is no maneuver to warp to.");
                            }
                        }
                    }
                    break;
                case Warp.TimewarpTo.InstanceEnum.TimewarpToBurn:
                    if (FlightGlobals.ActiveVessel.patchedConicSolver != null)
                    {
                        if (FlightGlobals.ActiveVessel.patchedConicSolver.maneuverNodes != null)
                        {
                            List<ManeuverNode> maneuvers = FlightGlobals.ActiveVessel.patchedConicSolver.maneuverNodes;

                            if (maneuvers.Count > 0 && maneuvers[0] != null)
                            {
                                timeToWarp = Planetarium.GetUniversalTime() + maneuvers[0].startBurnIn;
                            }
                            else
                            {
                                this.logger.LogDebug("There is no maneuver to warp to.");
                            }
                        }
                    }
                    break;
                case Warp.TimewarpTo.InstanceEnum.TimewarpToNextSOI:
                    Orbit.PatchTransitionType orbitType = FlightGlobals.ActiveVessel.GetOrbit().patchEndTransition;

                    if (orbitType == Orbit.PatchTransitionType.ENCOUNTER ||
                        orbitType == Orbit.PatchTransitionType.ESCAPE)
                    {
                        timeToWarp = FlightGlobals.ActiveVessel.GetOrbit().EndUT;
                    }
                    else
                    {
                        this.logger.LogDebug("There is no SOI change to warp to. Orbit type : " + orbitType);
                    }
                    break;
                case Warp.TimewarpTo.InstanceEnum.TimewarpToApoapsis:
                    double timeToApoapsis = FlightGlobals.ActiveVessel.GetOrbit().timeToAp;
                    if (Double.IsNaN(timeToApoapsis) || Double.IsInfinity(timeToApoapsis))
                    {
                        //This can happen in an escape trajectory for instance
                        this.logger.LogDebug("Cannot TW to apoasis since there is no apoapsis");
                    }
                    else
                    {
                        timeToWarp = (Planetarium.GetUniversalTime() + timeToApoapsis);
                    }
                    break;
                case Warp.TimewarpTo.InstanceEnum.TimewarpToPeriapsis:
                    double timeToPeriapsis = FlightGlobals.ActiveVessel.GetOrbit().timeToPe;
                    if (Double.IsNaN(timeToPeriapsis) || Double.IsInfinity(timeToPeriapsis))
                    {
                        //Can this happen ?
                        this.logger.LogDebug("Cannot TW to apoasis since there is no periapsis");
                    }
                    else
                    {
                        timeToWarp = (Planetarium.GetUniversalTime() + timeToPeriapsis);
                    }
                    break;
                case Warp.TimewarpTo.InstanceEnum.TimewarpToNextMorning:
                    Vessel vessel = FlightGlobals.ActiveVessel;

                    if (vessel.situation == Vessel.Situations.LANDED ||
                        vessel.situation == Vessel.Situations.SPLASHED ||
                        vessel.situation == Vessel.Situations.PRELAUNCH)
                    {
                        double timeToMorning = OrbitalHelper.TimeToDaylight(vessel.latitude, vessel.longitude, vessel.mainBody);
                        timeToWarp = (Planetarium.GetUniversalTime() + timeToMorning);
                    }
                    else
                    {
                        this.logger.LogDebug("Cannot warp to next morning if not landed or splashed");
                    }
                    break;
                default:
                    this.logger.LogDebug("received an unrecognized WarpTO command : {0}. Ignoring it.", message.Data.Instant);
                    break;
            }

            timeToWarp = timeToWarp + message.Data.Delay;
            this.logger.LogVerbose("TW to UT {0}. Which is {1}s away", timeToWarp, timeToWarp - Planetarium.GetUniversalTime());

            if (timeToWarp < 0)
            {
                this.logger.LogDebug("Cannot compute the time to timewarp to. Ignoring TW command {0}", message.Data.Instant);
            }
            else if (timeToWarp < Planetarium.GetUniversalTime())
            {
                this.logger.LogDebug("cannot warp in the past. Ignoring TW command {0}", message.Data.Delay);
            }
            else
            {
                TimeWarp.fetch.WarpTo(timeToWarp);
            }
        }

        private void SetWarpRate(int rateIndex, bool physical)
        {
            if (physical)
            {
                if (TimeWarp.fetch.Mode != TimeWarp.Modes.LOW)
                {
                    this.logger.LogDebug("Ignore a timewarp for physical rate since we are in non-physical mode");
                }
                else
                {
                    if (rateIndex < TimeWarp.fetch.physicsWarpRates.Length)
                    {
                        TimeWarp.SetRate(rateIndex, USE_INSTANT_WARP, DISPLAY_MESSAGE);
                    }
                    else
                    {
                        this.logger.LogDebug("Cannot find a physical warp speed at index: {0}", rateIndex);
                    }
                }
            }
            else
            {
                if (TimeWarp.fetch.Mode != TimeWarp.Modes.HIGH)
                {
                    this.logger.LogDebug("Simpit : ignore a timewarp for non-physical rate since we are in physical mode");
                }
                else
                {
                    if (rateIndex < TimeWarp.fetch.warpRates.Length)
                    {
                        TimeWarp.SetRate(rateIndex, USE_INSTANT_WARP, DISPLAY_MESSAGE);
                    }
                    else
                    {
                        this.logger.LogDebug("Simpit : Cannot find a warp speed at index: {0}", rateIndex);
                    }
                }
            }
        }
    }
}
