using KerbalSimpit.Common.Core;
using KerbalSimpit.Unity.Common.Providers;
using ResourceMessagess = KerbalSimpit.Core.KSP.Messages.Resource;

namespace KerbalSimpit.Unity.KSP1.Providers
{
    public static partial class ReosurceProviders
    {
        public abstract class BaseResourceProvider<T> : GenericUpdateProvider<T>
            where T : unmanaged, ISimpitMessageData, ResourceMessagess.IBasicResource
        {
            private readonly PartResourceDefinition _resource;

            public BaseResourceProvider(string resourceName = null)
            {
                if (resourceName == null)
                {
                    resourceName = typeof(T).Name;
                }

                _resource = PartResourceLibrary.Instance.GetDefinition(resourceName);
            }

            protected override T GetOutgoingData()
            {
                T instance = new T();

                FlightGlobals.ActiveVessel.GetConnectedResourceTotals(_resource.id, out double available, out double max);
                instance.Available = (float)available;
                instance.Max = (float)max;

                return instance;
            }
        }

        public class LiquidFuelProvider : BaseResourceProvider<ResourceMessagess.LiquidFuel> { }
        public class OxidizerProvider : BaseResourceProvider<ResourceMessagess.Oxidizer> { }
        public class SolidFuelProvider : BaseResourceProvider<ResourceMessagess.SolidFuel> { }
        public class MonoPropellantProvider : BaseResourceProvider<ResourceMessagess.MonoPropellant> { }
        public class ElectricChargeProvider : BaseResourceProvider<ResourceMessagess.ElectricCharge> { }
        public class OreProvider : BaseResourceProvider<ResourceMessagess.Ore> { }
        public class AblatorProvider : BaseResourceProvider<ResourceMessagess.Ablator> { }
        public class XenonGasProvider : BaseResourceProvider<ResourceMessagess.XenonGas> { }
    }
}
