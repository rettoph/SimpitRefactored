using KerbalSimpit.Common.Core.Utilities;
using KerbalSimpit.Core.Messages;
using KerbalSimpit.Core.Peers;
using System;

namespace KerbalSimpit.Core
{
    public partial class Simpit :
        ISimpitMessageSubscriber<Synchronisation>,
        ISimpitMessageSubscriber<RegisterHandler>,
        ISimpitMessageSubscriber<DeregisterHandler>,
        ISimpitMessageSubscriber<Request>
    {
        private void AddCoreSubscriptions()
        {
            this.AddIncomingSubscriber<Synchronisation>(this)
                .AddIncomingSubscriber<RegisterHandler>(this)
                .AddIncomingSubscriber<DeregisterHandler>(this)
                .AddIncomingSubscriber<Request>(this);
        }

        void ISimpitMessageSubscriber<Synchronisation>.Process(SimpitPeer peer, ISimpitMessage<Synchronisation> message)
        {
            switch (message.Data.Type)
            {
                case Synchronisation.SynchronisationMessageTypeEnum.SYN:
                    _logger.LogDebug("{0}::{1} - {2} recieved on peer {3}. Replying.", nameof(Simpit), nameof(ISimpitMessageSubscriber<Synchronisation>.Process), Synchronisation.SynchronisationMessageTypeEnum.SYN, peer);
                    peer.ProcessSYN(message.Data);
                    break;

                case Synchronisation.SynchronisationMessageTypeEnum.SYNACK:
                    throw new NotImplementedException();

                case Synchronisation.SynchronisationMessageTypeEnum.ACK:
                    _logger.LogDebug("{0}::{1} - {2} recieved on peer {3}. Handshake complete, Resetting channels, Arduino library version '{4}'.", nameof(Simpit), nameof(ISimpitMessageSubscriber<Synchronisation>.Process), Synchronisation.SynchronisationMessageTypeEnum.ACK, peer, message.Data.Version);
                    peer.ProcessACK(message.Data);
                    break;
            }
        }

        void ISimpitMessageSubscriber<RegisterHandler>.Process(SimpitPeer peer, ISimpitMessage<RegisterHandler> message)
        {
            peer.Process(message.Data);
        }

        void ISimpitMessageSubscriber<DeregisterHandler>.Process(SimpitPeer peer, ISimpitMessage<DeregisterHandler> message)
        {
            peer.Process(message.Data);
        }

        void ISimpitMessageSubscriber<Request>.Process(SimpitPeer peer, ISimpitMessage<Request> message)
        {
            if (message.Data.MessageTypeId == 0)
            { // Magic number. Request all active subscriptions.
                peer.ForceEnqueueOutgoingSubscriptions();
                return;
            }

            if (this.Messages.TryGetOutgoingType(message.Data.MessageTypeId, out SimpitMessageType type) == false)
            {
                _logger.LogWarning("{0}::{1} - Unknown message type {2} request recieved on peer {3}.", nameof(Simpit), nameof(ISimpitMessageSubscriber<Synchronisation>.Process), message.Data.MessageTypeId, peer);
                return;
            }

            _logger.LogDebug("{0}::{1} - message type {2} request recieved on peer {3}.", nameof(Simpit), nameof(ISimpitMessageSubscriber<Synchronisation>.Process), type, peer);
            type.TryEnqueueOutgoingData(peer, this);
        }
    }
}
