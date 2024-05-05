using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KerbalSimpit.Core.Kerbal.Constants
{
    internal static class MessageTypeIds
    {
        public static class Outgoing
        {
            #region Propulsion Resources
            public const byte LiquidFuel = 10;
            public const byte LiquidFuelStage = 11;
            public const byte Methane = 10;
            public const byte MethaneStage = 11;
            public const byte Oxidizer = 12;
            public const byte OxidizerStage = 13;
            public const byte SolidFuel = 14;
            public const byte SolidFuelStage = 15;
            public const byte XenonGas = 28;
            public const byte XenonGasStage = 29;
            public const byte MonoPropellant = 16;
            public const byte EvaPropellant = 18;

            //Propulsion Resources on KSP2
            public const byte IntakeAir = 52;
            //public const byte TestRocks = xx; //Available in KSP2 but seems to be unused
            public const byte Hydrogen = 53;
            public const byte HydrogenStage = 54;
            //public const byte Methalox = xx; //Available in KSP2 but seems to be unused
            //public const byte MethaloxStage = xx; //Available in KSP2 but seems to be unused
            //public const byte MethaneAir = xx; //Available in KSP2 but seems to be unused
            //public const byte MethaneAirStage = xx; //Available in KSP2 but seems to be unused
            public const byte Uranium = 55;
            //public const byte XenonEC = xx; //Available in KSP2 but seems to be unused
            //public const byte XenonECStage = xx; //Available in KSP2 but seems to be unused
            #endregion

            #region Vessel Resources
            public const byte ElectricCharge = 17;
            public const byte Ore = 19;
            public const byte Ablator = 20;
            public const byte AblatorStage = 21;
            public const byte TACLSResource = 30;
            public const byte TACLSWaste = 31;
            public const byte CustomResource1 = 32;
            public const byte CustomResource2 = 33;
            #endregion

            #region Vessel Movement/Postion
            public const byte Altitude = 8;
            public const byte Velocity = 22;
            public const byte Airspeed = 27;
            public const byte Apsides = 9;
            public const byte ApsidesTime = 24;
            public const byte ManeuverData = 34;
            public const byte SASInfo = 35;
            public const byte OrbitInfo = 36;
            public const byte Rotation = 45;
            #endregion

            #region Vessel Commands
            public const byte RotationCmd = 47;
            public const byte TranslationCmd = 48;
            public const byte WheelCmd = 49;
            public const byte ThrottleCmd = 50;
            #endregion

            #region Vessel Details
            public const byte ActionGroups = 37;
            public const byte DeltaV = 38;
            public const byte DeltaVEnv = 39;
            public const byte BurnTime = 40;
            public const byte CustomActionGroups = 41;
            public const byte TempLimit = 42;
            public const byte AdvancedActionGroups = 56; // Appears to be unused in KerbalSimpitRevamped
            public const byte AdvancedCustomActionGroups = 57;  // Appears to be unused in KerbalSimpitRevamped
            #endregion

            #region External Environment
            public const byte TargetInfo = 25;
            public const byte SoIName = 26;
            public const byte SceneChange = 3;
            public const byte FlightStatus = 43;
            public const byte AtmoCondition = 44;
            public const byte VesselName = 46;
            public const byte VesselChange = 51;
            #endregion
        }

        public static class Incoming
        {
            public const byte CustomActionGroupEnable = 10;
            public const byte CustomActionGroupDisable = 11;
            public const byte CustomActionGroupToggle = 12;
            public const byte ActionGroupActivate = 13;
            public const byte ActionGroupDeactivate = 14;
            public const byte ActionGroupToggle = 15;
            public const byte SetSingleActionGroup = 58;
            public const byte SetSingleCAG = 59;
            public const byte Rotation = 16;
            public const byte Translation = 17;
            public const byte WheelControl = 18;
            public const byte Throttle = 19;
            public const byte AutopilotMode = 20;
            public const byte CameraMode = 21;
            public const byte CameraRotation = 22;
            public const byte CameraTranslation = 23;
            public const byte WarpChange = 24;
            public const byte TimewarpTo = 30;
            public const byte KeyboardEmulator = 26;
            public const byte VesselCustomAxis = 27;
            public const byte NavballMode = 28;
        }
    }
}
