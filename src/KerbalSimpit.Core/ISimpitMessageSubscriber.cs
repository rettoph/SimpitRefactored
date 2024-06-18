using KerbalSimpit.Common.Core;
using KerbalSimpit.Core.Peers;

namespace KerbalSimpit.Core
{
    public interface ISimpitMessageSubscriber<T>
        where T : unmanaged, ISimpitMessageData
    {
        void Process(SimpitPeer peer, ISimpitMessage<T> message);
    }
}
