using KerbalSimpit.Common.Core;
using KerbalSimpit.Common.Core.Utilities;
using System.Runtime.InteropServices;

namespace KerbalSimpit.Core.Messages
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct EchoResponse : ISimpitMessageData
    {
        public FixedBuffer Data;
    }
}
