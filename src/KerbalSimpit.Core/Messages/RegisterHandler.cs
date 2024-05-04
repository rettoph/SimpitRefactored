using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KerbalSimpit.Core.Messages
{
    public struct RegisterHandler : ISimpitMessageContent
    {
        public byte[] MessageIds { get; set; }

        internal static RegisterHandler Deserialize(SimpitStream input)
        {
            return new RegisterHandler()
            {
                MessageIds = input.ReadAll(out int offset, out int count).Skip(offset).Take(count).ToArray()
            };
        }
    }
}
