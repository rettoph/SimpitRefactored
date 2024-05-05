using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace KerbalSimpit.Core.Kerbal.Messages
{
    public static class TACLS
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct Resource : ISimpitMessageContent
        {
            public float CurrentFood { get; set; }
            public float MaxFood { get; set; }
            public float CurrentWater { get; set; }
            public float MaxWater { get; set; }
            public float CurrentOxygen { get; set; }
            public float MaxOxygen { get; set; }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct Waste : ISimpitMessageContent
        {
            public float CurrentWaste { get; }
            public float MaxWaste { get; }
            public float CurrentLiquidWaste { get; }
            public float MaxLiquidWaste { get; }
            public float CurrentCO2 { get; }
            public float MaxCO2 { get; }
        }
    }
}
