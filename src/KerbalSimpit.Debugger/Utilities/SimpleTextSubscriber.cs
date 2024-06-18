using KerbalSimpit.Common.Core;
using KerbalSimpit.Core;
using KerbalSimpit.Core.Peers;
using KerbalSimpit.Debugger.Controls;

namespace KerbalSimpit.Debugger.Utilities
{
    internal sealed class SimpleTextSubscriber<T> : ISimpitMessageSubscriber<T>
        where T : unmanaged, ISimpitMessageData
    {
        private readonly Func<T, string> _text;
        private readonly SimpleTextSubscriberControl _control;
        private DateTime _updatedAt;

        public SimpleTextSubscriberControl Control => _control;

        public SimpleTextSubscriber(Func<T, string> text)
        {
            _text = text;
            _control = new SimpleTextSubscriberControl(typeof(T).Name);
            _control.SetValue(_text(default));
        }

        public void Process(SimpitPeer peer, ISimpitMessage<T> message)
        {
            _control.SetValue(_text(message.Data));
        }
    }
}
