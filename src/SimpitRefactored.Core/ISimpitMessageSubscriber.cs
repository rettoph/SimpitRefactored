using SimpitRefactored.Common.Core;
using SimpitRefactored.Core.Peers;

namespace SimpitRefactored.Core
{
    public interface ISimpitMessageSubscriber<T>
        where T : unmanaged, ISimpitMessageData
    {
        void Process(SimpitPeer peer, ISimpitMessage<T> message);
    }
}
