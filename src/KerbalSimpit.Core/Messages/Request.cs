using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KerbalSimpit.Core.Messages
{
    public struct Request : ISimpitMessageContent
    {
        public readonly byte ChannelId;
    }
}
