using KerbalSimpit.Core.Peers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KerbalSimpit.Core
{
    public interface ISimpitMessageSubscriber<T>
        where T : ISimpitMessageData
    {
        void Process(SimpitPeer peer, ISimpitMessage<T> message);
    }
}
