using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AR_Manager : MonoBehaviour
{
    [SerializeField] DemoCarController DC;

    public GameObject AR_Signal_CMS_5_L, AR_Signal_CMS_5_R, AR_Signal_CMS_6_L, AR_Signal_CMS_6_R, AR_Signal_CMS_7_L, AR_Signal_CMS_7_R;

    int TurnOn_AR_Signal_L, TurnOn_AR_Signal_R;

    void Update()
    {
        if(DC.Activate_AR)
        {
            if (TurnOn_AR_Signal_L == 1)
            {
                AR_Signal_CMS_5_L.SetActive(true);
                AR_Signal_CMS_6_L.SetActive(true);
                AR_Signal_CMS_7_L.SetActive(true);
                TurnOn_AR_Signal_L = 0;
            }
            else if (TurnOn_AR_Signal_L == 2)
            {
                AR_Signal_CMS_5_L.SetActive(false);
                AR_Signal_CMS_6_L.SetActive(false);
                AR_Signal_CMS_7_L.SetActive(false);
                TurnOn_AR_Signal_L = 0;
            }

            if (TurnOn_AR_Signal_R == 1)
            {
                AR_Signal_CMS_5_R.SetActive(true);
                AR_Signal_CMS_6_R.SetActive(true);
                AR_Signal_CMS_7_R.SetActive(true);
                TurnOn_AR_Signal_L = 0;
            }
            else if (TurnOn_AR_Signal_R == 2)
            {
                AR_Signal_CMS_5_R.SetActive(false);
                AR_Signal_CMS_6_R.SetActive(false);
                AR_Signal_CMS_7_R.SetActive(false);
                TurnOn_AR_Signal_R = 0;
            }

            if (DC.respawnTrigger)
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
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "AR_Signal_L")
            TurnOn_AR_Signal_L = 1;

        if(other.tag == "AR_Signal_R")
            TurnOn_AR_Signal_R = 1;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "AR_Signal_L")
            TurnOn_AR_Signal_L = 2;

        if (other.tag == "AR_Signal_R")
            TurnOn_AR_Signal_R = 2;
    }
}
