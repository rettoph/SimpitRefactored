using SimpitRefactored.Common.Core;
using SimpitRefactored.Common.Core.Utilities;

namespace SimpitRefactored.Core.Messages
{
    public unsafe struct EchoRequest : ISimpitMessageData
    {
        public FixedBuffer Data;
    }
}
