using KerbalSimpit.Common.Core;
using KerbalSimpit.Common.Core.Utilities;

namespace KerbalSimpit.Core.Messages
{
    public unsafe struct EchoRequest : ISimpitMessageData
    {
        public FixedBuffer Data;
    }
}
