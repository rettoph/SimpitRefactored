using SimpitRefactored.Common.Core;
using SimpitRefactored.Core.Peers;
using System;

namespace SimpitRefactored.Core.Extensions
{
    public static class SimpitExtensions
    {
        public static Simpit AddSerialPeer(this Simpit simpit, string name, int baudRate)
        {
            return simpit.AddPeer(new SerialPeer(name, baudRate));
        }

        public static Simpit AddTcpPeer(this Simpit simpit, int port)
        {
            return simpit.AddPeer(new TcpPeer(port));
        }

        public static Simpit AddIncomingConsumer<T>(this Simpit simpit, Action<SimpitPeer, ISimpitMessage<T>> consumer)
            where T : unmanaged, ISimpitMessageData
        {
            return simpit.AddIncomingSubscriber(new RuntimeMessageConsumer<T>(consumer));
        }

        private class RuntimeMessageConsumer<T> : ISimpitMessageSubscriber<T>
            where T : unmanaged, ISimpitMessageData
        {
            private readonly Action<SimpitPeer, ISimpitMessage<T>> _consumer;

            public RuntimeMessageConsumer(Action<SimpitPeer, ISimpitMessage<T>> consumer)
            {
                _consumer = consumer;
            }

            public void Process(SimpitPeer peer, ISimpitMessage<T> message)
            {
                _consumer(peer, message);
            }
        }
    }
}
