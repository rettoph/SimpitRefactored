using SimpitRefactored.Common.Core;

namespace SimpitRefactored.Core.Messages
{
    public unsafe struct RegisterHandler : ISimpitMessageData
    {
        public fixed byte MessageTypeIds[Constants.MaximumMessageSize];
    }
}
