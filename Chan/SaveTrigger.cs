using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class SaveTrigger : MonoBehaviour
{
    [SerializeField] DemoCarController DriverCar;
    [SerializeField] Questionnaire Questionnaire;
    [SerializeField] FinalQuestionnaire FinalQuestionnaire;
    LogitechGSDK.LogiControllerPropertiesData properties;
    public bool SaveButton;

    void Update()
    {
        if (LogitechGSDK.LogiUpdate() && LogitechGSDK.LogiIsConnected(0))
        {
            LogitechGSDK.DIJOYSTATE2ENGINES rec;
            rec = LogitechGSDK.LogiGetStateUnity(0);

            if (rec.rgbButtons[5] == 128 && DriverCar.FinalQuestionnaireBool == false)
            {
                Questionnaire.SaveTrigger = true;
                gameObject.SetActive(false);
            }

            else if (rec.rgbButtons[5] == 128 && DriverCar.FinalQuestionnaireBool == true)
            {
                FinalQuestionnaire.SaveTrigger = true; 
                gameObject.SetActive(false);
            }
        }
    }
}