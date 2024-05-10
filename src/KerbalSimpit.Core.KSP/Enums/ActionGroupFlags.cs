using System;

namespace KerbalSimpit.Core.KSP.Enums
{
    [Flags]
    public enum ActionGroupFlags : byte
    {
        None = 0,
        Stage = 1,
        Gear = 2,
        Light = 4,
        RCS = 8,
        SAS = 16,
        Brakes = 32,
        Abort = 64
    }
}
