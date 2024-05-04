using KerbalSimpit.Core.Constants;
using KerbalSimpit.Core.Messages;

namespace KerbalSimpit.Core
{
    public partial class Simpit
    {
        private void RegisterCoreMessages()
        {
            // Incoming Messages
            this.Messages.RegisterIncomingType<Request>(MessageTypeIds.Incoming.RequestMessage);
            this.Messages.RegisterIncomingType<Synchronisation>(MessageTypeIds.Incoming.Synchronisation, Synchronisation.Deserialize);
            this.Messages.RegisterIncomingType<CustomLog>(MessageTypeIds.Incoming.CustomLog, CustomLog.Deserialize);
            this.Messages.RegisterIncomingType<RegisterHandler>(MessageTypeIds.Incoming.RegisterHandler, RegisterHandler.Deserialize);

            // Outgoing Messages
            this.Messages.RegisterOutogingType<Handshake>(MessageTypeIds.Outgoing.HandshakeMessage);
        }
    }
}
