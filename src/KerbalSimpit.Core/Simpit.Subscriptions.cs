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
    public partial class Simpit : ISimpitMessageSubscriber<SynchronisationMessage>, ISimpitMessageSubscriber<RegisterHandlerMessage>
    {
        private void RegisterCoreSubscriptions()
        {
            this.Subscribe<SynchronisationMessage>(this).Subscribe<RegisterHandlerMessage>(this);
        }

        void ISimpitMessageSubscriber<SynchronisationMessage>.Process(SimpitPeer peer, SynchronisationMessage message)
        {
            switch (message.Type)
            {
                case SynchronisationMessage.SynchronisationMessageTypeEnum.SYN:
                    _logger.LogVerbose("{0}::{1} - {2} recieved on peer {3}. Replying.", nameof(Simpit), nameof(ISimpitMessageSubscriber<SynchronisationMessage>.Process), SynchronisationMessage.SynchronisationMessageTypeEnum.SYN, peer);
                    peer.ProcessSYN(message);
                    break;

                case SynchronisationMessage.SynchronisationMessageTypeEnum.SYNACK:
                    throw new NotImplementedException();

                case SynchronisationMessage.SynchronisationMessageTypeEnum.ACK:
                    _logger.LogVerbose("{0}::{1} - {2} recieved on peer {3}. Handshake complete, Resetting channels, Arduino library version '{4}'.", nameof(Simpit), nameof(ISimpitMessageSubscriber<SynchronisationMessage>.Process), SynchronisationMessage.SynchronisationMessageTypeEnum.ACK, peer, message.Version);
                    peer.ProcessSYN(message);
                    break;
            }
        }

        public void Process(SimpitPeer peer, RegisterHandlerMessage message)
        {
            peer.ProcessRegistration(message);
        }
    }
}
