using KerbalSimpit.Common.Core;

namespace KerbalSimpit.Unity.Common.Providers
{
    public abstract class GenericUpdateProvider<T> : GenericProvider<T>
        where T : unmanaged, ISimpitMessageData
    {
        public virtual void Update()
        {
            this.CleanOutgoingData();
        }
    }
}
