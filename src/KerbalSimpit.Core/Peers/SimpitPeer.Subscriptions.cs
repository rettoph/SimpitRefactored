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
        internal void ProcessSYN(Synchronisation message)
        {
            // Remove all messages not yet sent to make sure the next message sent is an SYNACK
            _outbound.Clear();
            _outbound.Clear();

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
    }
}
