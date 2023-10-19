using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskFailureNotice : MonoBehaviour
{
    [SerializeField] DemoCarController DC;
    [SerializeField] DC_Collidor DC_C;

    void Update()
    {
        if (LogitechGSDK.LogiUpdate() && LogitechGSDK.LogiIsConnected(0))
        {
            LogitechGSDK.DIJOYSTATE2ENGINES rec;
            rec = LogitechGSDK.LogiGetStateUnity(0);
            if (rec.rgbButtons[5] == 128)
            {
                DC_C.FadingEvent = false;
                DC.RespawnTrigger = false;
                DC.waitTimer = 0;
                gameObject.SetActive(false);
            }
        }
    }
}
