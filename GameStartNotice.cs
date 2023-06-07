using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStartNotice : MonoBehaviour
{
    [SerializeField] FadeInOut FadeInOut;
    public float GameStartNoticeTimeCount;

    void Start()
    {
        FadeInOut.FadingEvent = true;
        Debug.Log("������ �����Ϸ��� ���� ������ ��⼼��");
    }

    void Update()
    {
        if (LogitechGSDK.LogiUpdate() && LogitechGSDK.LogiIsConnected(0))
        {
            LogitechGSDK.DIJOYSTATE2ENGINES rec;
            rec = LogitechGSDK.LogiGetStateUnity(0);
            if (rec.rgbButtons[5] == 128)
            {
                FadeInOut.FadingEvent=false;
                gameObject.SetActive(false);
            } 

        }
    }
}
