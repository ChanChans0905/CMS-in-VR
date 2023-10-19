using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskStartNotice : MonoBehaviour
{
    [SerializeField] DemoCarController DC;
    [SerializeField] DC_Collidor DC_C;
    [SerializeField] TrialManager TM;

    float ThresholdTImer;

    void Update()
    {


        if (LogitechGSDK.LogiUpdate() && LogitechGSDK.LogiIsConnected(0))
        {
            ThresholdTImer += Time.deltaTime;

            LogitechGSDK.DIJOYSTATE2ENGINES rec;
            rec = LogitechGSDK.LogiGetStateUnity(0);
            if (ThresholdTImer > 3)
            {
                if (rec.rgbButtons[5] == 128)
                {
                    DC_C.FadingEvent = false;
                    DC_C.Activate_Fade = true;
                    DC.RespawnTrigger = false;
                    DC.waitTimer = 0;
                    ThresholdTImer = 0;
                    if(!TM.TrialTask)
                        DC.MainTask = true;
                    gameObject.SetActive(false);
                }
            }
        }
    }
}
