using KerbalSimpit.Common.Core;
using System.Runtime.InteropServices;

namespace KerbalSimpit.Core.KSP.Messages
{
    public static class CustomResource
    {
        public interface ICustomResource : ISimpitMessageData
        {
            float CurrentResource1 { get; set; }
            float MaxResource1 { get; set; }
            float CurrentResource2 { get; set; }
            float MaxResource2 { get; set; }
            float CurrentResource3 { get; set; }
            float MaxResource3 { get; set; }
            float CurrentResource4 { get; set; }
            float MaxResource4 { get; set; }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct One : ICustomResource
        {
            public float CurrentResource1 { get; set; }
            public float MaxResource1 { get; set; }
            public float CurrentResource2 { get; set; }
            public float MaxResource2 { get; set; }
            public float CurrentResource3 { get; set; }
            public float MaxResource3 { get; set; }
            public float CurrentResource4 { get; set; }
            public float MaxResource4 { get; set; }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct Two : ICustomResource
        {
            public float CurrentResource1 { get; set; }
            public float MaxResource1 { get; set; }
            public float CurrentResource2 { get; set; }
            public float MaxResource2 { get; set; }
            public float CurrentResource3 { get; set; }
            public float MaxResource3 { get; set; }
            public float CurrentResource4 { get; set; }
            public float MaxResource4 { get; set; }
        }
    }
}
