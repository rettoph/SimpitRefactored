using KerbalSimpit.Core.Messages;
using KerbalSimpit.Core.Peers;
using KerbalSimpit.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KerbalSimpit.Core
{
    public partial class Simpit : ISimpitMessageConsumer<Synchronisation>, ISimpitMessageConsumer<RegisterHandler>, ISimpitMessageConsumer<DeregisterHandler>
    {
        private void RegisterCoreSubscriptions()
        {
            this.RegisterIncomingConsumer<Synchronisation>(this)
                .RegisterIncomingConsumer<RegisterHandler>(this)
                .RegisterIncomingConsumer<DeregisterHandler>(this);
        }

        void ISimpitMessageConsumer<Synchronisation>.Consume(SimpitPeer peer, ISimpitMessage<Synchronisation> message)
        {
            switch (message.Content.Type)
            {
                case Synchronisation.SynchronisationMessageTypeEnum.SYN:
                    _logger.LogVerbose("{0}::{1} - {2} recieved on peer {3}. Replying.", nameof(Simpit), nameof(ISimpitMessageConsumer<Synchronisation>.Consume), Synchronisation.SynchronisationMessageTypeEnum.SYN, peer);
                    peer.ProcessSYN(message.Content);
                    break;

                case Synchronisation.SynchronisationMessageTypeEnum.SYNACK:
                    throw new NotImplementedException();

                case Synchronisation.SynchronisationMessageTypeEnum.ACK:
                    _logger.LogVerbose("{0}::{1} - {2} recieved on peer {3}. Handshake complete, Resetting channels, Arduino library version '{4}'.", nameof(Simpit), nameof(ISimpitMessageConsumer<Synchronisation>.Consume), Synchronisation.SynchronisationMessageTypeEnum.ACK, peer, message.Content.Version);
                    peer.ProcessSYN(message.Content);
                    break;
            }
        }

        void ISimpitMessageConsumer<RegisterHandler>.Consume(SimpitPeer peer, ISimpitMessage<RegisterHandler> message)
        {
            foreach(byte messageTypeId in message.Content.MessageTypeIds)
            {
                if(this.Messages.TryGetOutgoingType(messageTypeId, out SimpitMessageType type) == false)
                {
                    _logger.LogWarning("{0}::{1} - Unrecognized registration request from {2}, {3}", nameof(Simpit), nameof(ISimpitMessageConsumer<RegisterHandler>.Consume), peer, messageTypeId);
                    continue;
                }

                try
                {
                    _logger.LogVerbose("{0}::{1} - Peer {2} subscribing to message type {3}", nameof(Simpit), nameof(ISimpitMessageConsumer<RegisterHandler>.Consume), peer, type);
                    _outogingMessageSubscribers.Add(type, peer);
                    this.OnPeerSubscribe?.Invoke(this, (type, peer));
                }
                catch
                {
                    _logger.LogWarning("{0}::{1} - Peer {2} already subscribed to message type {3}", nameof(Simpit), nameof(ISimpitMessageConsumer<RegisterHandler>.Consume), peer, type);
                }
            }
        }

        void ISimpitMessageConsumer<DeregisterHandler>.Consume(SimpitPeer peer, ISimpitMessage<DeregisterHandler> message)
        {
            foreach (byte messageTypeId in message.Content.MessageTypeIds)
            {
                if (this.Messages.TryGetOutgoingType(messageTypeId, out SimpitMessageType type) == false)
                {
                    _logger.LogWarning("{0}::{1} - Unrecognized deregistration request from {2}, {3}", nameof(Simpit), nameof(ISimpitMessageConsumer<RegisterHandler>.Consume), peer, messageTypeId);
                    continue;
                }

                try
                {
                    _logger.LogVerbose("{0}::{1} - Peer {2} unsubscribing from message type {3}", nameof(Simpit), nameof(ISimpitMessageConsumer<RegisterHandler>.Consume), peer, type);
                    _outogingMessageSubscribers.Remove(type, peer);
                    this.OnPeerUnsubscribe?.Invoke(this, (type, peer));
                }
                catch
                {
                    _logger.LogWarning("{0}::{1} - Peer {2} already unsubscribed from message type {3}", nameof(Simpit), nameof(ISimpitMessageConsumer<RegisterHandler>.Consume), peer, type);
                }
            }
        }
    }
}
