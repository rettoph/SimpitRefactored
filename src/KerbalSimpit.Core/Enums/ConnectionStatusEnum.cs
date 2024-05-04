using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KerbalSimpit.Core.Enums
{
    public enum ConnectionStatusEnum
    {
        CLOSED, // The port is closed, SimPit does not use it.
        WAITING_HANDSHAKE, // The port is opened, waiting for the controller to start the handshake
        HANDSHAKE, // The port is opened, the first handshake packet was received, waiting for the SYN/ACK
        CONNECTED, // The connection is established and a message was received from the controller in the last IDLE_TIMEOUT seconds
        IDLE, // The connection is established and no message was received from the controller in the last IDLE_TIMEOUT seconds. This can indicate a failure on the controller side or a controller that only read data.
        ERROR, // The port could not be openned.
    }
}
