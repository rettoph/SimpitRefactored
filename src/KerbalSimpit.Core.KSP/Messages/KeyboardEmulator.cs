using KerbalSimpit.Common.Core;
using System;
using System.Runtime.InteropServices;

namespace KerbalSimpit.Core.KSP.Messages
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct KeyboardEmulator : ISimpitMessageData
    {
        [Flags]
        public enum ModifierFlags : byte
        {
            SHIFT_MOD = 1,
            CTRL_MOD = 2,
            ALT_MOD = 4,
            KEY_DOWN_MOD = 8,
            KEY_UP_MOD = 16
        }

        public ModifierFlags Modifier;
        public short Key;
    }
}
