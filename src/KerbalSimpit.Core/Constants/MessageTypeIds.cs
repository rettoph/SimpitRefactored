using KerbalSimpit.Core.Enums;
using KerbalSimpit.Core.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KerbalSimpit.Core.Constants
{
    internal static class MessageTypeIds
    {
        public static class Incoming
        {
            public static readonly byte Synchronisation = 0;
            public static readonly byte RegisterHandler = 8;
            public static readonly byte DeregisterHandler = 9;
            public static readonly byte CustomLog = 25;
            public static readonly byte RequestMessage = 29;
        }

        public static class Outgoing
        {
            public static readonly byte HandshakeMessage = 0;
        }
    }
}
