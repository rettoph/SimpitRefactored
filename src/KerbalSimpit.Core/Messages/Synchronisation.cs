using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KerbalSimpit.Core.Messages
{
    public struct Synchronisation : ISimpitMessageContent
    {
        public enum SynchronisationMessageTypeEnum : byte
        {
            SYN = 0x0,
            SYNACK = 0x1,
            ACK = 0x2
        }

        public SynchronisationMessageTypeEnum Type { get; set; }
        public string Version { get; set; }

        internal static Synchronisation Deserialize(SimpitStream input)
        {
            return new Synchronisation()
            {
                Type = (Synchronisation.SynchronisationMessageTypeEnum)input.ReadByte(),
                Version = input.ReadString(input.Length - 1)
            };
        }
    }
}
