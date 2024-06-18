using KerbalSimpit.Common.Core;
using System.Runtime.InteropServices;

namespace KerbalSimpit.Core.KSP.Messages
{
    public class Resource
    {
        public interface IBasicResource : ISimpitMessageData
        {
            float Max { get; set; }
            float Available { get; set; }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct LiquidFuel : IBasicResource
        {
            public float Max { get; set; }
            public float Available { get; set; }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct LiquidFuelStage : IBasicResource
        {
            public float Max { get; set; }
            public float Available { get; set; }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct Methane : IBasicResource
        {
            public float Max { get; set; }
            public float Available { get; set; }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct MethaneStage : IBasicResource
        {
            public float Max { get; set; }
            public float Available { get; set; }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct Oxidizer : IBasicResource
        {
            public float Max { get; set; }
            public float Available { get; set; }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct OxidizerStage : IBasicResource
        {
            public float Max { get; set; }
            public float Available { get; set; }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct SolidFuel : IBasicResource
        {
            public float Max { get; set; }
            public float Available { get; set; }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct SolidFuelStage : IBasicResource
        {
            public float Max { get; set; }
            public float Available { get; set; }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct XenonGas : IBasicResource
        {
            public float Max { get; set; }
            public float Available { get; set; }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct XenonGasStage : IBasicResource
        {
            public float Max { get; set; }
            public float Available { get; set; }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct MonoPropellant : IBasicResource
        {
            public float Max { get; set; }
            public float Available { get; set; }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct EvaPropellant : IBasicResource
        {
            public float Max { get; set; }
            public float Available { get; set; }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct ElectricCharge : IBasicResource
        {
            public float Max { get; set; }
            public float Available { get; set; }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct Ore : IBasicResource
        {
            public float Max { get; set; }
            public float Available { get; set; }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct Ablator : IBasicResource
        {
            public float Max { get; set; }
            public float Available { get; set; }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct AblatorStage : IBasicResource
        {
            public float Max { get; set; }
            public float Available { get; set; }
        }
    }
}
