namespace KerbalSimpit.Core
{
    public partial class Simpit
    {
        public abstract class OutgoingData
        {
            public abstract int ChangeId { get; }
        }
        public sealed class OutgoingData<T> : OutgoingData
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
                    _data = value;
                    _changeId++;
                }
            }
        }
    }

}
