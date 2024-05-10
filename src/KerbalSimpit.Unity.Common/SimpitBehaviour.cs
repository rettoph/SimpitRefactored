using KerbalSimpit.Core;
using UnityEngine;

namespace KerbalSimpit.Unity.Common
{
    public abstract class SimpitBehaviour : MonoBehaviour
    {
        protected Simpit simpit => KerbalSimpitUnity.Simpit;
        protected SimpitLogger logger => KerbalSimpitUnity.Logger;
    }
}
