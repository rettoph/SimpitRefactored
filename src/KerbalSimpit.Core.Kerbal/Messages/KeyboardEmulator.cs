using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace KerbalSimpit.Core.Kerbal.Messages
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct KeyboardEmulator : ISimpitMessageContent
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
