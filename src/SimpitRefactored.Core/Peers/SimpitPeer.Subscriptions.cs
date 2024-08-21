using SimpitRefactored.Common.Core;
using SimpitRefactored.Common.Core.Utilities;
using SimpitRefactored.Core.Enums;
using SimpitRefactored.Core.Messages;

namespace SimpitRefactored.Core.Peers
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
                HandShakeType = (byte)Synchronisation.SynchronisationMessageTypeEnum.SYNACK
            });
        }

        internal void ProcessACK(Synchronisation message)
        {
            this.Status = ConnectionStatusEnum.CONNECTED;
        }

        internal unsafe void Process(RegisterHandler message)
        {
            for (int i = 0; i < Constants.MaximumMessageSize; i++)
            {
                byte messageTypeId = message.MessageTypeIds[i];

                if (messageTypeId == default)
                { // Ignore default requests
                    continue;
                }

                if (_simpit.Messages.TryGetOutgoingType(messageTypeId, out SimpitMessageType type) == false)
                {
                    this.logger.LogWarning("{0}::{1} - Unrecognized registration request from {2}, {3}", nameof(SimpitPeer), nameof(Process), this, messageTypeId);
                    continue;
                }

                try
                {
                    this.logger.LogVerbose("{0}::{1} - {2} subscribing to message type {3}", nameof(Simpit), nameof(Process), this, type);
                    _outgoingSubscriptions.TryAdd(type, default);
                    _simpit.GetOutgoingData(type).AddSubscriber(this);
                    this.OnOutgoingSubscribed?.Invoke(this, type);
                }
                catch
                {
                    this.logger.LogWarning("{0}::{1} - {2} already subscribed to message type {3}", nameof(Simpit), nameof(Process), this, type);
                }
            }
        }

        internal unsafe void Process(DeregisterHandler message)
        {
            for (int i = 0; i < Constants.MaximumMessageSize; i++)
            {
                byte messageTypeId = message.MessageTypeIds[i];

                if (messageTypeId == default)
                { // Ignore default requests
                    continue;
                }

                if (_simpit.Messages.TryGetOutgoingType(messageTypeId, out SimpitMessageType type) == false)
                {
                    this.logger.LogWarning("{0}::{1} - Unrecognized deregistration request from {2}, {3}", nameof(Simpit), nameof(ISimpitMessageSubscriber<RegisterHandler>.Process), this, messageTypeId);
                    continue;
                }

                try
                {
                    this.logger.LogVerbose("{0}::{1} - Peer {2} unsubscribing from message type {3}", nameof(Simpit), nameof(ISimpitMessageSubscriber<RegisterHandler>.Process), this, type);
                    _outgoingSubscriptions.TryRemove(type, out _);
                    _simpit.GetOutgoingData(type).RemoveSubscriber(this);
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
