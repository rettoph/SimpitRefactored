using KerbalSimpit.Core.Constants;
using KerbalSimpit.Core.Enums;
using KerbalSimpit.Core.Utilities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace KerbalSimpit.Core.Peers
{
    public abstract partial class SimpitPeer
    {
        private Simpit _simpit;
        private ConnectionStatusEnum _status;
        private bool _initialized;

        private readonly ConcurrentQueue<ISimpitMessage> _read;
        private readonly ConcurrentQueue<ISimpitMessage> _write;
        private readonly SimpitStream _incoming;
        private readonly SimpitStream _outgoing;
        private readonly SimpitStream _encodeBuffer;
        private readonly SimpitStream _decodeBuffer;
        private readonly ConcurrentDictionary<SimpitMessageType, int> _outgoingSubscriptions;

        protected ISimpitLogger logger => _simpit.Logger;
        protected CancellationToken cancellationToken => _simpit.CancellationToken;

        public abstract bool Running { get; }

        public IEnumerable<SimpitMessageType> OutgoingSubscriptions => _outgoingSubscriptions.Keys;

        public ConnectionStatusEnum Status
        {
            get => _status;
            set
            {
                _status = value;
                this.OnStatusChanged?.Invoke(this, _status);
            }
        }

        public event EventHandler<ConnectionStatusEnum> OnStatusChanged;
        public event EventHandler<SimpitMessageType> OnOutgoingSubscribed;
        public event EventHandler<SimpitMessageType> OnOutgoingUnsubscribed;
        public event EventHandler<ISimpitMessage> OnIncomingMessage;
        public event EventHandler<ISimpitMessage> OnOutgoingMessage;

        public SimpitPeer()
        {
            _read = new ConcurrentQueue<ISimpitMessage>();
            _write = new ConcurrentQueue<ISimpitMessage>();
            _outgoingSubscriptions = new ConcurrentDictionary<SimpitMessageType, int>();

            _incoming = new SimpitStream();
            _outgoing = new SimpitStream();
            _encodeBuffer = new SimpitStream();
            _decodeBuffer = new SimpitStream();
        }

        public void Dispose()
        {
            if (this.Running)
            {
                this.TryClose();
            }
        }

        public virtual void Initialize(Simpit simpit)
        {
            if (_initialized == true)
            {
                throw new InvalidOperationException($"{nameof(SimpitPeer)}::{nameof(Initialize)} - Already initialized.");
            }

            _simpit = simpit;
            _initialized = true;
        }

        public void Open()
        {
            if (_initialized == false)
            {
                throw new InvalidOperationException(string.Format("{0}::{1} - Unable to start, uninitialized.", nameof(SimpitPeer), nameof(Open), nameof(SimpitPeer)));
            }

            if (this.Status != ConnectionStatusEnum.CLOSED)
            {
                throw new InvalidOperationException(string.Format("{0}::{1} - Unable to start, already open.", nameof(SimpitPeer), nameof(Open), nameof(SimpitPeer)));
            }

            if (this.TryOpen() == true)
            {
                this.Reset();

                this.Status = ConnectionStatusEnum.WAITING_HANDSHAKE;
            }
        }

        public void Close()
        {
            if (this.TryClose())
            {
                this.Reset();

                this.Status = ConnectionStatusEnum.CLOSED;
            }
        }

        protected abstract bool TryOpen();

        protected abstract bool TryClose();

        /// <summary>
        /// Enqueue an outgoing message to be sent within the outbound thread.
        /// </summary>
        /// <param name="message"></param>
        public void EnqueueOutgoing(ISimpitMessage message)
        {
            _write.Enqueue(message);
        }

        /// <summary>
        /// Enqueue an outgoing message to be sent within the outbound thread.
        /// </summary>
        /// <param name="message"></param>
        public void EnqueueOutgoing<T>(T content)
            where T : ISimpitMessageData
        {
            try
            {
                this.EnqueueOutgoing(_simpit.Messages.CreateOutgoing(content));
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "{0}::{1} - Exception", nameof(SimpitPeer), nameof(EnqueueOutgoing));
            }
        }

        public bool TryRead(out ISimpitMessage message)
        {
            if (_read.TryDequeue(out message))
            {
                this.OnIncomingMessage?.Invoke(this, message);
                return true;
            }

            return false;
        }

        public void Clear()
        {
            _incoming.Clear();
            _outgoing.Clear();

            while (_read.TryDequeue(out _))
            {
                //
            }
        }

        public void IncomingDataRecieved(byte data)
        {
            _incoming.Write(data);

            if (data != SpecialBytes.EndOfMessage)
            { // More data to be read before we've recieved a complete message
                return;
            }

            if (_simpit.Messages.TryDeserializeIncoming(_incoming, _decodeBuffer, out ISimpitMessage message) == false)
            {
                this.logger.LogWarning("{0}::{1} - Unable to deserialize incoming message. Peer = {2}", nameof(SimpitPeer), nameof(IncomingDataRecieved), this);
                return;
            }

            this.logger.LogDebug("{0}::{1} - Peer {2} recieved message {3}", nameof(SimpitPeer), nameof(IncomingDataRecieved), this, message.Type);
            _read.Enqueue(message);
        }

        protected bool DequeueOutgoing(out SimpitStream outgoing)
        {
            outgoing = _outgoing;

            if (_write.TryDequeue(out ISimpitMessage message) == false)
            {
                return false;
            }

            if (_simpit.Messages.TrySerializeOutgoing(message, _outgoing, _encodeBuffer) == false)
            {
                return false;
            }

            this.logger.LogDebug("{0}::{1} - Peer {2} sending message {3}", nameof(SimpitPeer), nameof(DequeueOutgoing), this, message.Type);
            this.OnOutgoingMessage?.Invoke(this, message);

            return true;
        }

        protected int GetEnqueuedOutgoingCount()
        {
            return _write.Count;
        }

        protected void EnqueueOutgoingSubscriptions()
        {
            try
            {
                foreach (KeyValuePair<SimpitMessageType, int> kvp in _outgoingSubscriptions)
                {
                    kvp.Key.TryEnqueueOutgoingData(this, kvp.Value, _simpit, false);
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "{0}::{1}, Exception", nameof(SimpitPeer), nameof(EnqueueOutgoingSubscription));
            }
        }

        internal void ForceEnqueueOutgoingSubscriptions()
        {
            try
            {
                foreach (KeyValuePair<SimpitMessageType, int> kvp in _outgoingSubscriptions)
                {
                    kvp.Key.TryEnqueueOutgoingData(this, kvp.Value, _simpit, true);
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "{0}::{1}, Exception", nameof(SimpitPeer), nameof(ForceEnqueueOutgoingSubscriptions));
            }

        }

        internal void EnqueueOutgoingSubscription<T>(SimpitMessageType type, OutgoingData<T> data)
            where T : ISimpitMessageData
        {
            this.EnqueueOutgoing(data.Value);
            _outgoingSubscriptions[type] = data.ChangeId;
        }

        private void Reset()
        {
            // Ensure all buffers are reset
            _incoming.Clear();
            _outgoing.Clear();
            _decodeBuffer.Clear();
            _encodeBuffer.Clear();

            // Clear any old subscriptions
            while (_outgoingSubscriptions.Count > 0)
            {
                SimpitMessageType type = _outgoingSubscriptions.First().Key;
                _outgoingSubscriptions.TryRemove(type, out _);
                _simpit.GetOutgoingData(type).RemoveSubscriber(this);
                this.OnOutgoingUnsubscribed?.Invoke(this, type);
            }
            _outgoingSubscriptions.Clear();
        }
    }
}
