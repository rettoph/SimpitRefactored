using KerbalSimpit.Core.Utilities;
using System;

namespace KerbalSimpit.Core
{
    public partial class Simpit
    {
        public abstract class OutgoingData
        {
            public abstract int ChangeId { get; }
        }

        public abstract class OutgoingData<T> : OutgoingData
            where T : ISimpitMessageData
        {
            private int _changeId;
            private T _data;

            public override int ChangeId => _changeId;
            public T Value
            {
                get => _data;
                set
                {
                    if (this.Equals(in _data, in value) == true)
                    { // No change
                        return;
                    }

                    _data = value;
                    _changeId++;
                }
            }

            protected OutgoingData()
            {

            }

            protected abstract bool Equals(in T x, in T y);

            public static OutgoingData<T> Create()
            {
                try
                {
                    Type unmanagedOutgoinfDataType = typeof(UnmanagedOutgoingData<>).MakeGenericType(typeof(T));
                    return (OutgoingData<T>)Activator.CreateInstance(unmanagedOutgoinfDataType);
                }
                catch
                {
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

}
