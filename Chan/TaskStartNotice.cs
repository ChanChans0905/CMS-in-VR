using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskStartNotice : MonoBehaviour
{
    [SerializeField] DemoCarController DC;
    [SerializeField] DC_Collidor DC_C;

    float ThresholdTImer;

    void Update()
    {
        if (LogitechGSDK.LogiUpdate() && LogitechGSDK.LogiIsConnected(0))
        {
            LogitechGSDK.DIJOYSTATE2ENGINES rec;
            rec = LogitechGSDK.LogiGetStateUnity(0);

            ThresholdTImer += Time.deltaTime;

            if (ThresholdTImer > 2)
            {
                if (rec.rgbButtons[5] == 128)
                {
                    DC_C.FadingEvent = false;
                    DC_C.Activate_Fade = true;
                    DC.respawnTrigger = false;
                    DC.waitTimer = 0;
                    ThresholdTImer = 0;
                    DC.MainTask = true;
                    gameObject.SetActive(false);
                }
            }
        }
    }
}
