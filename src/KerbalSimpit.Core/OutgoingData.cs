using KerbalSimpit.Core.Peers;
using KerbalSimpit.Core.Utilities;
using System;
using System.Collections.Generic;

namespace KerbalSimpit.Core
{
    public abstract class OutgoingData
    {
        internal HashSet<SimpitPeer> _subscribers;

        public abstract int ChangeId { get; }

        public IEnumerable<SimpitPeer> Subscribers => _subscribers;

        protected internal OutgoingData()
        {
            _subscribers = new HashSet<SimpitPeer>();
        }

        internal void AddSubscriber(SimpitPeer peer)
        {
            lock (this)
            {
                _subscribers.Add(peer);
            }
        }

        internal void RemoveSubscriber(SimpitPeer peer)
        {
            lock (this)
            {
                _subscribers.Remove(peer);
            }
        }
    }

    public abstract class OutgoingData<T> : OutgoingData
        where T : ISimpitMessageData
    {
        private int _changeId;
        private T _value;

        public override int ChangeId => _changeId;
        public T Value
        {
            get => _value;
        }

        protected internal OutgoingData()
        {

        }

        public void SetValue(in T value, bool force)
        {
            if (force == true || this.Equals(in _value, in value) == false)
            {
                _value = value;
                _changeId++;
            }
        }

        protected abstract bool Equals(in T x, in T y);

        public static OutgoingData<T> Create()
        {
            try
            {
                // Attempt to create an unmanaged contained first for fast unmanaged value type comparison when possible
                Type unmanagedOutgoinfDataType = typeof(UnmanagedOutgoingData<>).MakeGenericType(typeof(T));
                return (OutgoingData<T>)Activator.CreateInstance(unmanagedOutgoinfDataType);
            }
            catch
            {
                // As a fallback use managed contained. This will incur boxing when used with value types
                Type managedOutgoinfDataType = typeof(ManagedOutgoingData<>).MakeGenericType(typeof(T));
                return (OutgoingData<T>)Activator.CreateInstance(managedOutgoinfDataType);
            }
        }
    }

    internal sealed class UnmanagedOutgoingData<T> : OutgoingData<T>
        where T : unmanaged, ISimpitMessageData
    {
        protected override bool Equals(in T x, in T y)
        {
            return Unmanaged.Equals<T>(in x, in y);
        }
    }

    internal sealed class ManagedOutgoingData<T> : OutgoingData<T>
        where T : ISimpitMessageData
    {
        protected override bool Equals(in T x, in T y)
        {
            return x.Equals(y);
        }
    }

}
