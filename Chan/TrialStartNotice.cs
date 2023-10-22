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

            DC.FixCarPos();

            if (Timer > 3.5f)
                if (rec.rgbButtons[5] == 128)
                {
                    DC.RespawnTrigger = false;
                    DC.waitTimer = 0;
                    DC_C.FadingEvent = false;
                    DC_C.Activate_Fade = true;
                    Timer = 0;
                    gameObject.SetActive(false);
                    DC.DrivingPhase = true;
                }
        }
    }
}
