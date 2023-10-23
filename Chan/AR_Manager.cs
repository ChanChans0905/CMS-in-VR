using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AR_Manager : MonoBehaviour
{
    [SerializeField] DemoCarController DC;

    public GameObject AR_Signal_CMS_5_L, AR_Signal_CMS_5_R, AR_Signal_CMS_6_L, AR_Signal_CMS_6_R, AR_Signal_CMS_7_L, AR_Signal_CMS_7_R;

    public int TurnOn_AR_Signal_L, TurnOn_AR_Signal_R;

    void FixedUpdate()
    {
        if (DC.Activate_AR)
        {
            if (TurnOn_AR_Signal_L == 1)
                ManageLeft(true);
            else if (TurnOn_AR_Signal_L == 2)
                ManageLeft(false);

            if (TurnOn_AR_Signal_R == 1)
                ManageRight(true);
            else if (TurnOn_AR_Signal_R == 2)
                ManageRight(false);

            if (DC.RespawnTrigger)
                Respawn();
        }
    }

    void Respawn()
    {
        TurnOn_AR_Signal_L = 0;
        TurnOn_AR_Signal_R = 0;
        AR_Signal_CMS_5_L.SetActive(false);
        AR_Signal_CMS_6_L.SetActive(false);
        AR_Signal_CMS_7_L.SetActive(false);
        AR_Signal_CMS_5_R.SetActive(false);
        AR_Signal_CMS_6_R.SetActive(false);
        AR_Signal_CMS_7_R.SetActive(false);
    }

    void ManageLeft(bool value)
    {
        AR_Signal_CMS_5_L.SetActive(value);
        AR_Signal_CMS_6_L.SetActive(value);
        AR_Signal_CMS_7_L.SetActive(value);
        TurnOn_AR_Signal_L = 0;
    }
    void ManageRight(bool value)
    {
        AR_Signal_CMS_5_R.SetActive(value);
        AR_Signal_CMS_6_R.SetActive(value);
        AR_Signal_CMS_7_R.SetActive(value);
        TurnOn_AR_Signal_R = 0;
    }
}
