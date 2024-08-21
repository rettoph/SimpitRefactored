using SimpitRefactored.Common.Core;

namespace SimpitRefactored.Core.Messages
{
    public struct Request : ISimpitMessageData
    {
        public readonly byte MessageTypeId;
    }
}
