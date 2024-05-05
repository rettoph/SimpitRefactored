using KerbalSimpit.Core.Constants;
using KerbalSimpit.Core.Messages;

namespace KerbalSimpit.Core
{
    public partial class Simpit
    {
        private void RegisterCoreMessages()
        {
            // Incoming Messages
            this.Messages.RegisterIncomingType<Synchronisation>(MessageTypeIds.Incoming.Synchronisation, Synchronisation.Deserialize);
            this.Messages.RegisterIncomingType<EchoRequest>(MessageTypeIds.Incoming.EchoRequest, EchoRequest.Deserialize);
            this.Messages.RegisterIncomingType<EchoResponse>(MessageTypeIds.Incoming.EchoResponse, EchoResponse.Deserialize);
            this.Messages.RegisterIncomingType<CloseSerialPort>(MessageTypeIds.Incoming.CloseSerialPort);
            this.Messages.RegisterIncomingType<RegisterHandler>(MessageTypeIds.Incoming.RegisterHandler, RegisterHandler.Deserialize);
            this.Messages.RegisterIncomingType<DeregisterHandler>(MessageTypeIds.Incoming.DeregisterHandler, DeregisterHandler.Deserialize);
            this.Messages.RegisterIncomingType<CustomLog>(MessageTypeIds.Incoming.CustomLog, CustomLog.Deserialize);
            this.Messages.RegisterIncomingType<Request>(MessageTypeIds.Incoming.RequestMessage);

            // Outgoing Messages
            this.Messages.RegisterOutogingType<Handshake>(MessageTypeIds.Outgoing.HandshakeMessage);
            this.Messages.RegisterOutogingType<EchoResponse>(MessageTypeIds.Outgoing.EchoResponse, EchoResponse.Serialize);
        }
    }
}
