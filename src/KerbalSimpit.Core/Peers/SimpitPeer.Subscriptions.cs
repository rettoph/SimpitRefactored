using KerbalSimpit.Core.Enums;
using KerbalSimpit.Core.Messages;
using KerbalSimpit.Core.Utilities;

namespace KerbalSimpit.Core.Peers
{
    public partial class SimpitPeer
    {
        internal void ProcessSYN(Synchronisation message)
        {
            // Remove all messages not yet sent to make sure the next message sent is an SYNACK
            _outgoing.Clear();
            _outgoing.Clear();

            this.Status = ConnectionStatusEnum.HANDSHAKE;
            this.EnqueueOutgoing(new Handshake()
            {
                Payload = 0x37, // TODO: Figure out what this is.
                HandShakeType = 0x01 // TODO: figure out what this is. KSP version?
            });
        }

        internal void ProcessACK(Synchronisation message)
        {
            this.Status = ConnectionStatusEnum.CONNECTED;
        }

        internal void Process(RegisterHandler message)
        {
            foreach (byte messageTypeId in message.MessageTypeIds)
            {
                if (_simpit.Messages.TryGetOutgoingType(messageTypeId, out SimpitMessageType type) == false)
                {
                    this.logger.LogWarning("{0}::{1} - Unrecognized registration request from {2}, {3}", nameof(SimpitPeer), nameof(Process), this, messageTypeId);
                    continue;
                }

                try
                {
                    this.logger.LogVerbose("{0}::{1} - {2} subscribing to message type {3}", nameof(Simpit), nameof(Process), this, type);
                    _outgoingSubscriptions.TryAdd(type, default);
                    this.OnOutgoingSubscribed?.Invoke(this, type);
                }
                catch
                {
                    this.logger.LogWarning("{0}::{1} - {2} already subscribed to message type {3}", nameof(Simpit), nameof(Process), this, type);
                }
            }
        }

        internal void Process(DeregisterHandler message)
        {
            foreach (byte messageTypeId in message.MessageTypeIds)
            {
                if (_simpit.Messages.TryGetOutgoingType(messageTypeId, out SimpitMessageType type) == false)
                {
                    this.logger.LogWarning("{0}::{1} - Unrecognized deregistration request from {2}, {3}", nameof(Simpit), nameof(ISimpitMessageSubscriber<RegisterHandler>.Process), this, messageTypeId);
                    continue;
                }

                try
                {
                    this.logger.LogVerbose("{0}::{1} - Peer {2} unsubscribing from message type {3}", nameof(Simpit), nameof(ISimpitMessageSubscriber<RegisterHandler>.Process), this, type);
                    _outgoingSubscriptions.TryRemove(type, out _);
                    this.OnOutgoingUnsubscribed?.Invoke(this, type);
                }
                catch
                {
                    this.logger.LogWarning("{0}::{1} - Peer {2} already unsubscribed from message type {3}", nameof(Simpit), nameof(ISimpitMessageSubscriber<RegisterHandler>.Process), this, type);
                }
            }
        }
    }
}
