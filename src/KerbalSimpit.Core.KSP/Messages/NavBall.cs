using KerbalSimpit.Common.Core;
using System.Runtime.InteropServices;

namespace KerbalSimpit.Core.KSP.Messages
{
    public static class NavBall
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct NavballMode : ISimpitMessageData
        {
            // TODO: Check if any data is actually transmitted with this message
            // If there is, it seems unnecessary
        }
    }
}
