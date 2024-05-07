using KerbalSimpit.Core.KSP.Enums;
using System;
using System.Collections.Generic;

namespace KerbalSimpit.Core.KSP.Utilities
{
    public static class AutoPilotHelper
    {
        public static AutoPilotModeFlags ModeEnumToFlag(AutoPilotModeEnum mode)
        {
            switch (mode)
            {
                case AutoPilotModeEnum.StabilityAssist:
                    return AutoPilotModeFlags.StabilityAssist;
                case AutoPilotModeEnum.Prograde:
                    return AutoPilotModeFlags.Prograde;
                case AutoPilotModeEnum.Retrograde:
                    return AutoPilotModeFlags.Retrograde;
                case AutoPilotModeEnum.Normal:
                    return AutoPilotModeFlags.Normal;
                case AutoPilotModeEnum.Antinormal:
                    return AutoPilotModeFlags.Antinormal;
                case AutoPilotModeEnum.RadialIn:
                    return AutoPilotModeFlags.RadialIn;
                case AutoPilotModeEnum.RadialOut:
                    return AutoPilotModeFlags.RadialOut;
                case AutoPilotModeEnum.Target:
                    return AutoPilotModeFlags.Target;
                case AutoPilotModeEnum.AntiTarget:
                    return AutoPilotModeFlags.AntiTarget;
                case AutoPilotModeEnum.Maneuver:
                    return AutoPilotModeFlags.Maneuver;
            }

            throw new InvalidOperationException();
        }

        public static AutoPilotModeFlags ModesEnumsToFlags(params AutoPilotModeEnum[] modes)
        {
            AutoPilotModeFlags flags = AutoPilotModeFlags.None;

            foreach (AutoPilotModeEnum mode in modes)
            {
                flags |= AutoPilotHelper.ModeEnumToFlag(mode);
            }

            return flags;
        }

        public static IEnumerable<AutoPilotModeEnum> ModeFlagsToEnums(AutoPilotModeFlags flags)
        {
            foreach (AutoPilotModeEnum mode in Enum.GetValues(typeof(AutoPilotModeEnum)))
            {
                if (flags.HasFlag(mode))
                {
                    yield return mode;
                }
            }
        }
    }
}
