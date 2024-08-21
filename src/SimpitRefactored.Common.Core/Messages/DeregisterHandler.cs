using SimpitRefactored.Common.Core;

namespace SimpitRefactored.Core.Messages
{
    public unsafe struct DeregisterHandler : ISimpitMessageData
    {
        public fixed byte MessageTypeIds[Constants.MaximumMessageSize];
    }
}
