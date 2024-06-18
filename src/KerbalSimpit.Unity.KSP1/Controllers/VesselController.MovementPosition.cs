using KerbalSimpit.Common.Core.Utilities;
using KerbalSimpit.Core;
using KerbalSimpit.Core.Peers;
using System;
using VesselMessages = KerbalSimpit.Core.KSP.Messages.Vessel;

namespace KerbalSimpit.Unity.KSP1.Controllers
{
    public partial class VesselController :
        ISimpitMessageSubscriber<VesselMessages.Incoming.Rotation>,
        ISimpitMessageSubscriber<VesselMessages.Incoming.Translation>,
        ISimpitMessageSubscriber<VesselMessages.Incoming.WheelControl>,
        ISimpitMessageSubscriber<VesselMessages.Incoming.CustomAxix>,
        ISimpitMessageSubscriber<VesselMessages.Incoming.Throttle>,
        ISimpitMessageSubscriber<VesselMessages.Incoming.AutopilotMode>
    {
        private VesselMessages.Incoming.Rotation _rotation;
        private VesselMessages.Incoming.Translation _translation;
        private VesselMessages.Incoming.WheelControl _wheel;
        private VesselMessages.Incoming.CustomAxix _custom;
        private VesselMessages.Incoming.Throttle _throttle;

        private bool _lastThrottleSentIsZero = true;
        private FlightCtrlState _lastFlightCtrlState = new FlightCtrlState();

        public FlightCtrlState LastFlightCtrlState => _lastFlightCtrlState;

        private void AutopilotUpdater(FlightCtrlState fcs)
        {
            // For all axis, we need to control both the usual axis and make sure the matching custom axis are also controlled.
            // This does not seem possible in 1.12.2 without using 2 different methods to set the axis values.
            // If any change is made to the following code, please check both (e.g. add a piston to the roll axis and check that setting roll also control the piston).
            var axisGroupModule = _vessel.FindVesselModuleImplementing<AxisGroupsModule>();

            if (_rotation.Pitch != 0)
            {
                fcs.pitch = (float)_rotation.Pitch / Int16.MaxValue;
                axisGroupModule.UpdateAxisGroup(KSPAxisGroup.Pitch, (float)_rotation.Pitch / Int16.MaxValue);
            }
            if (_rotation.Roll != 0)
            {
                fcs.roll = (float)_rotation.Roll / Int16.MaxValue;
                axisGroupModule.UpdateAxisGroup(KSPAxisGroup.Roll, (float)_rotation.Roll / Int16.MaxValue);
            }
            if (_rotation.Yaw != 0)
            {
                fcs.yaw = (float)_rotation.Yaw / Int16.MaxValue;
                axisGroupModule.UpdateAxisGroup(KSPAxisGroup.Yaw, (float)_rotation.Yaw / Int16.MaxValue);
            }

            if (_translation.X != 0)
            {
                fcs.X = (float)_translation.X / Int16.MaxValue;
                axisGroupModule.UpdateAxisGroup(KSPAxisGroup.TranslateX, (float)_translation.X / Int16.MaxValue);
            }
            if (_translation.Y != 0)
            {
                fcs.Y = (float)_translation.Y / Int16.MaxValue;
                axisGroupModule.UpdateAxisGroup(KSPAxisGroup.TranslateY, (float)_translation.Y / Int16.MaxValue);
            }
            if (_translation.Z != 0)
            {
                fcs.Z = (float)_translation.Z / Int16.MaxValue;
                axisGroupModule.UpdateAxisGroup(KSPAxisGroup.TranslateZ, (float)_translation.Z / Int16.MaxValue);
            }

            if (_wheel.Steer != 0)
            {
                fcs.wheelSteer = (float)_wheel.Steer / Int16.MaxValue;
                axisGroupModule.UpdateAxisGroup(KSPAxisGroup.WheelSteer, (float)_wheel.Steer / Int16.MaxValue);
            }
            if (_wheel.Throttle != 0)
            {
                fcs.wheelThrottle = (float)_wheel.Throttle / Int16.MaxValue;
                axisGroupModule.UpdateAxisGroup(KSPAxisGroup.WheelThrottle, (float)_wheel.Throttle / Int16.MaxValue);
            }

            if (_throttle.Value != 0 || !_lastThrottleSentIsZero)
            {
                // Throttle seems to be handled differently than the other axis since when no value is set, a zero is assumed. For throttle, no value set mean the last one is used.
                // So we need to send a 0 first before stopping to send values.
                FlightInputHandler.state.mainThrottle = (float)_throttle.Value / Int16.MaxValue;

                _lastThrottleSentIsZero = (_throttle.Value == 0);
            }
            if (_custom.Custom1 != 0)
            {
                axisGroupModule.UpdateAxisGroup(KSPAxisGroup.Custom01, (float)_custom.Custom1 / Int16.MaxValue);
            }
            if (_custom.Custom2 != 0)
            {
                axisGroupModule.UpdateAxisGroup(KSPAxisGroup.Custom02, (float)_custom.Custom2 / Int16.MaxValue);
            }
            if (_custom.Custom3 != 0)
            {
                axisGroupModule.UpdateAxisGroup(KSPAxisGroup.Custom03, (float)_custom.Custom3 / Int16.MaxValue);
            }
            if (_custom.Custom4 != 0)
            {
                axisGroupModule.UpdateAxisGroup(KSPAxisGroup.Custom04, (float)_custom.Custom4 / Int16.MaxValue);
            }

            // Store the last flight command to send them in the dedicated channels
            _lastFlightCtrlState.CopyFrom(fcs);
        }


        public void Process(SimpitPeer peer, ISimpitMessage<VesselMessages.Incoming.Rotation> message)
        {
            // Bit fields:
            // pitch = 1
            // roll = 2
            // yaw = 4
            if ((message.Data.Mask & (byte)1) > 0)
            {
                _rotation.Pitch = message.Data.Pitch;
            }
            if ((message.Data.Mask & (byte)2) > 0)
            {
                _rotation.Roll = message.Data.Roll;
            }
            if ((message.Data.Mask & (byte)4) > 0)
            {
                _rotation.Yaw = message.Data.Yaw;
            }
        }

        public void Process(SimpitPeer peer, ISimpitMessage<VesselMessages.Incoming.Translation> message)
        {
            // Bit fields:
            // X = 1
            // Y = 2
            // Z = 4
            if ((message.Data.Mask & (byte)1) > 0)
            {
                _translation.X = message.Data.X;
            }
            if ((message.Data.Mask & (byte)2) > 0)
            {
                _translation.Y = message.Data.Y;
            }
            if ((message.Data.Mask & (byte)4) > 0)
            {
                _translation.Z = message.Data.Z;
            }
        }

        public void Process(SimpitPeer peer, ISimpitMessage<VesselMessages.Incoming.WheelControl> message)
        {
            // Bit fields
            // steer = 1
            // throttle = 2
            if ((message.Data.Mask & (byte)1) > 0)
            {
                _wheel.Steer = message.Data.Steer;
            }
            if ((message.Data.Mask & (byte)2) > 0)
            {
                _wheel.Throttle = message.Data.Throttle;
            }
        }

        public void Process(SimpitPeer peer, ISimpitMessage<VesselMessages.Incoming.CustomAxix> message)
        {
            if ((message.Data.Mask & (byte)1) > 0)
            {
                _custom.Custom1 = message.Data.Custom1;
            }
            if ((message.Data.Mask & (byte)2) > 0)
            {
                _custom.Custom2 = message.Data.Custom2;
            }
            if ((message.Data.Mask & (byte)4) > 0)
            {
                _custom.Custom3 = message.Data.Custom3;
            }
            if ((message.Data.Mask & (byte)8) > 0)
            {
                _custom.Custom4 = message.Data.Custom4;
            }
        }

        public void Process(SimpitPeer peer, ISimpitMessage<VesselMessages.Incoming.Throttle> message)
        {
            _throttle = message.Data;
        }

        public void Process(SimpitPeer peer, ISimpitMessage<VesselMessages.Incoming.AutopilotMode> message)
        {
            if (_vessel.Autopilot == null)
            {
                this.logger.LogDebug("Ignoring a SAS MODE Message since I could not find the autopilot");
                return;
            }

            VesselAutopilot.AutopilotMode mode = (VesselAutopilot.AutopilotMode)message.Data.Value;
            if (_vessel.Autopilot.CanSetMode(mode))
            {
                _vessel.Autopilot.SetMode(mode);

                this.logger.LogVerbose("Payload is {0}, SAS mode is {1}", mode, _vessel.Autopilot.Mode);
                this.logger.LogVerbose("payload is {0}", mode);
            }
            else
            {
                this.logger.LogWarning("Unable to set SAS mode to {0}", mode);
            }
        }

        private void OnPostAutopilotUpdateHandler(FlightCtrlState st)
        {
            this.AutopilotUpdater(st);
        }
    }
}
