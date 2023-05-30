using PathCreation;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class SaveTrigger : MonoBehaviour
{
    [Serializefield] Questionnaire Questionnaire;
    LogitechGSDK.LogiControllerPropertiesdata properties;

    void Update()
    {
        LogitechGSDK.DIJOYSTATE2ENGINES rec;
        rec = LogitechGSDK.LogiGetStateUnity(0);

        if (rec.rgbButtons[5] == 128)
        {
            Questionnaire.SaveTrigger = true;
        }
    }
}