using KerbalSimpit.Core.KSP.Constants;
using KerbalSimpit.Core.KSP.Messages;

namespace KerbalSimpit.Core.KSP.Extensions
{
    public static class SimpitExtensions
    {
        public static Simpit RegisterKerbal(this Simpit simpit)
        {
            return simpit.RegisterKerbalMessages();
        }

        private static Simpit RegisterKerbalMessages(this Simpit simpit)
        {
            #region Outgoing Messaages
            // Propulsion Resources
            simpit.Messages.RegisterOutogingType<Resource.LiquidFuel>(MessageTypeIds.Outgoing.LiquidFuel);
            simpit.Messages.RegisterOutogingType<Resource.LiquidFuelStage>(MessageTypeIds.Outgoing.LiquidFuelStage);
            // simpit.Messages.RegisterOutogingType<Resource.Methane>(MessageTypeIds.Outgoing.Methane);
            // simpit.Messages.RegisterOutogingType<Resource.MethaneStage>(MessageTypeIds.Outgoing.MethaneStage);
            simpit.Messages.RegisterOutogingType<Resource.Oxidizer>(MessageTypeIds.Outgoing.Oxidizer);
            simpit.Messages.RegisterOutogingType<Resource.OxidizerStage>(MessageTypeIds.Outgoing.OxidizerStage);
            simpit.Messages.RegisterOutogingType<Resource.SolidFuel>(MessageTypeIds.Outgoing.SolidFuel);
            simpit.Messages.RegisterOutogingType<Resource.SolidFuelStage>(MessageTypeIds.Outgoing.SolidFuelStage);
            simpit.Messages.RegisterOutogingType<Resource.XenonGas>(MessageTypeIds.Outgoing.XenonGas);
            simpit.Messages.RegisterOutogingType<Resource.XenonGasStage>(MessageTypeIds.Outgoing.XenonGasStage);
            simpit.Messages.RegisterOutogingType<Resource.MonoPropellant>(MessageTypeIds.Outgoing.MonoPropellant);
            simpit.Messages.RegisterOutogingType<Resource.EvaPropellant>(MessageTypeIds.Outgoing.EvaPropellant);

            // Vessel Resources
            simpit.Messages.RegisterOutogingType<Resource.ElectricCharge>(MessageTypeIds.Outgoing.ElectricCharge);
            simpit.Messages.RegisterOutogingType<Resource.Ore>(MessageTypeIds.Outgoing.Ore);
            simpit.Messages.RegisterOutogingType<Resource.Ablator>(MessageTypeIds.Outgoing.Ablator);
            simpit.Messages.RegisterOutogingType<Resource.AblatorStage>(MessageTypeIds.Outgoing.AblatorStage);
            simpit.Messages.RegisterOutogingType<TACLS.Resource>(MessageTypeIds.Outgoing.TACLSResource);
            simpit.Messages.RegisterOutogingType<TACLS.Waste>(MessageTypeIds.Outgoing.TACLSWaste);
            simpit.Messages.RegisterOutogingType<CustomResource.One>(MessageTypeIds.Outgoing.CustomResource1);
            simpit.Messages.RegisterOutogingType<CustomResource.Two>(MessageTypeIds.Outgoing.CustomResource2);

            // Vessel Movement/Postion
            simpit.Messages.RegisterOutogingType<Vessel.Outgoing.Altitude>(MessageTypeIds.Outgoing.Altitude);
            simpit.Messages.RegisterOutogingType<Vessel.Outgoing.Velocity>(MessageTypeIds.Outgoing.Velocity);
            simpit.Messages.RegisterOutogingType<Vessel.Outgoing.Airspeed>(MessageTypeIds.Outgoing.Airspeed);
            simpit.Messages.RegisterOutogingType<Vessel.Outgoing.Apsides>(MessageTypeIds.Outgoing.Apsides);
            simpit.Messages.RegisterOutogingType<Vessel.Outgoing.ApsidesTime>(MessageTypeIds.Outgoing.ApsidesTime);
            simpit.Messages.RegisterOutogingType<Vessel.Outgoing.Maneuver>(MessageTypeIds.Outgoing.ManeuverData);
            simpit.Messages.RegisterOutogingType<Vessel.Outgoing.SASInfo>(MessageTypeIds.Outgoing.SASInfo);
            simpit.Messages.RegisterOutogingType<Vessel.Outgoing.OrbitInfo>(MessageTypeIds.Outgoing.OrbitInfo);
            simpit.Messages.RegisterOutogingType<Vessel.Outgoing.Rotation>(MessageTypeIds.Outgoing.Rotation);

            // Vessel commands
            simpit.Messages.RegisterOutogingType<Vessel.Outgoing.RotationCmd>(MessageTypeIds.Outgoing.RotationCmd);
            simpit.Messages.RegisterOutogingType<Vessel.Outgoing.TranslationCmd>(MessageTypeIds.Outgoing.TranslationCmd);
            simpit.Messages.RegisterOutogingType<Vessel.Outgoing.WheelCmd>(MessageTypeIds.Outgoing.WheelCmd);
            simpit.Messages.RegisterOutogingType<Vessel.Outgoing.ThrottleCmd>(MessageTypeIds.Outgoing.ThrottleCmd);

            // Vessel Details
            simpit.Messages.RegisterOutogingType<Vessel.Outgoing.ActionGroups>(MessageTypeIds.Outgoing.ActionGroups);
            simpit.Messages.RegisterOutogingType<Vessel.Outgoing.DeltaV>(MessageTypeIds.Outgoing.DeltaV);
            simpit.Messages.RegisterOutogingType<Vessel.Outgoing.DeltaVEnv>(MessageTypeIds.Outgoing.DeltaVEnv);
            simpit.Messages.RegisterOutogingType<Vessel.Outgoing.BurnTime>(MessageTypeIds.Outgoing.BurnTime);
            simpit.Messages.RegisterOutogingType<Vessel.Outgoing.CustomActionGroups>(MessageTypeIds.Outgoing.CustomActionGroups, Vessel.Outgoing.CustomActionGroups.Serialize);
            simpit.Messages.RegisterOutogingType<Vessel.Outgoing.TempLimit>(MessageTypeIds.Outgoing.TempLimit);

            // External Environment
            simpit.Messages.RegisterOutogingType<Environment.TargetInfo>(MessageTypeIds.Outgoing.TargetInfo);
            simpit.Messages.RegisterOutogingType<Environment.SoIName>(MessageTypeIds.Outgoing.SoIName, Environment.SoIName.Serialize);
            simpit.Messages.RegisterOutogingType<Environment.SceneChange>(MessageTypeIds.Outgoing.SceneChange);
            simpit.Messages.RegisterOutogingType<Environment.FlightStatus>(MessageTypeIds.Outgoing.FlightStatus);
            simpit.Messages.RegisterOutogingType<Environment.AtmoCondition>(MessageTypeIds.Outgoing.AtmoCondition);
            simpit.Messages.RegisterOutogingType<Environment.VesselName>(MessageTypeIds.Outgoing.VesselName, Environment.VesselName.Serialize);
            simpit.Messages.RegisterOutogingType<Environment.VesselChange>(MessageTypeIds.Outgoing.VesselChange);
            #endregion

            #region Incoming Messages
            simpit.Messages.RegisterIncomingType<Vessel.Incoming.CustomActionGroupEnable>(MessageTypeIds.Incoming.CustomActionGroupEnable, Vessel.Incoming.CustomActionGroupEnable.Deserialize);
            simpit.Messages.RegisterIncomingType<Vessel.Incoming.CustomActionGroupDisable>(MessageTypeIds.Incoming.CustomActionGroupDisable, Vessel.Incoming.CustomActionGroupDisable.Deserialize);
            simpit.Messages.RegisterIncomingType<Vessel.Incoming.CustomActionGroupToggle>(MessageTypeIds.Incoming.CustomActionGroupToggle, Vessel.Incoming.CustomActionGroupToggle.Deserialize);
            simpit.Messages.RegisterIncomingType<Vessel.Incoming.ActionGroupActivate>(MessageTypeIds.Incoming.ActionGroupActivate);
            simpit.Messages.RegisterIncomingType<Vessel.Incoming.ActionGroupDeactivate>(MessageTypeIds.Incoming.ActionGroupDeactivate);
            simpit.Messages.RegisterIncomingType<Vessel.Incoming.ActionGroupToggle>(MessageTypeIds.Incoming.ActionGroupToggle);
            simpit.Messages.RegisterIncomingType<Vessel.Incoming.Rotation>(MessageTypeIds.Incoming.Rotation);
            simpit.Messages.RegisterIncomingType<Vessel.Incoming.Translation>(MessageTypeIds.Incoming.Translation);
            simpit.Messages.RegisterIncomingType<Vessel.Incoming.WheelControl>(MessageTypeIds.Incoming.WheelControl);
            simpit.Messages.RegisterIncomingType<Vessel.Incoming.Throttle>(MessageTypeIds.Incoming.Throttle);
            simpit.Messages.RegisterIncomingType<Vessel.Incoming.AutopilotMode>(MessageTypeIds.Incoming.AutopilotMode);

            simpit.Messages.RegisterIncomingType<Camera.CameraMode>(MessageTypeIds.Incoming.CameraMode);
            simpit.Messages.RegisterIncomingType<Camera.CameraRotation>(MessageTypeIds.Incoming.CameraRotation);
            simpit.Messages.RegisterIncomingType<Camera.CameraTranslation>(MessageTypeIds.Incoming.CameraTranslation);

            simpit.Messages.RegisterIncomingType<Warp.WarpChange>(MessageTypeIds.Incoming.WarpChange);
            simpit.Messages.RegisterIncomingType<Warp.TimewarpTo>(MessageTypeIds.Incoming.TimewarpTo);

            simpit.Messages.RegisterIncomingType<NavBall.NavballMode>(MessageTypeIds.Incoming.NavballMode);

            simpit.Messages.RegisterIncomingType<KeyboardEmulator>(MessageTypeIds.Incoming.KeyboardEmulator);
            #endregion

            return simpit;
        }
    }
}
