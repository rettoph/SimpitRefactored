using SimpitRefactored.Common.Core;
using SimpitRefactored.Common.Core.Utilities;
using System.Runtime.InteropServices;

namespace SimpitRefactored.Core.Messages
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct EchoResponse : ISimpitMessageData
    {
        public FixedBuffer Data;
    }
}
