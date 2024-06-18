using KerbalSimpit.Common.Core;

namespace KerbalSimpit.Core.Messages
{
    public struct Handshake : ISimpitMessageData
    {
        public byte HandShakeType { get; set; }
        public byte Payload { get; set; }
    }
}
