using KerbalSimpit.Common.Core;
using System.Runtime.InteropServices;

namespace KerbalSimpit.Core.KSP.Messages
{
    public static class Warp
    {
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
    }
}
