using KerbalSimpit.Core;
using KerbalSimpit.Core.KSP.Messages;
using KerbalSimpit.Core.Peers;
using KerbalSimpit.Core.Utilities;
using KerbalSimpit.Unity.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Mathf = UnityEngine.Mathf;

namespace KerbalSimpit.Unity.KSP1.Controllers
{
    [KSPAddon(KSPAddon.Startup.Flight, false)]
    public class CameraController : SimpitBehaviour,
        ISimpitMessageSubscriber<Camera.CameraMode>,
        ISimpitMessageSubscriber<Camera.CameraRotation>,
        ISimpitMessageSubscriber<Camera.CameraTranslation>
    {
        private float _flightCameraPitchMultiplier = 0.00002f;
        private float _flightCameraYawMultiplier = 0.00006f;
        private float _flightCameraZoomMultiplier = 0.001f;

        private bool _ivaCamFieldsLoaded = true;
        private FieldInfo _ivaPitchField;
        private FieldInfo _ivaYawField;
        private float _ivaCameraMultiplier = 0.003f;

        private Camera.CameraRotation _rotation;
        private Camera.CameraTranslation _translation;

        public override void Start()
        {
            base.Start();

            List<FieldInfo> fields = new List<FieldInfo>(typeof(InternalCamera).GetFields(
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance));
            fields = new List<FieldInfo>(fields.Where(f => f.FieldType.Equals(typeof(float))));
            _ivaPitchField = fields[3];
            _ivaYawField = fields[4];
            if (_ivaPitchField == null || _ivaYawField == null)
            {
                _ivaCamFieldsLoaded = false;
                this.logger.LogWarning("AFBW - Failed to acquire pitch/yaw fields in InternalCamera");
            }
        }

        public void Process(SimpitPeer peer, ISimpitMessage<Camera.CameraMode> message)
        {
            // Switch based on the CameraControlBits values
            switch (message.Data.Value)
            {
                // See if the control bit is a camera mode setting bit - need to add validity checking, to make sure the modes can be entered
                case Camera.CameraMode.ValueEnum.FlightMode:
                    CameraManager.Instance.SetCameraFlight();
                    break;
                case Camera.CameraMode.ValueEnum.IVAMode:
                    CameraManager.Instance.SetCameraIVA();
                    break;
                case Camera.CameraMode.ValueEnum.PlanetaryMode:
                    PlanetariumCamera.fetch.Activate();
                    break;
                // If it is not a camera mode, it must be a mode for the (hopefully) current camera
                default:

                    // Based on the current camera mode, change where the CameraControl value gets sent
                    switch (CameraManager.Instance.currentCameraMode)
                    {
                        // Flight Camera
                        case CameraManager.CameraMode.Flight:
                            this.logger.LogInformation("Camera can have it's mode changed");
                            this.logger.LogInformation("Control mode: {0}", message.Data.Value);
                            int currentFlightMode = FlightCamera.CamMode;
                            int maxEnum = Enum.GetNames(typeof(FlightCamera.Modes)).Length - 1;
                            this.logger.LogInformation("Max Enum Value: {0}", maxEnum);

                            // Switch based on the operation to perform on the flight camera
                            switch (message.Data.Value)
                            {
                                case Camera.CameraMode.ValueEnum.Auto:
                                    FlightCamera.SetMode(FlightCamera.Modes.AUTO);
                                    this.logger.LogVerbose("Set Camera Mode to: {0}", FlightCamera.Modes.AUTO);
                                    break;
                                case Camera.CameraMode.ValueEnum.Free:
                                    FlightCamera.SetMode(FlightCamera.Modes.FREE);
                                    this.logger.LogVerbose("Set Camera Mode to: {0}", FlightCamera.Modes.FREE);
                                    break;
                                case Camera.CameraMode.ValueEnum.Orbital:
                                    FlightCamera.SetMode(FlightCamera.Modes.ORBITAL);
                                    this.logger.LogVerbose("Set Camera Mode to: {0}", FlightCamera.Modes.ORBITAL);
                                    break;
                                case Camera.CameraMode.ValueEnum.Chase:
                                    FlightCamera.SetMode(FlightCamera.Modes.CHASE);
                                    this.logger.LogVerbose("Set Camera Mode to: {0}", FlightCamera.Modes.CHASE);
                                    break;
                                case Camera.CameraMode.ValueEnum.Locked:
                                    FlightCamera.SetMode(FlightCamera.Modes.LOCKED);
                                    this.logger.LogVerbose("Set Camera Mode to: {0}", FlightCamera.Modes.LOCKED);
                                    break;
                                case Camera.CameraMode.ValueEnum.NextCameraModeState:
                                    this.logger.LogVerbose("Set Camera Mode to: {0}", "Next");
                                    // If the current flight camera mode is at the end of the list, wrap around to the start
                                    if (currentFlightMode == maxEnum)
                                    {
                                        FlightCamera.SetMode((FlightCamera.Modes)0);
                                    }
                                    else
                                    {
                                        FlightCamera.SetMode((FlightCamera.Modes)currentFlightMode + 1);
                                    }
                                    break;
                                case Camera.CameraMode.ValueEnum.PreviousCameraModeState:
                                    this.logger.LogVerbose("Set Camera Mode to: {0}", "Previous");
                                    // If the current flight camera mode is at the start of the list, wrap around to the end
                                    if (currentFlightMode == 0)
                                    {
                                        FlightCamera.SetMode((FlightCamera.Modes)maxEnum);
                                    }
                                    else
                                    {
                                        // Hack to get around the issue of orbital mode not being available below a certain altitude around SOIs.
                                        FlightCamera.Modes autoMode = FlightCamera.GetAutoModeForVessel(FlightGlobals.ActiveVessel);
                                        // If the Flight Camera mode tries to go backwards to orbital mode, and orbital mode is not available, set the camera mode back two places.
                                        if (((FlightCamera.Modes)(FlightCamera.CamMode - 1)) == FlightCamera.Modes.ORBITAL && autoMode != FlightCamera.Modes.ORBITAL)
                                        {
                                            FlightCamera.SetMode((FlightCamera.Modes)(currentFlightMode - 2));
                                        }
                                        // Otherwise just change the mode back by one
                                        else
                                        {
                                            FlightCamera.SetMode((FlightCamera.Modes)currentFlightMode - 1);
                                        }
                                    }
                                    break;
                                default:
                                    this.logger.LogWarning("Set Camera Mode to: {0}", "No flight camera state to match the control bits:(");
                                    break;
                            }
                            break; /* End Flight Camera Mode */
                        case CameraManager.CameraMode.IVA:
                            break;

                        default:
                            this.logger.LogWarning("Simpit control for the camera mode: {0} is unsupported", CameraManager.Instance.currentCameraMode);
                            break;
                    } /* End of current camera mode switch */
                    break;
            } /* End of control mode switch */
        }

        public void Process(SimpitPeer peer, ISimpitMessage<Camera.CameraRotation> message)
        {
            // Bit fields:
            // pitch = 1
            // roll = 2
            // yaw = 4
            switch (CameraManager.Instance.currentCameraMode)
            {

                case CameraManager.CameraMode.Flight:
                    FlightCamera flightCamera = FlightCamera.fetch;
                    if ((message.Data.Mask & (byte)1) > 0)
                    {
                        _rotation.Pitch = message.Data.Pitch;
                        // Debug.Log("Rotation Message Seen");
                        float newPitch = flightCamera.camPitch + (_rotation.Pitch * _flightCameraPitchMultiplier);
                        if (newPitch > flightCamera.maxPitch)
                        {
                            flightCamera.camPitch = flightCamera.maxPitch;
                        }
                        else if (newPitch < flightCamera.minPitch)
                        {
                            flightCamera.camPitch = flightCamera.minPitch;
                        }
                        else
                        {
                            flightCamera.camPitch = newPitch;
                        }

                    }
                    if ((message.Data.Mask & (byte)2) > 0)
                    {
                        _rotation.Roll = message.Data.Roll;
                    }
                    if ((message.Data.Mask & (byte)4) > 0)
                    {
                        _rotation.Yaw = message.Data.Yaw;
                        // Debug.Log("Yaw Message Seen");
                        float newHdg = flightCamera.camHdg + (_rotation.Yaw * _flightCameraYawMultiplier);
                        flightCamera.camHdg = newHdg;
                    }
                    if ((message.Data.Mask & (byte)8) > 0)
                    {
                        _rotation.Zoom = message.Data.Zoom;
                        float newZoom = flightCamera.Distance + (_rotation.Zoom * _flightCameraZoomMultiplier);
                        if (newZoom > flightCamera.maxDistance)
                        {
                            newZoom = flightCamera.maxDistance;
                        }
                        else if (newZoom < flightCamera.minDistance)
                        {
                            newZoom = flightCamera.minDistance;
                        }
                        flightCamera.SetDistance(newZoom);
                    }
                    break;

                case CameraManager.CameraMode.IVA:
                case CameraManager.CameraMode.Internal:
                    Kerbal ivaKerbal = CameraManager.Instance.IVACameraActiveKerbal;

                    if (ivaKerbal == null)
                    {
                        this.logger.LogWarning("Kerbal is null");
                    }

                    InternalCamera ivaCamera = InternalCamera.Instance;
                    ivaCamera.mouseLocked = false;

                    if (ivaCamera == null)
                    {
                        this.logger.LogWarning("IVA Camera is null");
                    }
                    else
                    {
                        float newPitch = (float)_ivaPitchField.GetValue(ivaCamera);
                        float newYaw = (float)_ivaYawField.GetValue(ivaCamera);

                        if ((message.Data.Mask & (byte)1) > 0)
                        {
                            _rotation.Pitch = message.Data.Pitch;
                            //Debug.Log("IVA Rotation Message Seen");
                            newPitch += (_rotation.Pitch * _ivaCameraMultiplier);

                            if (newPitch > ivaCamera.maxPitch)
                            {
                                newPitch = ivaCamera.maxPitch;
                            }
                            else if (newPitch < ivaCamera.minPitch)
                            {
                                newPitch = ivaCamera.minPitch;
                            }
                        }
                        if ((message.Data.Mask & (byte)2) > 0)
                        {
                            _rotation.Roll = message.Data.Roll;
                        }
                        if ((message.Data.Mask & (byte)4) > 0)
                        {
                            _rotation.Yaw = message.Data.Yaw;
                            //Debug.Log("IVA Yaw Message Seen");
                            newYaw += (_rotation.Yaw * _ivaCameraMultiplier);
                            if (newYaw > 120f)
                            {
                                newYaw = 120f;
                            }
                            else if (newYaw < -120f)
                            {
                                newYaw = -120f;
                            }
                        }
                        //Debug.Log("Before set angle");
                        if (_ivaCamFieldsLoaded)
                        {
                            _ivaPitchField.SetValue(ivaCamera, newPitch);
                            _ivaYawField.SetValue(ivaCamera, newYaw);
                            // Debug.Log("Camera vector: " + ivaCamera.transform.localEulerAngles.ToString());
                            FlightCamera.fetch.transform.rotation = InternalSpace.InternalToWorld(InternalCamera.Instance.transform.rotation);
                        }
                    }
                    break;

                case CameraManager.CameraMode.Map:
                    //Debug.Log("Map Cam");
                    PlanetariumCamera planetariumCamera = PlanetariumCamera.fetch;
                    if ((message.Data.Mask & (byte)1) > 0)
                    {
                        _rotation.Pitch = message.Data.Pitch;
                        float newPitch = planetariumCamera.camPitch + (_rotation.Pitch * _flightCameraPitchMultiplier);

                        newPitch = Mathf.Clamp(newPitch, planetariumCamera.minPitch, planetariumCamera.maxPitch);

                        planetariumCamera.camPitch = newPitch;

                    }
                    if ((message.Data.Mask & (byte)2) > 0)
                    {
                        _rotation.Roll = message.Data.Roll;
                    }
                    if ((message.Data.Mask & (byte)4) > 0)
                    {
                        _rotation.Yaw = message.Data.Yaw;
                        float newHdg = planetariumCamera.camHdg + (_rotation.Yaw * _flightCameraYawMultiplier);
                        planetariumCamera.camHdg = newHdg;
                    }
                    if ((message.Data.Mask & (byte)8) > 0)
                    {
                        _rotation.Zoom = message.Data.Zoom;
                        float newZoom = planetariumCamera.Distance + (_rotation.Zoom * _flightCameraZoomMultiplier);

                        newZoom = Mathf.Clamp(newZoom, planetariumCamera.minDistance, planetariumCamera.maxDistance);

                        planetariumCamera.SetDistance(newZoom);
                    }
                    break;
                default:
                    this.logger.LogWarning("Kerbal Simpit does not support this camera mode: {0}", CameraManager.Instance.currentCameraMode);
                    break;
            }
        }

        public void Process(SimpitPeer peer, ISimpitMessage<Camera.CameraTranslation> message)
        {
            // TODO: Investigate - This appears to be doing nothing?

            // Bit fields:
            // X = 1
            // Y = 2
            // Z = 4
            if ((message.Data.Mask & (byte)1) > 0)
            {
                _translation.X = message.Data.X;
            }
            if ((message.Data.Mask & (byte)2) > 0)
            {
                _translation.Y = message.Data.Y;
            }
            if ((message.Data.Mask & (byte)4) > 0)
            {
                _translation.Z = message.Data.Z;
            }
        }
    }
}
