using KerbalSimpit.Core.Constants;
using KerbalSimpit.Core.Enums;
using KerbalSimpit.Core.Messages;
using KerbalSimpit.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KerbalSimpit.Core.Peers
{
    public partial class SimpitPeer
    {
        internal void ProcessSYN(SynchronisationMessage message)
        {
            // Remove all messages not yet sent to make sure the next message sent is an SYNACK
            _outbound.Clear();
            _outbound.Clear();

            this.Status = ConnectionStatusEnum.HANDSHAKE;
            this.EnqueueOutbound(new HandshakeMessage()
            {
                Payload = 0x37, // TODO: Figure out what this is.
                HandShakeType = 0x01 // TODO: figure out what this is. KSP version?
            });
		}

        internal void ProcessACK(SynchronisationMessage message)
        {
            this.Status = ConnectionStatusEnum.CONNECTED;
        }

        internal void ProcessRegistration(RegisterHandlerMessage message)
        {
            foreach(byte messageIdValue in message.MessageIds)
            {
                if(SimpitMessageId.TryGetByValue(messageIdValue, SimputMessageIdDirectionEnum.Outbound, out SimpitMessageId messageId) == false)
                {
                    this.logger.LogWarning("Peer {0} requesting subscription to unknown message {1}", this, messageIdValue);
                    continue;
                }

                if(_subscribedOutgoingMessageIds.Add(messageId))
                {
                    this.logger.LogVerbose("Peer {0} subscribing to message {1} ({2})", this, messageId.Value, messageId.Type.Name);
                }
                else
                {
                    this.logger.LogWarning("Peer {0} trying to subscribe to channel {1} but is already subscribed. Ignoring it", this, messageIdValue);
                }
            }
        }
    }
}
