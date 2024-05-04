using System;

namespace KerbalSimpit.Core.Serialization
{
    public interface ISimpitMessageSerializer
    {
        byte MessageId { get; }

        Type MessageType { get; }

        void Serialize(SimpitStream stream, ISimpitMessage message);
    }
}
