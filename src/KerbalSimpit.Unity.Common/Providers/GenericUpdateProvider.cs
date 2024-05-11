using KerbalSimpit.Core;

namespace KerbalSimpit.Unity.Common.Providers
{
    public abstract class GenericUpdateProvider<T> : GenericProvider<T>
        where T : ISimpitMessageData
    {
        public virtual void Update()
        {
            this.CleanOutgoingData();
        }
    }
}
