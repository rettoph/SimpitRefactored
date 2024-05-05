using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KerbalSimpit.Core.Messages
{
    public struct EchoResponse : ISimpitMessageContent
    {
        public byte[] Data { get; set; }

        internal static EchoResponse Deserialize(SimpitStream input)
        {
            return new EchoResponse()
            {
                Data = input.ReadAll(out int offset, out int count).Skip(offset).Take(count).ToArray()
            };
        }

        internal static void Serialize(EchoResponse input, SimpitStream output)
        {
            output.Write(input.Data);
        }
    }
}
