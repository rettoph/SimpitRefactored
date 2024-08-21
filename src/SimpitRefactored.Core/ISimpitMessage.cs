using SimpitRefactored.Common.Core;

namespace SimpitRefactored.Core
{
    public interface ISimpitMessage
    {
        SimpitMessageType Type { get; }
        ISimpitMessageData Data { get; }
    }

    public interface ISimpitMessage<TContent> : ISimpitMessage
        where TContent : unmanaged, ISimpitMessageData
    {
        new SimpitMessageType<TContent> Type { get; }
        new TContent Data { get; }
    }
}
