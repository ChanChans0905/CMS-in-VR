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

            if (Timer < 3)
                Timer += Time.deltaTime;

            if (Timer > 2 && rec.rgbButtons[5] == 128)
            {
                DC_C.FadingEvent = false;
                DC_C.Activate_Fade = true;
                DC.RespawnTrigger = false;
                TM.TrialTask = true;
                DC.waitTimer = 0;
                gameObject.SetActive(false);
                Timer = 0;
            }
        }
    }
}
