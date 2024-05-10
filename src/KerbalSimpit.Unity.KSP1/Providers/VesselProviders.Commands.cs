using VesselMessages = KerbalSimpit.Core.KSP.Messages.Vessel;

namespace KerbalSimpit.Unity.KSP1.Providers
{
    public static partial class VesselProviders
    {
        public class RotationCmdProvider : BaseVesselProvider<VesselMessages.Outgoing.RotationCmd>
        {
            protected override VesselMessages.Outgoing.RotationCmd GetOutgoingData()
            {
                return new VesselMessages.Outgoing.RotationCmd()
                {
                    Pitch = (short)(this.controller.LastFlightCtrlState.pitch * short.MaxValue),
                    Yaw = (short)(this.controller.LastFlightCtrlState.yaw * short.MaxValue),
                    Roll = (short)(this.controller.LastFlightCtrlState.roll * short.MaxValue)
                };
            }
        }

        public class TranslationCmdProvider : BaseVesselProvider<VesselMessages.Outgoing.TranslationCmd>
        {
            protected override VesselMessages.Outgoing.TranslationCmd GetOutgoingData()
            {
                return new VesselMessages.Outgoing.TranslationCmd()
                {
                    X = (short)(this.controller.LastFlightCtrlState.X * short.MaxValue),
                    Y = (short)(this.controller.LastFlightCtrlState.Y * short.MaxValue),
                    Z = (short)(this.controller.LastFlightCtrlState.Z * short.MaxValue)
                };
            }
        }

        public class WheelCmdProvider : BaseVesselProvider<VesselMessages.Outgoing.WheelCmd>
        {
            protected override VesselMessages.Outgoing.WheelCmd GetOutgoingData()
            {
                return new VesselMessages.Outgoing.WheelCmd()
                {
                    Steer = (short)(this.controller.LastFlightCtrlState.wheelSteer * short.MaxValue),
                    Throttle = (short)(this.controller.LastFlightCtrlState.wheelThrottle * short.MaxValue)
                };
            }
        }

        public class ThrottleCmdProvider : BaseVesselProvider<VesselMessages.Outgoing.ThrottleCmd>
        {
            protected override VesselMessages.Outgoing.ThrottleCmd GetOutgoingData()
            {
                return new VesselMessages.Outgoing.ThrottleCmd()
                {
                    Value = (short)(this.controller.LastFlightCtrlState.mainThrottle * short.MaxValue)
                };
            }
        }
    }
}
