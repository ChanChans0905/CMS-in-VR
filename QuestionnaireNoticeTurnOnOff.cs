using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionnaireNoticeTurnOnOff : MonoBehaviour
{
    public GameObject Next;

    void Update()
    {
        if (LogitechGSDK.LogiUpdate() && LogitechGSDK.LogiIsConnected(0))
        {
            LogitechGSDK.DIJOYSTATE2ENGINES rec;
            rec = LogitechGSDK.LogiGetStateUnity(0);

            if (rec.rgbButtons[4] == 128) { Next.SetActive(true); gameObject.SetActive(false); }
        }
    }
}
