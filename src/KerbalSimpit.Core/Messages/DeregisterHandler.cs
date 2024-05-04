using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KerbalSimpit.Core.Messages
{
    public struct DeregisterHandler : ISimpitMessageContent
    {
        public byte[] MessageTypeIds { get; set; }

        internal static DeregisterHandler Deserialize(SimpitStream input)
        {
            return new DeregisterHandler()
            {
                MessageTypeIds = input.ReadAll(out int offset, out int count).Skip(offset).Take(count).ToArray()
            };
        }
    }
}
