using KerbalSimpit.Core.Enums;
using KerbalSimpit.Core.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KerbalSimpit.Core.Constants
{
    public static class MessageIds
    {
        public static class Inbound
        {
            public static readonly SimpitMessageId<SynchronisationMessage> Synchronisation = SimpitMessageId.GetOrCreate<SynchronisationMessage>(0, SimputMessageIdDirectionEnum.Inbound);
            public static readonly SimpitMessageId<RegisterHandlerMessage> RegisterHandler = SimpitMessageId.GetOrCreate<RegisterHandlerMessage>(8, SimputMessageIdDirectionEnum.Inbound);
            public static readonly SimpitMessageId<CustomLogMessage> CustomLog = SimpitMessageId.GetOrCreate<CustomLogMessage>(25, SimputMessageIdDirectionEnum.Inbound);
            public static readonly SimpitMessageId<RequestMessage> RequestMessage = SimpitMessageId.GetOrCreate<RequestMessage>(29, SimputMessageIdDirectionEnum.Inbound);
        }

        public static class Outbound
        {
            public static readonly SimpitMessageId<HandshakeMessage> HandshakeMessage = SimpitMessageId.GetOrCreate<HandshakeMessage>(0, SimputMessageIdDirectionEnum.Outbound);
        }
    }
}
