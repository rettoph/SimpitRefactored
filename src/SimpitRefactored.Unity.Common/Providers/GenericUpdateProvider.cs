using SimpitRefactored.Common.Core;

namespace SimpitRefactored.Unity.Common.Providers
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
