using KerbalSimpit.Core.Messages;
using KerbalSimpit.Core.Peers;
using KerbalSimpit.Core.Utilities;
using System;

namespace KerbalSimpit.Core
{
    public partial class Simpit : ISimpitMessageSubscriber<Synchronisation>, ISimpitMessageSubscriber<RegisterHandler>, ISimpitMessageSubscriber<DeregisterHandler>
    {
        private void AddCoreSubscriptions()
        {
            this.AddIncomingSubscriber<Synchronisation>(this)
                .AddIncomingSubscriber<RegisterHandler>(this)
                .AddIncomingSubscriber<DeregisterHandler>(this);
        }

        void ISimpitMessageSubscriber<Synchronisation>.Process(SimpitPeer peer, ISimpitMessage<Synchronisation> message)
        {
            switch (message.Data.Type)
            {
                case Synchronisation.SynchronisationMessageTypeEnum.SYN:
                    _logger.LogVerbose("{0}::{1} - {2} recieved on peer {3}. Replying.", nameof(Simpit), nameof(ISimpitMessageSubscriber<Synchronisation>.Process), Synchronisation.SynchronisationMessageTypeEnum.SYN, peer);
                    peer.ProcessSYN(message.Data);
                    break;

                case Synchronisation.SynchronisationMessageTypeEnum.SYNACK:
                    throw new NotImplementedException();

                case Synchronisation.SynchronisationMessageTypeEnum.ACK:
                    _logger.LogVerbose("{0}::{1} - {2} recieved on peer {3}. Handshake complete, Resetting channels, Arduino library version '{4}'.", nameof(Simpit), nameof(ISimpitMessageSubscriber<Synchronisation>.Process), Synchronisation.SynchronisationMessageTypeEnum.ACK, peer, message.Data.Version);
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
    }
}
