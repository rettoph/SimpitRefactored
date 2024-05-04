using KerbalSimpit.Core.Peers;
using KerbalSimpit.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KerbalSimpit.Core
{
    public partial class Simpit : IDisposable
    {
        private readonly ISimpitLogger _logger;
        private readonly List<SimpitPeer> _peers;
        private readonly Dictionary<Type, SimpitMessagePublisher> _publishers;

        private CancellationToken _cancellationToken;

        public readonly ReadOnlyCollection<SimpitPeer> Peers;

        public bool Running { get; private set; }
        public ISimpitLogger Logger => _logger;

        public Simpit(ISimpitLogger logger)
        {
            _logger = logger;
            _peers = new List<SimpitPeer>();
            _publishers = new Dictionary<Type, SimpitMessagePublisher>();

            this.Peers = new ReadOnlyCollection<SimpitPeer>(_peers);

            this.RegisterCoreSubscriptions();
        }

        public void Dispose()
        {
            if(this.Running)
            {
                this.Stop();
            }
        }

        public Simpit Add(SimpitPeer peer)
        {
            _peers.Add(peer);

            if(this.Running && peer.Running == false)
            {
                peer.Start(_cancellationToken);
            }

            return this;
        }

        public Simpit Start(CancellationToken cancellationToken)
        {
            _cancellationToken = cancellationToken;

            foreach(SimpitPeer peer in _peers)
            {
                if(peer.Running == false)
                {
                    peer.Start(_cancellationToken);
                }
            }

            this.Running = true;

            return this;
        }

        public void Stop()
        {
            foreach (SimpitPeer peer in _peers)
            {
                if (peer.Running == false)
                {
                    peer.Stop();
                }
            }

            this.Running = true;
        }

        public void Flush()
        {
            foreach(SimpitPeer peer in _peers)
            {
                while(peer.TryRead(out ISimpitMessage message))
                {
                    this.GetPublisher(message.GetType()).Publish(peer, message);
                }
            }
        }

        public Simpit Subscribe<T>(ISimpitMessageSubscriber<T> subscriber)
            where T : ISimpitMessage
        {
            SimpitMessagePublisher<T> publisher = this.GetPublisher(typeof(T)) as SimpitMessagePublisher<T>;
            publisher.Subscribe(subscriber);

            return this;
        }

        public void Unsubscribe<T>(ISimpitMessageSubscriber<T> subscriber)
            where T : ISimpitMessage
        {
            SimpitMessagePublisher<T> publisher = this.GetPublisher(typeof(T)) as SimpitMessagePublisher<T>;
            publisher.Unsubscribe(subscriber);
        }

        private SimpitMessagePublisher GetPublisher(Type messageType)
        {
            if(_publishers.TryGetValue(messageType, out SimpitMessagePublisher publisher))
            {
                return publisher;
            }

            publisher = SimpitMessagePublisher.Create(messageType);
            _publishers.Add(messageType, publisher);

            return publisher;
        }
    }
}
