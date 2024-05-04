using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KerbalSimpit.Core.Enums
{
    public enum CoreOutboundMessageIdEnum : byte
    {
        Synchronisation = 0,
        HandshakeMessage = Synchronisation
    }
}
