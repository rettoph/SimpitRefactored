using KerbalSimpit.Core.KSP.Enums;
using System.Linq;
using System.Runtime.InteropServices;

namespace KerbalSimpit.Core.KSP.Messages
{
    public static class Vessel
    {
        public static class Outgoing
        {
            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            public struct Altitude : ISimpitMessageData
            {
                public float Alt { get; set; }
                public float SurfAlt { get; set; }
            }

            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            public struct Apsides : ISimpitMessageData
            {
                public float Periapsis { get; set; }
                public float Apoapsis { get; set; }
            }

            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            public struct ApsidesTime : ISimpitMessageData
            {
                public int Periapsis { get; set; }
                public int Apoapsis { get; set; }
            }

            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            public struct Velocity : ISimpitMessageData
            {
                public float Orbital { get; set; }
                public float Surface { get; set; }
                public float Vertical { get; set; }
            }

            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            public struct Rotation : ISimpitMessageData
            {
                public float Heading { get; set; }
                public float Pitch { get; set; }
                public float Roll { get; set; }
                public float OrbitalVelocityHeading { get; set; }
                public float OrbitalVelocityPitch { get; set; }
                public float SurfaceVelocityHeading { get; set; }
                public float SurfaceVelocityPitch { get; set; }
            }

            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            public struct OrbitInfo : ISimpitMessageData
            {
                public float Eccentricity { get; set; }
                public float SemiMajorAxis { get; set; }
                public float Inclination { get; set; }
                public float LongAscendingNode { get; set; }
                public float ArgPeriapsis { get; set; }
                public float TrueAnomaly { get; set; }
                public float MeanAnomaly { get; set; }
                public float Period { get; set; }
            }

            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            public struct Airspeed : ISimpitMessageData
            {
                public float IAS { get; set; }
                public float MachNumber { get; set; }
                public float GForces { get; set; }
            }

            public struct Maneuver : ISimpitMessageData
            {
                public float TimeToNextManeuver { get; set; }
                public float DeltaVNextManeuver { get; set; }
                public float DurationNextManeuver { get; set; }
                public float DeltaVTotal { get; set; }
                public float HeadingNextManeuver { get; set; }
                public float PitchNextManeuver { get; set; }
            }

            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            public struct SASInfo : ISimpitMessageData
            {
                public AutoPilotModeEnum CurrentSASMode { get; set; }
                public ushort SASModeAvailability { get; set; }
            }

            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            public struct RotationCmd : ISimpitMessageData
            {
                public short Pitch { get; set; }
                public short Roll { get; set; }
                public short Yaw { get; set; }
                public byte Mask { get; set; }
            }

            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            public struct TranslationCmd : ISimpitMessageData
            {
                public short X { get; set; }
                public short Y { get; set; }
                public short Z { get; set; }
                public byte Mask { get; set; }
            }

            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            public struct WheelCmd : ISimpitMessageData
            {
                public short Steer { get; set; }
                public short Throttle { get; set; }
                public byte Mask { get; set; }
            }

            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            public struct ThrottleCmd : ISimpitMessageData
            {
                public short Throttle { get; set; }
            }

            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            public struct ActionGroups : ISimpitMessageData
            {
                public ActionGroupFlags Flags { get; set; }
            }

            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            public struct DeltaV : ISimpitMessageData
            {
                public float StageDeltaV { get; set; }
                public float TotalDeltaV { get; set; }
            }

            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            public struct DeltaVEnv : ISimpitMessageData
            {
                public float StageDeltaVASL { get; set; }
                public float TotalDeltaVASL { get; set; }
                public float StageDeltaVVac { get; set; }
                public float TotalDeltaVVac { get; set; }
            }

            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            public struct BurnTime : ISimpitMessageData
            {
                public float StageBurnTime { get; set; }
                public float TotalBurnTime { get; set; }
            }

            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            public unsafe struct CustomActionGroups : ISimpitMessageData
            {
                private const int Length = 32;
                public fixed byte Status[CustomActionGroups.Length];

                public bool Equals(CustomActionGroups obj)
                {
                    for (int i = 0; i < CustomActionGroups.Length; i++)
                    {
                        if (this.Status[i] != obj.Status[i])
                        {
                            return false;
                        }
                    }
                    return true;
                }

                internal static void Serialize(CustomActionGroups input, SimpitStream output)
                {
                    for (int i = 0; i < CustomActionGroups.Length; i++)
                    {
                        output.Write(input.Status[i]);
                    }
                }
            }

            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            public struct TempLimit : ISimpitMessageData
            {
                public byte TempLimitPercentage { get; set; }
                public byte SkinTempLimitPercentage { get; set; }
            }
        }

        public static class Incoming
        {
            public struct CustomActionGroupEnable : ISimpitMessageData
            {
                public byte[] GroupIds { get; set; }

