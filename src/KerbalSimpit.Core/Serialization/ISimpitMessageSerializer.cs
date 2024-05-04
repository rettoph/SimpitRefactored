using System;

namespace KerbalSimpit.Core.Serialization
{
    public interface ISimpitMessageSerializer
    {
        SimpitMessageId MessageId { get; }

        void Serialize(SimpitStream stream, ISimpitMessage message);
    }
}
