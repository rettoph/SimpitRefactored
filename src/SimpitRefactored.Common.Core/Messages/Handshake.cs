using SimpitRefactored.Common.Core;

namespace SimpitRefactored.Core.Messages
{
    public struct Handshake : ISimpitMessageData
    {
        public byte HandShakeType { get; set; }
        public byte Payload { get; set; }
    }
}