                internal static CustomActionGroupEnable Deserialize(SimpitStream input)
                {
                    return new CustomActionGroupEnable()
                    {
                        GroupIds = input.ReadAll(out int offset, out int count).Skip(offset).Take(count).ToArray()
                    };
                }
            }

            public struct CustomActionGroupDisable : ISimpitMessageData
            {
                public byte[] GroupIds { get; set; }

                internal static CustomActionGroupDisable Deserialize(SimpitStream input)
                {
                    return new CustomActionGroupDisable()
                    {
                        GroupIds = input.ReadAll(out int offset, out int count).Skip(offset).Take(count).ToArray()
                    };
                }
            }

            public struct CustomActionGroupToggle : ISimpitMessageData
            {
                public byte[] GroupIds { get; set; }

                internal static CustomActionGroupToggle Deserialize(SimpitStream input)
                {
                    return new CustomActionGroupToggle()
                    {
                        GroupIds = input.ReadAll(out int offset, out int count).Skip(offset).Take(count).ToArray()
                    };
                }
            }

            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            public struct ActionGroupActivate : ISimpitMessageData
            {
                public ActionGroupFlags Flags { get; set; }
            }

            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            public struct ActionGroupDeactivate : ISimpitMessageData
            {
                public ActionGroupFlags Flags { get; set; }
            }

            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            public struct ActionGroupToggle : ISimpitMessageData
            {
                public ActionGroupFlags Flags { get; set; }
            }

            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            public struct Rotation : ISimpitMessageData
            {
                public short Pitch { get; set; }
                public short Roll { get; set; }
                public short Yaw { get; set; }
                public byte Mask { get; set; }
            }

            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            public struct Translation : ISimpitMessageData
            {
                public short X { get; set; }
                public short Y { get; set; }
                public short Z { get; set; }
                public byte Mask { get; set; }
            }

            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            public struct WheelControl : ISimpitMessageData
            {
                public short Steer { get; set; }
                public short Throttle { get; set; }
                public byte Mask { get; set; }
            }

            [StructLayout(LayoutKind.Sequential, Pack = 2)]
            public struct Throttle : ISimpitMessageData
            {
                public short Value { get; set; }
            }

            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            public struct AutopilotMode : ISimpitMessageData
            {
                public AutoPilotModeEnum Value { get; set; }
            }

            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            public struct CameraMode : ISimpitMessageData
            {
                public enum ValueEnum : byte
                {
                    FlightMode = 1,
                    Auto = 2,
                    Free = 3,
                    Orbital = 4,
                    Chase = 5,
                    Locked = 6,
                    IVAMode = 10,
                    PlanetaryMode = 20,
                    NextCamera = 50,
                    PreviousCamera = 51,
                    NextCameraModeState = 52,
                    PreviousCameraModeState = 53
                }

                public ValueEnum Value { get; set; }
            }

            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            public struct CameraRotation : ISimpitMessageData
            {
                public short Pitch;
                public short Roll;
                public short Yaw;
                public short Zoom;
                public byte Mask;
            }

            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            public struct CameraTranslation : ISimpitMessageData
            {
                public short X;
                public short Y;
                public short Z;
                public byte Mask;
            }

            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            public struct WarpChange : ISimpitMessageData
            {
                public enum RateEnum : byte
                {
                    WarpRate1 = 0,
                    WarpRate2 = 1,
                    WarpRate3 = 2,
                    WarpRate4 = 3,
                    WarpRate5 = 4,
                    WarpRate6 = 5,
                    WarpRate7 = 6,
                    WarpRate8 = 7,
                    WarpRatePhys1 = 8,
                    WarpRatePhys2 = 9,
                    WarpRatePhys3 = 10,
                    WarpRatePhys4 = 11,
                    WarpRateUp = 12,
                    WarpRateDown = 13,
                    WarpCancelAutoWarp = 255,
                }

                public RateEnum Rate { get; set; }
            }

            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            public struct TimewarpTo : ISimpitMessageData
            {
                public enum InstanceEnum : byte
                {
                    TimewarpToNow = 0,
                    TimewarpToManeuver = 1,
                    TimewarpToBurn = 2,
                    TimewarpToNextSOI = 3,
                    TimewarpToApoapsis = 4,
                    TimewarpToPeriapsis = 5,
                    TimewarpToNextMorning = 6
                }

                public InstanceEnum Instant;
                public float Delay; // negative for warping before the instant
            }

            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            public struct CustomAxix : ISimpitMessageData
            {
                public short Custom1 { get; set; }
                public short Custom2 { get; set; }
                public short Custom3 { get; set; }
                public short Custom4 { get; set; }
                public byte Mask { get; set; }
            }

            public struct NavballMode : ISimpitMessageData
            {
                // TODO: Check if any data is actually transmitted with this message
                // If there is, it seems unnecessary
            }
        }
    }
}
