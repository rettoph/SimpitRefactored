using KerbalSimpit.Core.Peers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KerbalSimpit.Core.Utilities
{
    internal abstract class SimpitMessagePublisher
    {
        public abstract Type Type { get; }

        public abstract void Publish(SimpitPeer peer,ISimpitMessage message);

        public static SimpitMessagePublisher Create(Type messageType)
        {
            Type type = typeof(SimpitMessagePublisher<>).MakeGenericType(messageType);

            return Activator.CreateInstance(type) as SimpitMessagePublisher;
        }
    }

    internal sealed class SimpitMessagePublisher<T> : SimpitMessagePublisher
        where T : ISimpitMessage
    {
        private HashSet<ISimpitMessageSubscriber<T>> _subscribers;

        public override Type Type => typeof(T);

        public SimpitMessagePublisher()
        {
            _subscribers = new HashSet<ISimpitMessageSubscriber<T>>();
        }

        public override void Publish(SimpitPeer peer, ISimpitMessage message)
        {
            if(message is T casted)
            {
                foreach(ISimpitMessageSubscriber<T> subscriber in _subscribers)
                {
                    subscriber.Process(peer, casted);
                }
            }
        }

        public void Subscribe(ISimpitMessageSubscriber<T> subscriber)
        {
            _subscribers.Add(subscriber);
        }

        public void Unsubscribe(ISimpitMessageSubscriber<T> subscriber)
        {
            _subscribers.Remove(subscriber);
        }
    }
}
