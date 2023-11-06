using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlTrialNotice : MonoBehaviour
{
    [SerializeField] DemoCarController DC;
    [SerializeField] DC_Collidor DC_C;
    public GameObject TrialStartNotice;
    bool TrialStart;
    float Timer;

    void Update()
    {
        if (LogitechGSDK.LogiUpdate() && LogitechGSDK.LogiIsConnected(0))
        {
            LogitechGSDK.DIJOYSTATE2ENGINES rec;
            rec = LogitechGSDK.LogiGetStateUnity(0);

            if (rec.rgbButtons[5] == 128)
            {
                DC.RespawnTrigger = false;
                DC.waitTimer = 0;
                DC_C.FadingEvent = false;
                DC_C.Activate_Fade = true;
                TrialStart = true;
            }
                
            if (TrialStart)
            {
                Timer += Time.deltaTime;

                if (rec.rgbButtons[4] == 128 || Timer >= 180)
                {
                    // end trial + respawn

                    DC.ResetTrigger = true;
                    TrialStartNotice.SetActive(true);
                }
            }
        }
    }
}
