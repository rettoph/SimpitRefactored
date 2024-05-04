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
    public partial class Simpit : ISimpitMessageSubscriber<Synchronisation>, ISimpitMessageSubscriber<RegisterHandler>
    {
        private void RegisterCoreSubscriptions()
        {
            this.Subscribe<Synchronisation>(this)
                .Subscribe<RegisterHandler>(this);
        }

        void ISimpitMessageSubscriber<Synchronisation>.Process(SimpitPeer peer, ISimpitMessage<Synchronisation> message)
        {
            switch (message.Content.Type)
            {
                case Synchronisation.SynchronisationMessageTypeEnum.SYN:
                    _logger.LogVerbose("{0}::{1} - {2} recieved on peer {3}. Replying.", nameof(Simpit), nameof(ISimpitMessageSubscriber<Synchronisation>.Process), Synchronisation.SynchronisationMessageTypeEnum.SYN, peer);
                    peer.ProcessSYN(message.Content);
                    break;

                case Synchronisation.SynchronisationMessageTypeEnum.SYNACK:
                    throw new NotImplementedException();

                case Synchronisation.SynchronisationMessageTypeEnum.ACK:
                    _logger.LogVerbose("{0}::{1} - {2} recieved on peer {3}. Handshake complete, Resetting channels, Arduino library version '{4}'.", nameof(Simpit), nameof(ISimpitMessageSubscriber<Synchronisation>.Process), Synchronisation.SynchronisationMessageTypeEnum.ACK, peer, message.Content.Version);
                    peer.ProcessSYN(message.Content);
                    break;
            }
        }

        void ISimpitMessageSubscriber<RegisterHandler>.Process(SimpitPeer peer, ISimpitMessage<RegisterHandler> message)
        {
            peer.ProcessRegistration(message.Content);
        }
    }
}
