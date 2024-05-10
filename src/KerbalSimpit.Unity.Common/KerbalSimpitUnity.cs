using KerbalSimpit.Core;
using PimDeWitte.UnityMainThreadDispatcher;
using UnityEngine;

namespace KerbalSimpit.Unity.Common
{
    public abstract class KerbalSimpitUnity : MonoBehaviour
    {
        public static SimpitLogger Logger { get; private set; }
        public static Simpit Simpit { get; private set; }

        public virtual void Start()
        {
            DontDestroyOnLoad(this);

            this.gameObject.AddComponent<UnityMainThreadDispatcher>();

            KerbalSimpitUnity.Logger = new SimpitLogger();

            KerbalSimpitUnity.Simpit = this.ConfigureSimpit(new Simpit(KerbalSimpitUnity.Logger)).Start(SimpitConfiguration.Instance);
        }

        public virtual void Update()
        {
            KerbalSimpitUnity.Simpit.Flush();
        }

        protected abstract Simpit ConfigureSimpit(Simpit simpit);
    }
}
