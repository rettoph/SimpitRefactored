namespace KerbalSimpit.Core
{
    public interface ISimpitMessage
    {
        SimpitMessageType Type { get; }
        ISimpitMessageData Data { get; }
    }

    public interface ISimpitMessage<TContent> : ISimpitMessage
        where TContent : ISimpitMessageData
    {
        new SimpitMessageType<TContent> Type { get; }
        new TContent Data { get; }
    }
}
