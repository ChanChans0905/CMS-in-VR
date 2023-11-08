using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Varjo.XR;

public class TaskStartNotice : MonoBehaviour
{
    [SerializeField] DemoCarController DC;
    [SerializeField] DC_Collidor DC_C;
    [SerializeField] TrialManager TM;
    public VarjoEyeTracking.GazeCalibrationMode gazeCalibrationMode = VarjoEyeTracking.GazeCalibrationMode.Fast;
    public bool Calibration;
    float Timer;

    void Update()
    {
        if (Calibration)
        {
            VarjoEyeTracking.RequestGazeCalibration(gazeCalibrationMode);
            Calibration = false;
        }

        if (LogitechGSDK.LogiUpdate() && LogitechGSDK.LogiIsConnected(0))
        {
            LogitechGSDK.DIJOYSTATE2ENGINES rec;
            rec = LogitechGSDK.LogiGetStateUnity(0);

            DC.FixCarPos();

            Timer += Time.deltaTime;

            if (Timer > 3.5f)
                if (rec.rgbButtons[5] == 128)
                {
                    DC.RespawnTrigger = false;
                    DC.waitTimer = 0;
                    DC_C.FadingEvent = false;
                    DC_C.Activate_Fade = true;
                    Timer = 0;
                    gameObject.SetActive(false);
                }
        }
    }
}
