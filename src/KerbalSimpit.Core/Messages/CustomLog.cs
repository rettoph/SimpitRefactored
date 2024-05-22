using KerbalSimpit.Core.Utilities;
using System;
using System.Runtime.InteropServices;

namespace KerbalSimpit.Core.Messages
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct CustomLog : ISimpitMessageData
    {
        [Flags]
        public enum FlagsEnum : byte
        {
            None = 0,
            Verbose = 1,
            PrintToScreen = 2,
            NoHeader = 4
        }

        public FlagsEnum Flags { get; set; }
        public FixedString Value { get; set; }
    }
}
