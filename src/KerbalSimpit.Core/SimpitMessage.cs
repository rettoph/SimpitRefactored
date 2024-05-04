namespace KerbalSimpit.Core
{
    internal class SimpitMessage<T> : ISimpitMessage<T>
        where T : ISimpitMessageContent
    {
        public readonly SimpitMessageType<T> Type;
        public readonly T Content;

        public SimpitMessage(SimpitMessageType<T> type, T content)
        {
            this.Type = type;
            this.Content = content;
        }

        SimpitMessageType<T> ISimpitMessage<T>.Type => this.Type;

        SimpitMessageType ISimpitMessage.Type => this.Type;

        T ISimpitMessage<T>.Content => this.Content;

        ISimpitMessageContent ISimpitMessage.Content => this.Content;
    }
}
