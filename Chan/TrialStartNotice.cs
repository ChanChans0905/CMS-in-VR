using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrialStartNotice : MonoBehaviour
{
    [SerializeField] DemoCarController DC;
    [SerializeField] DC_Collidor DC_C;
    [SerializeField] TrialManager TM;
    float Timer;

    void Update()
    {
        if (LogitechGSDK.LogiUpdate() && LogitechGSDK.LogiIsConnected(0))
        {
            LogitechGSDK.DIJOYSTATE2ENGINES rec;
            rec = LogitechGSDK.LogiGetStateUnity(0);

            Timer += Time.deltaTime;

            if (Timer > 3.5f)
                if (rec.rgbButtons[5] == 128)
                {
                    DC.ActivateCar(Timer, gameObject);
                    DC.DrivingPhase = true;
                }
        }
    }
}
