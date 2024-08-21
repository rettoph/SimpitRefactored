using PimDeWitte.UnityMainThreadDispatcher;
using SimpitRefactored.Core;
using UnityEngine;

namespace SimpitRefactored.Unity.Common
{
    public abstract class SimpitConfigurationBehavior : MonoBehaviour
    {
        internal static SimpitLogger Logger { get; private set; }
        internal static Simpit Simpit { get; private set; }

        public virtual void Start()
        {
            DontDestroyOnLoad(this);

            this.gameObject.AddComponent<UnityMainThreadDispatcher>();

            SimpitConfigurationBehavior.Logger = new SimpitLogger();

            SimpitConfigurationBehavior.Simpit = this.ConfigureSimpit(new Simpit(SimpitConfigurationBehavior.Logger)).Start(SimpitConfiguration.Instance);
        }

        public virtual void Update()
        {
            SimpitConfigurationBehavior.Simpit.Flush();
        }

        protected abstract Simpit ConfigureSimpit(Simpit simpit);
    }
}
