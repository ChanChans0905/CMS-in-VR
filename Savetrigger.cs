using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class SaveTrigger : MonoBehaviour
{
    [SerializeField] Questionnaire Questionnaire;
    [SerializeField] FinalQuestionnaire FinalQuestionnaire;
    LogitechGSDK.LogiControllerPropertiesData properties;
    public bool SaveButton = false;

    void Update()
    {
        if(gameObject.active)
        {
            SaveButton = true;
        }
        LogitechGSDK.DIJOYSTATE2ENGINES rec;
        rec = LogitechGSDK.LogiGetStateUnity(0);

        if (rec.rgbButtons[5] == 128)
        {
            Questionnaire.SaveTrigger = true;
        }
        else if(Questionnaire.FinalQuestionnaireBool == true && rec.rgbButtons[5] == 128)
        {
            FinalQuestionnaire.GameEndBool = true;
        }
    }
}