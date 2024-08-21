using SimpitRefactored.Core;
using UnityEngine;

namespace SimpitRefactored.Unity.Common
{
    public abstract class SimpitBehaviour : MonoBehaviour
    {
        protected Simpit simpit => SimpitConfigurationBehavior.Simpit;
        protected SimpitLogger logger => SimpitConfigurationBehavior.Logger;

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
