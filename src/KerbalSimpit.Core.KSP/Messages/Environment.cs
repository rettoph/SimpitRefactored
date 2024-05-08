using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace KerbalSimpit.Core.KSP.Messages
{
    public static class Environment
    {
        public struct TargetInfo : ISimpitMessageData
        {
            public float Distance { get; set; }
            public float Velocity { get; set; }
            public float Heading { get; set; }
            public float Pitch { get; set; }
            public float VelocityHeading { get; set; }
            public float VelocityPitch { get; set; }
        }

        public struct SoIName : ISimpitMessageData
        {
            public string Value { get; set; }

            internal static void Serialize(SoIName input, SimpitStream output)
            {
                output.Write(input.Value);
            }
        }

        public struct SceneChange : ISimpitMessageData
        {
            public enum SceneChangeTypeEnum
            {
                Flight = 0x0,
                NotFlight = 0x1
            }

            public SceneChangeTypeEnum Type;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct FlightStatus : ISimpitMessageData
        {
            [Flags]
            public enum StatusFlags : byte
            {
                IsInFlight = 1,
                IsEva = 2,
                IsRecoverable = 4,
                IsInAtmoTW = 8,
                ComnetControlLevel0 = 16,
                ComnetControlLevel1 = 32,
                HasTargetSet = 64
            }

            public StatusFlags Status;
            public byte VesselSituation; // See Vessel.Situations for possible values
            public byte CurrentTWIndex;
            public byte CrewCapacity;
            public byte CrewCount;
            public byte CommNetSignalStrenghPercentage;
            public byte CurrentStage;
            public byte VesselType;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct AtmoCondition : ISimpitMessageData
        {
            [Flags]
            public enum AtmoCharacteristicsFlags : byte
            {
                HasAtmosphere = 1,
                HasOxygen = 2,
                IsVesselInAtmosphere = 4
            }

            public AtmoCharacteristicsFlags AtmoCharacteristics;
            public float AirDensity;
            public float Temperature;
            public float Pressure;
        }

        public struct VesselName : ISimpitMessageData
        {
            public string Value { get; set; }

            internal static void Serialize(VesselName input, SimpitStream output)
            {
                output.Write(input.Value);
            }
        }

        public struct VesselChange : ISimpitMessageData
        {
            public enum TypeEnum : byte
            {
                Switching = 1,
                Docking = 2,
                Undocking = 3
            }

            public TypeEnum Type { get; set; }
        }
    }
}
