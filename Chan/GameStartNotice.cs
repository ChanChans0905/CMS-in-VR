using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStartNotice : MonoBehaviour
{
    [SerializeField] DC_Collidor DC_C;
    [SerializeField] TrialManager TM;

    public GameObject WelcomeNotice, KeepLaneNotice;

    int Threshold;
    int Next;

    void Start()
    {
        DC_C.FadingEvent = true;
    }

    void Update()
    {
        if (LogitechGSDK.LogiUpdate() && LogitechGSDK.LogiIsConnected(0))
        {
            LogitechGSDK.DIJOYSTATE2ENGINES rec;
            rec = LogitechGSDK.LogiGetStateUnity(0);

            if (rec.rgbButtons[5] == 128)
            {
                Threshold++;
                if (Threshold > 20)
                {
                    Threshold = 0;
                    Next++;
                }

                switch (Next)
                {
                    case 1:
                        WelcomeNotice.SetActive(false);
                        KeepLaneNotice.SetActive(true);
                        break;

                    case 2:
                        DC_C.FadingEvent = false;
                        TM.TrialTask = true;
                        TM.TurnOnTrialStartNotice = true;
                        gameObject.SetActive(false);
                        break;
                }
            }
        }
    }
}
