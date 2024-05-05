using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KerbalSimpit.Core.Kerbal.Enums
{
    [Flags]
    public enum ActionGroupFlags : byte
    {
        Stage = 1,
        Gear = 2,
        Light = 4,
        RCS = 8,
        SAS = 16,
        Brakes = 32,
        Abort = 64
    }
}
