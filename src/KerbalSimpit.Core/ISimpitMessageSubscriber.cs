using KerbalSimpit.Core.Peers;

namespace KerbalSimpit.Core
{
    public interface ISimpitMessageSubscriber<T>
        where T : ISimpitMessageData
    {
        void Process(SimpitPeer peer, ISimpitMessage<T> message);
    }
}
