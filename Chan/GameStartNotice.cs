using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.UI;
using VolvoCars.Data;

public class GameStartNotice : MonoBehaviour
{
    [SerializeField] DC_Collidor DC_C;
    [SerializeField] TrialManager TM;
    [SerializeField] DemoCarController DC;

    public GameObject WelcomeNotice, KeepLaneNotice, ProcessNotice, TrialStartNotice, SampleSelectionNotice, ControlTrialNotice;
    public GameObject VolvoCar, DC_Collidor_Object;
    public GameObject TaskStartPoint1, TaskStartPoint2;

    public Text SN;
    int Num;
    int Next;
    bool SampleSelection;
    float ThresholdTimer;
    float ControlTrialTimer;

    private void Start()
    {
        SampleSelection = true;
    }

    void Update()
    {
        if (LogitechGSDK.LogiUpdate() && LogitechGSDK.LogiIsConnected(0))
        {
            LogitechGSDK.DIJOYSTATE2ENGINES rec;
            rec = LogitechGSDK.LogiGetStateUnity(0);

            if(ControlTrialTimer == 0)
                DC.FixCarPos();

            ThresholdTimer += Time.deltaTime;

            if (SampleSelection)
            {
                if (ThresholdTimer > 0.3f)
                {
                    if (rec.rgbButtons[1] == 128 && Num != 0)
                    {
                        Num--;
                        ThresholdTimer = 0;
                    }

                    if (rec.rgbButtons[2] == 128)
                    {
                        Num++;
                        ThresholdTimer = 0;
                    }

                    if (rec.rgbButtons[3] == 128)
                    {
                        Num += 10;
                        ThresholdTimer = 0;
                    }

                    if (rec.rgbButtons[0] == 128 && Num - 10 >= 0)
                    {
                        Num -= 10;
                        ThresholdTimer = 0;
                    }
                }
                SN.text = Num.ToString();

                if (rec.rgbButtons[23] == 128)
                {
                    DC.SampleNumber = Num;
                    SampleSelectionNotice.SetActive(false);
                    WelcomeNotice.SetActive(true);
                    SampleSelection = false;
                    ThresholdTimer = 0;
                }
            }
            else
            {
                if (ThresholdTimer > 1f)
                {
                    if (rec.rgbButtons[4] == 128)
                    {
                        Next++;
                        ThresholdTimer = 0;
                    }
                }

                switch (Next)
                {
                    case 1:
                        WelcomeNotice.SetActive(false);
                        ProcessNotice.SetActive(true);
                        break;

                    case 2:
                        ProcessNotice.SetActive(false);
                        KeepLaneNotice.SetActive(true);
                        break;

                    case 3:
                        KeepLaneNotice.SetActive(false);
                        ControlTrialNotice.SetActive(true);
                        break;
                    case 4:
                        ControlTrialNotice.SetActive(false);
                        if(ControlTrialTimer == 0)
                        {
                            DC_Collidor_Object.SetActive(false);
                            DC.RespawnTrigger = false;
                            DC.waitTimer = 0;
                            DC_C.FadingEvent = false;
                            DC_C.Activate_Fade = true;
                        }
                        ControlTrialTimer += Time.deltaTime;

                        if (ControlTrialTimer >= 180)
                            Next = 5;
                        break;
                    case 5:
                        DC.FixCarPos();
                        DC_Collidor_Object.SetActive(true);
                        TaskStartPoint1.SetActive(true);
                        TaskStartPoint2.SetActive(true);
                        DC.RespawnTrigger = true;
                        TrialStartNotice.SetActive(true);
                        DC.SelectArray = true;
                        Destroy(gameObject);
                        break;
                }
            }
        }
    }
}
