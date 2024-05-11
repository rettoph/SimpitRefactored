using KerbalSimpit.Core;
using UnityEngine;

namespace KerbalSimpit.Unity.Common
{
    public abstract class SimpitBehaviour : MonoBehaviour
    {
        protected Simpit simpit => KerbalSimpitUnity.Simpit;
        protected SimpitLogger logger => KerbalSimpitUnity.Logger;

        public virtual void Start()
        {
            this.simpit.AddIncomingSubscribers(this);
        }

        public virtual void OnDestroy()
        {
            this.simpit.RemoveIncomingSubscribers(this);
        }
    }
}
