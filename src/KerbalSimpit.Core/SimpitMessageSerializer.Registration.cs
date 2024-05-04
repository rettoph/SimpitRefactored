using KerbalSimpit.Core.Constants;
using KerbalSimpit.Core.Enums;
using KerbalSimpit.Core.Messages;
using KerbalSimpit.Core.Serialization.Deserializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KerbalSimpit.Core
{
    public partial class SimpitMessageSerializer
    {
        private static void RegisterCoreMessages()
        {
            // Inbound
            SimpitMessageSerializer.RegisterDeserializer<RequestMessage>(MessageIds.Inbound.RequestMessage);
            SimpitMessageSerializer.RegisterDeserializer(new SynchronisationMessageDeserializer());
            SimpitMessageSerializer.RegisterDeserializer(new CustomLogMessageDeserializer());
            SimpitMessageSerializer.RegisterDeserializer(new RegisterHandlerMessageDeserializer());

            // Outbound
            SimpitMessageSerializer.RegisterSerializer<HandshakeMessage>(MessageIds.Outbound.HandshakeMessage);
		}
    }
}
