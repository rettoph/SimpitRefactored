using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KerbalSimpit.Core.Messages
{
    public struct SynchronisationMessage : ISimpitMessage
    {
        public enum SynchronisationMessageTypeEnum : byte
        {
            SYN = 0x0,
            SYNACK = 0x1,
            ACK = 0x2
        }

        public SynchronisationMessageTypeEnum Type { get; set; }
        public string Version { get; set; }
    }
}
