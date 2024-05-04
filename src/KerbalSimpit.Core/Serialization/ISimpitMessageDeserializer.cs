using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KerbalSimpit.Core.Serialization
{
    public interface ISimpitMessageDeserializer
    {
        byte MessageId { get; }

        Type MessageType { get; }

        bool TryDeserialize(SimpitStream stream, out ISimpitMessage message);
    }
}
