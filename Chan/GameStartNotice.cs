using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class GameStartNotice : MonoBehaviour
{
    [SerializeField] DC_Collidor DC_C;
    [SerializeField] TrialManager TM;

    public GameObject WelcomeNotice, KeepLaneNotice, ProcessNotice, TrialStartNotice;

    int Next;
    bool GameStart;
    float ThresholdTimer;

    private void Start()
    {
        GameStart = true;
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
