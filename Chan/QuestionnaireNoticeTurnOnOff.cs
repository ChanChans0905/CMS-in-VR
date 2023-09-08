using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionnaireNoticeTurnOnOff : MonoBehaviour
{
    [SerializeField] Questionnaire Q;
    public GameObject Next;
    public GameObject ParentObject;

    void Update()
    {
        if (LogitechGSDK.LogiUpdate() && LogitechGSDK.LogiIsConnected(0))
        {
            LogitechGSDK.DIJOYSTATE2ENGINES rec;
            rec = LogitechGSDK.LogiGetStateUnity(0);

            if (rec.rgbButtons[4] == 128)
            {
                Q.QuestionnairePhase = true;
                Next.SetActive(true); 
                ParentObject.SetActive(true); 
                gameObject.SetActive(false);
            }
        }
    }
}
