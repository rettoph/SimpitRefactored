namespace KerbalSimpit.Unity.KSP1.Helpers
{
    public static class TelemetryHelper
    {
        /// <summary>
        /// Convert a direction given in world space v into a heading and a pitch, relative to the vessel passed as a paramater
        /// </summary>
        /// <param name="activeVessel"></param>
        /// <param name="v"></param>
        /// <param name="heading"></param>
        /// <param name="pitch"></param>
        public static void WorldVecToNavHeading(Vessel activeVessel, Vector3d v, out float heading, out float pitch)
        {
            Vector3d CoM, north, up, east;
            CoM = activeVessel.CoM;
            up = (CoM - activeVessel.mainBody.position).normalized;
            north = Vector3d.Exclude(up, (activeVessel.mainBody.position + activeVessel.mainBody.transform.up * (float)activeVessel.mainBody.Radius) - CoM).normalized;
            east = Vector3d.Cross(up, north);

            // Code from KSPIO to do angle conversions : https://github.com/zitron-git/KSPSerialIO/blob/062d97e892077ea14737f5e79268c0c4d067f5b6/KSPSerialIO/KSPIO.cs#L1301-L1313
            pitch = (float)-((Vector3d.Angle(up, v)) - 90.0f);
            Vector3d progradeFlat = Vector3d.Exclude(up, v);
            float NAngle = (float)Vector3d.Angle(north, progradeFlat);
            float EAngle = (float)Vector3d.Angle(east, progradeFlat);
            if (EAngle < 90)
                heading = NAngle;
            else
                heading = -NAngle + 360;
        }
    }
}
