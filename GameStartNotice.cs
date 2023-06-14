using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStartNotice : MonoBehaviour
{
    [SerializeField] DemoCarController DriverCar;
    [SerializeField] FadeInOut FadeInOut;

    void Start()
    {
        FadeInOut.FadingEvent = true;
    }

    void Update()
    {
        if (LogitechGSDK.LogiUpdate() && LogitechGSDK.LogiIsConnected(0))
        {
            LogitechGSDK.DIJOYSTATE2ENGINES rec;
            rec = LogitechGSDK.LogiGetStateUnity(0);
            if (rec.rgbButtons[5] == 128)
            {
                FadeInOut.FadingEvent=false;
                DriverCar.GameStartNoticeBool = true;
                gameObject.SetActive(false);
            } 

        }
    }
}
