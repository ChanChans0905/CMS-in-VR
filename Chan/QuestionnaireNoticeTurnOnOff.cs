using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionnaireNoticeTurnOnOff : MonoBehaviour
{
    [SerializeField] Questionnaire Q;
    [SerializeField] DemoCarController DC;
    [SerializeField] FinalQuestionnaire FQ;
    public GameObject Next;
    public GameObject ParentObject;
    float Timer;

    void Update()
    {
        if (LogitechGSDK.LogiUpdate() && LogitechGSDK.LogiIsConnected(0))
        {
            LogitechGSDK.DIJOYSTATE2ENGINES rec;
            rec = LogitechGSDK.LogiGetStateUnity(0);

            Timer += Time.deltaTime;

            if(Timer > 3.5f)
            {
                if (rec.rgbButtons[4] == 128)
                {
                    if (DC.QuestionnaireCount < 7)
                    {
                        Q.QuestionnairePhase = true;
                        Q.FirstSlider = true;
                    }
                    if (DC.QuestionnaireCount == 7)
                    {
                        FQ.FinalQuestionnairePhase = true;
                        FQ.FirstSlider = true;
                    }

                    Timer = 0;
                    Next.SetActive(true);
                    ParentObject.SetActive(true);
                    gameObject.SetActive(false);
                }
            }
        }
    }
}
