using System.Runtime.InteropServices;

namespace KerbalSimpit.Core.KSP.Messages
{
    public static class Camera
    {
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
    }
}
