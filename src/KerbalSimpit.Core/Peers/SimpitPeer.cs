using KerbalSimpit.Core.Constants;
using KerbalSimpit.Core.Enums;
using KerbalSimpit.Core.Utilities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KerbalSimpit.Core.Peers
{
    public abstract partial class SimpitPeer
    {
        private ConcurrentQueue<ISimpitMessage> _read;
        private ConcurrentQueue<ISimpitMessage> _write;

        private readonly ISimpitLogger _logger;
        private readonly SimpitStream _inbound;
        private readonly SimpitStream _outbound;
        private readonly SimpitMessageSerializer _serializer;

        protected ConcurrentQueue<ISimpitMessage> read => _read;

        public abstract bool Running { get; }

        public ConnectionStatusEnum Status { get; private set; }

        public SimpitPeer(ISimpitLogger logger)
        {
            _logger = logger;
            _read = new ConcurrentQueue<ISimpitMessage>();
            _write = new ConcurrentQueue<ISimpitMessage>();
            _serializer = new SimpitMessageSerializer(_logger);

            _inbound = new SimpitStream();
            _outbound = new SimpitStream();
        }

        public void Dispose()
        {
            if (this.Running)
            {
                this.Stop();
            }
        }

        public abstract void Start(CancellationToken cancellationToken);

        public abstract void Stop();

        /// <summary>
        /// Enqueue an outgoing message to be sent within the outbound thread.
        /// </summary>
        /// <param name="message"></param>
        public void EnqueueOutbound(ISimpitMessage message)
        {
            _write.Enqueue(message);
        }

        public bool TryRead(out ISimpitMessage message)
        {
            return _read.TryDequeue(out message);
        }

        public void Clear()
        {
            _inbound.Clear();
            _outbound.Clear();

            while (_read.TryDequeue(out _))
            {
                //
            }
        }

        public void InboundDataRecieved(byte data)
        {
            _inbound.Write(data);

            if (data != SpecialBytes.EndOfMessage)
            { // More data to be read before we've recieved a complete message
                return;
            }

            if (_serializer.TryDeserialize(_inbound, out ISimpitMessage message) == false)
            {
                _logger.LogWarning("{0}::{1} - Unable to deserialize incoming message.", nameof(SimpitPeer), nameof(InboundDataRecieved));
                return;
            }

            _logger.LogVerbose("{0}::{1} - Recieved message {2}", nameof(SimpitPeer), nameof(InboundDataRecieved), message.GetType().Name);
            this.read.Enqueue(message);
        }

        protected bool GetOutbound(out SimpitStream outbound)
        {
            outbound = _outbound;

            if (_write.TryDequeue(out ISimpitMessage message))
            {
                return _serializer.TrySerialize(outbound, message);
            }

            return false;
        }
    }
}
