using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KerbalSimpit.Core.Messages
{
    public struct EchoRequest : ISimpitMessageContent
    {
        public byte[] Data { get; set; }

        internal static EchoRequest Deserialize(SimpitStream input)
        {
            return new EchoRequest()
            {
                Data = input.ReadAll(out int offset, out int count).Skip(offset).Take(count).ToArray()
            };
        }
    }
}
