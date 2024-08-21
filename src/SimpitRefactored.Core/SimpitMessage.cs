using SimpitRefactored.Common.Core;

namespace SimpitRefactored.Core
{
    internal class SimpitMessage<T> : ISimpitMessage<T>
        where T : unmanaged, ISimpitMessageData
    {
        public readonly SimpitMessageType<T> Type;
        public readonly T Data;

        public SimpitMessage(SimpitMessageType<T> type, T content)
        {
            this.Type = type;
            this.Data = content;
        }

        SimpitMessageType<T> ISimpitMessage<T>.Type => this.Type;

        SimpitMessageType ISimpitMessage.Type => this.Type;

        T ISimpitMessage<T>.Data => this.Data;

        ISimpitMessageData ISimpitMessage.Data => this.Data;
    }
}
