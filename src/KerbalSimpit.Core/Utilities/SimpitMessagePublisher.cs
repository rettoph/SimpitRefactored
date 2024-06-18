using KerbalSimpit.Common.Core;
using KerbalSimpit.Common.Core.Utilities;
using KerbalSimpit.Core.Peers;
using System;
using System.Collections.Generic;

namespace KerbalSimpit.Core.Utilities
{
    internal abstract class SimpitMessagePublisher
    {
        public abstract Type Type { get; }

        public abstract void Publish(SimpitPeer peer, ISimpitMessage message);

        public static SimpitMessagePublisher Create(Type messageType, Simpit simpit)
        {
            Type type = typeof(SimpitMessagePublisher<>).MakeGenericType(messageType);

            return Activator.CreateInstance(type, new object[] { simpit }) as SimpitMessagePublisher;
        }
    }

    internal sealed class SimpitMessagePublisher<T> : SimpitMessagePublisher
        where T : unmanaged, ISimpitMessageData
    {
        private readonly Simpit _simpit;
        private readonly HashSet<ISimpitMessageSubscriber<T>> _subscribers;

        public override Type Type => typeof(T);

        public SimpitMessagePublisher(Simpit simpit)
        {
            _simpit = simpit;
            _subscribers = new HashSet<ISimpitMessageSubscriber<T>>();
        }

        public override void Publish(SimpitPeer peer, ISimpitMessage message)
        {
            if (message is ISimpitMessage<T> casted)
            {
                if (_subscribers.Count > 0)
                {
                    foreach (ISimpitMessageSubscriber<T> subscriber in _subscribers)
                    {
                        subscriber.Process(peer, casted);
                    }

                    return;
                }

                _simpit.Logger.LogWarning("{0}::{1} - Incoming message {2} has no subscribers", nameof(SimpitMessagePublisher), nameof(Publish), message.Type);
            }
        }

        public void AddSubscriber(ISimpitMessageSubscriber<T> subscriber)
        {
            _subscribers.Add(subscriber);
        }

        public void RemoveSubscriber(ISimpitMessageSubscriber<T> subscriber)
        {
            _subscribers.Remove(subscriber);
        }
    }
}
