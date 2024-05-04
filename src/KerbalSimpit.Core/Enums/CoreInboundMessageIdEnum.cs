using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KerbalSimpit.Core.Enums
{
    public enum CoreInboundMessageIdEnum : byte
    {
        Synchronisation = 0,
        RegisterHandler = 8,
        CustomLog = 25,
        RequestMessage = 29
    }
}
