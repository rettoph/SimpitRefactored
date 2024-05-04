using KerbalSimpit.Core.Peers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KerbalSimpit.Core
{
    public interface ISimpitMessageConsumer<T>
        where T : ISimpitMessageContent
    {
        void Consume(SimpitPeer peer, ISimpitMessage<T> message);
    }
}
