using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KerbalSimpit.Core.Messages
{
    public struct Handshake : ISimpitMessageContent
    {
        public byte HandShakeType { get; set; }
        public byte Payload { get; set; }
    }
}
