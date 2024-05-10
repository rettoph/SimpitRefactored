using KerbalSimpit.Core;
using KerbalSimpit.Core.KSP.Extensions;
using KerbalSimpit.Unity.Common;
using UnityEngine;

namespace KerbalSimpit.Unity.KSP1
{
    [KSPAddon(KSPAddon.Startup.Instantly, false)]
    public class KerbalSimpitUnity : MonoBehaviour
    {
        public static readonly SimpitLogger Logger = new SimpitLogger();
        public static readonly Simpit Simpit = new Simpit(KerbalSimpitUnity.Logger).RegisterKerbal().Start(SimpitConfiguration.Instance);

        public void Start()
        {
            DontDestroyOnLoad(this);
        }

        public void Update()
        {
            KerbalSimpitUnity.Simpit.Flush();
        }
    }
}
