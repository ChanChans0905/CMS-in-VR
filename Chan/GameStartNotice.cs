using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.UI;

public class GameStartNotice : MonoBehaviour
{
    [SerializeField] DC_Collidor DC_C;
    [SerializeField] TrialManager TM;
    [SerializeField] DemoCarController DC;

    public GameObject WelcomeNotice, KeepLaneNotice, ProcessNotice, TrialStartNotice, SampleSelectionNotice;

    public Text SN;
    int Num;
    int Next;
    bool GameStart, SampleSelection;
    float ThresholdTimer;

    private void Start()
    {
        GameStart = true;
        SampleSelection = true;
    }

    void Update()
    {
        if (GameStart)
        {
            if (LogitechGSDK.LogiUpdate() && LogitechGSDK.LogiIsConnected(0))
            {
                LogitechGSDK.DIJOYSTATE2ENGINES rec;
                rec = LogitechGSDK.LogiGetStateUnity(0);

                ThresholdTimer += Time.deltaTime;

                if (SampleSelection)
                {
                    if (ThresholdTimer > 0.3f)
                    {
                        if (rec.rgbButtons[1] == 128)
                            if (Num != 0)
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
                            Num = Num + 10;
                            ThresholdTimer = 0;
                        }

                        if (rec.rgbButtons[0] == 128)
                            if (Num - 10 >= 0)
                            {
                                Num = Num - 10;
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
                    }
                }
                else
                {
                    if (ThresholdTimer > 0.5f)
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
                            TrialStartNotice.SetActive(true);
                            gameObject.SetActive(false);
                            break;
                    }
                }
            }
        }
    }
}
