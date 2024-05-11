using KerbalSimpit.Core;
using KerbalSimpit.Core.KSP.Extensions;
using KerbalSimpit.Unity.Common;

namespace KerbalSimpit.Unity.KSP1
{
    [KSPAddon(KSPAddon.Startup.Instantly, false)]
    public class KerbalSimpitUnityKSP1 : KerbalSimpitUnity
    {
        protected override Simpit ConfigureSimpit(Simpit simpit)
        {
            return simpit.RegisterKerbal();
        }
    }
}
