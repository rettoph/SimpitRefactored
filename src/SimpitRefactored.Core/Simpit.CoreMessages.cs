using SimpitRefactored.Common.Core;
using SimpitRefactored.Core.Messages;

namespace SimpitRefactored.Core
{
    public partial class Simpit
    {
        private void RegisterCoreMessages()
        {
            // Incoming Messages
            this.Messages.RegisterIncomingType<Synchronisation>(Constants.MessageTypeIds.Incoming.Synchronisation);
            this.Messages.RegisterIncomingType<EchoRequest>(Constants.MessageTypeIds.Incoming.EchoRequest);
            this.Messages.RegisterIncomingType<EchoResponse>(Constants.MessageTypeIds.Incoming.EchoResponse);
            this.Messages.RegisterIncomingType<CloseSerialPort>(Constants.MessageTypeIds.Incoming.CloseSerialPort);
            this.Messages.RegisterIncomingType<RegisterHandler>(Constants.MessageTypeIds.Incoming.RegisterHandler);
            this.Messages.RegisterIncomingType<DeregisterHandler>(Constants.MessageTypeIds.Incoming.DeregisterHandler);
            this.Messages.RegisterIncomingType<CustomLog>(Constants.MessageTypeIds.Incoming.CustomLog);
            this.Messages.RegisterIncomingType<Request>(Constants.MessageTypeIds.Incoming.RequestMessage);

            // Outgoing Messages
            this.Messages.RegisterOutogingType<Handshake>(Constants.MessageTypeIds.Outgoing.HandshakeMessage);
            this.Messages.RegisterOutogingType<EchoResponse>(Constants.MessageTypeIds.Outgoing.EchoResponse);
        }
    }
}
