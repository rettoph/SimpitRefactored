using KerbalSimpit.Core.Peers;
using KerbalSimpit.Core.Services;
using KerbalSimpit.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;

namespace KerbalSimpit.Core
{
    public partial class Simpit : IDisposable
    {
        public static Simpit Instance { get; private set; }

        private readonly ISimpitLogger _logger;
        private readonly List<SimpitPeer> _peers;
        private readonly Dictionary<Type, SimpitMessagePublisher> _publishers;
        private readonly ManyToMany<SimpitMessageType, SimpitPeer> _outogingMessageSubscribers;

        private CancellationTokenSource _cancellationTokenSource;

        public readonly ReadOnlyCollection<SimpitPeer> Peers;
        public readonly SimpitMessageService Messages;

        public CancellationToken CancellationToken => _cancellationTokenSource.Token;

        public bool Running { get; private set; }
        public ISimpitLogger Logger => _logger;

        public event EventHandler<(SimpitMessageType, SimpitPeer)> OnPeerSubscribe;
        public event EventHandler<(SimpitMessageType, SimpitPeer)> OnPeerUnsubscribe;

        public Simpit(ISimpitLogger logger)
        {
            _logger = logger;
            _peers = new List<SimpitPeer>();
            _publishers = new Dictionary<Type, SimpitMessagePublisher>();
            _outogingMessageSubscribers = new ManyToMany<SimpitMessageType, SimpitPeer>();

            this.Messages = new SimpitMessageService(this);
            this.Peers = new ReadOnlyCollection<SimpitPeer>(_peers);

            this.RegisterCoreMessages();
            this.AddCoreSubscriptions();
        }

        public void Dispose()
        {
            if (this.Running)
            {
                this.Stop();
            }

            this.Messages.Dispose();

            _peers.Clear();
            _publishers.Clear();
            _outogingMessageSubscribers.Clear();
        }

        public Simpit AddPeer(SimpitPeer peer)
        {
            _peers.Add(peer);

            if (this.Running == true && peer.Running == false)
            {
                peer.Start(this);
            }

            return this;
        }

        public Simpit RemovePeer(SimpitPeer peer)
        {
            if (_peers.Remove(peer) == false)
            {
                return this;
            }

            if (this.Running == true && peer.Running == true)
            {
                peer.Stop(this);
            }

            return this;
        }

        public Simpit Start()
        {
            _cancellationTokenSource = new CancellationTokenSource();

            foreach (SimpitPeer peer in _peers)
            {
                if (peer.Running == false)
                {
                    peer.Start(this);
                }
            }

            this.Running = true;

            return this;
        }

        public void Stop()
        {
            foreach (SimpitPeer peer in _peers)
            {
                if (peer.Running == true)
                {
                    peer.Stop(this);
                }
            }

            this.Running = true;
        }

        public void Broadcast<T>(T content)
            where T : ISimpitMessageContent

        {
            if (this.Messages.TryGetOutgoingType<T>(out SimpitMessageType<T> type) == false)
            {
                _logger.LogWarning("{0}::{1} - Attempting to broadcast unrecognized message type {2}", nameof(Simpit), nameof(Broadcast), typeof(T).Name);
                return;
            }

            ISimpitMessage message = new SimpitMessage<T>(type, content);
            foreach (SerialPeer peer in _outogingMessageSubscribers.Get(type))
            {
                peer.EnqueueOutgoing(message);
            }
        }

        public void Flush()
        {
            foreach (SimpitPeer peer in _peers)
            {
                while (peer.TryRead(out ISimpitMessage message))
                {
                    this.GetPublisher(message.Type.ContentType).Publish(peer, message);
                }
            }
        }

        public Simpit AddIncomingConsumer<T>(ISimpitMessageConsumer<T> subscriber)
            where T : ISimpitMessageContent
        {
            SimpitMessagePublisher<T> publisher = this.GetPublisher(typeof(T)) as SimpitMessagePublisher<T>;
            publisher.AddConsumer(subscriber);

            return this;
        }

        private SimpitMessagePublisher GetPublisher(Type type)
        {
            if (_publishers.TryGetValue(type, out SimpitMessagePublisher publisher))
            {
                return publisher;
            }

            publisher = SimpitMessagePublisher.Create(type);
            _publishers.Add(type, publisher);

            return publisher;
        }
    }
}
