using KerbalSimpit.Common.Core;
using KerbalSimpit.Common.Core.Utilities;
using KerbalSimpit.Core.Configuration;
using KerbalSimpit.Core.Extensions;
using KerbalSimpit.Core.Peers;
using KerbalSimpit.Core.Services;
using KerbalSimpit.Core.Utilities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace KerbalSimpit.Core
{
    public partial class Simpit : IDisposable
    {
        public static Simpit Instance { get; private set; }

        private readonly ISimpitLogger _logger;
        private readonly List<SimpitPeer> _peers;
        private readonly ConcurrentDictionary<Type, SimpitMessagePublisher> _publishers;
        private readonly ConcurrentDictionary<SimpitMessageType, OutgoingData> _outgoing;

        private CancellationTokenSource _cancellationTokenSource;

        public readonly ReadOnlyCollection<SimpitPeer> Peers;
        public readonly SimpitMessageService Messages;

        public CancellationToken CancellationToken => _cancellationTokenSource.Token;

        public bool Running { get; private set; }
        public ISimpitLogger Logger => _logger;
        public ISimpitConfiguration Configuration { get; private set; }

        public event EventHandler<SimpitPeer> OnPeerAdded;
        public event EventHandler<SimpitPeer> OnPeerRemoved;

        public event EventHandler<ISimpitMessage> OnMessageRecieved;

        public Simpit(ISimpitLogger logger)
        {
            _logger = logger;
            _peers = new List<SimpitPeer>();
            _publishers = new ConcurrentDictionary<Type, SimpitMessagePublisher>();
            _outgoing = new ConcurrentDictionary<SimpitMessageType, OutgoingData>();

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
            _outgoing.Clear();
        }

        public Simpit AddPeer(SimpitPeer peer)
        {
            _peers.Add(peer);

            peer.Initialize(this);

            if (this.Running == true && peer.Running == false)
            {
                peer.Open();
            }

            this.OnPeerAdded?.Invoke(this, peer);

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
                peer.Close();
            }

            this.OnPeerRemoved?.Invoke(this, peer);

            return this;
        }

        public Simpit Start(ISimpitConfiguration configuration)
        {
            _cancellationTokenSource = new CancellationTokenSource();
            this.Configuration = configuration;

            // TODO: This should not be hardcoded for serial peers.
            foreach (ISerialConfiguration serial in configuration.Serial)
            {
                this.AddSerialPeer(serial.PortName, serial.BaudRate);
            }

            foreach (SimpitPeer peer in _peers)
            {
                if (peer.Running == false)
                {
                    peer.Open();
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
                    peer.Close();
                }
            }

            this.Running = true;
        }

        public void OpenAll()
        {
            foreach (SimpitPeer peer in _peers)
            {
                peer.Open();
            }
        }

        public void CloseAll()
        {
            foreach (SimpitPeer peer in _peers)
            {
                peer.Close();
            }
        }

        public void SetOutgoingData<T>(T value, bool force = false)
            where T : unmanaged, ISimpitMessageData

        {
            if (this.Messages.TryGetOutgoingType<T>(out SimpitMessageType<T> type) == false)
            {
                throw new InvalidOperationException(string.Format("{0}::{1} - Unknown outgoing message type {2}", nameof(Simpit), nameof(SetOutgoingData), typeof(T).Name));
            }

            OutgoingData<T> data = this.GetOutgoingData(type);
            lock (data)
            {
                data.SetValue(value, force);
            }
        }

        public IEnumerable<SimpitPeer> GetOutgoingSubscribers<T>()
            where T : unmanaged, ISimpitMessageData
        {
            if (this.Messages.TryGetOutgoingType<T>(out SimpitMessageType<T> type) == false)
            {
                throw new InvalidOperationException(string.Format("{0}::{1} - Unknown outgoing message type {2}", nameof(Simpit), nameof(GetOutgoingSubscribers), typeof(T).Name));
            }

            foreach (SimpitPeer peer in _peers)
            {
                if (peer.OutgoingSubscriptions.Contains(type) == true)
                {
                    yield return peer;
                }
            }
        }

        public OutgoingData<T> GetOutgoingData<T>(SimpitMessageType<T> type)
            where T : unmanaged, ISimpitMessageData
        {
            if (_outgoing.TryGetValue(type, out OutgoingData uncasted))
            {
                return (OutgoingData<T>)uncasted;
            }

            OutgoingData<T> data = (OutgoingData<T>)type.CreateOutgoingData();
            _outgoing.TryAdd(type, data);

            return data;
        }

        public OutgoingData GetOutgoingData(SimpitMessageType type)
        {
            if (_outgoing.TryGetValue(type, out OutgoingData data))
            {
                return data;
            }

            data = type.CreateOutgoingData();
            _outgoing.TryAdd(type, data);

            return data;
        }

        public void Flush()
        {
            foreach (SimpitPeer peer in _peers)
            {
                while (peer.TryRead(out ISimpitMessage message))
                {
                    this.OnMessageRecieved?.Invoke(peer, message);
                    this.GetPublisher(message.Type.DataType).Publish(peer, message);
                }
            }
        }

        public Simpit AddIncomingSubscriber<T>(ISimpitMessageSubscriber<T> subscriber)
            where T : unmanaged, ISimpitMessageData
        {
            SimpitMessagePublisher<T> publisher = this.GetPublisher(typeof(T)) as SimpitMessagePublisher<T>;
            publisher.AddSubscriber(subscriber);

            return this;
        }

        public Simpit RemoveIncomingSubscriber<T>(ISimpitMessageSubscriber<T> subscriber)
            where T : unmanaged, ISimpitMessageData
        {
            SimpitMessagePublisher<T> publisher = this.GetPublisher(typeof(T)) as SimpitMessagePublisher<T>;
            publisher.RemoveSubscriber(subscriber);

            return this;
        }

        private static MethodInfo GenericAddIncomingSubscriberMethod = typeof(Simpit).GetMethod(nameof(Simpit.AddIncomingSubscriber));
        public Simpit AddIncomingSubscribers(object instance)
        {
            IEnumerable<Type> subscriberTypes = instance.GetType().GetInterfaces().Where(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(ISimpitMessageSubscriber<>));

            foreach (Type subscriberType in subscriberTypes)
            {
                Type messageDataType = subscriberType.GenericTypeArguments[0];
                MethodInfo addIncomingSubscriberMethod = GenericAddIncomingSubscriberMethod.MakeGenericMethod(messageDataType);
                addIncomingSubscriberMethod.Invoke(this, new object[] { instance });
            }

            return this;
        }

        private static MethodInfo GenericRemoveIncomingSubscriberMethod = typeof(Simpit).GetMethod(nameof(Simpit.RemoveIncomingSubscriber));
        public Simpit RemoveIncomingSubscribers(object instance)
        {
            IEnumerable<Type> subscriberTypes = instance.GetType().GetInterfaces().Where(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(ISimpitMessageSubscriber<>));

            foreach (Type subscriberType in subscriberTypes)
            {
                Type messageDataType = subscriberType.GenericTypeArguments[0];
                MethodInfo removeIncomingSubscriberMethod = GenericRemoveIncomingSubscriberMethod.MakeGenericMethod(messageDataType);
                removeIncomingSubscriberMethod.Invoke(this, new object[] { instance });
            }

            return this;
        }

        private SimpitMessagePublisher GetPublisher(Type type)
        {
            if (_publishers.TryGetValue(type, out SimpitMessagePublisher publisher))
            {
                return publisher;
            }

            publisher = SimpitMessagePublisher.Create(type, this);
            _publishers.TryAdd(type, publisher);

            return publisher;
        }
    }
}
