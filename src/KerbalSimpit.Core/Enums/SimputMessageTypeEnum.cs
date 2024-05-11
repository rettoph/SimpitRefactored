using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KerbalSimpit.Core.Enums
{
    [Flags]
    public enum SimputMessageTypeEnum
    {
        Unknown = 0,
        Incoming = 1,
        Outgoing = 2
    }
}
