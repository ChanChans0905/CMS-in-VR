using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARSignalTurnOnOff : MonoBehaviour
{ 
    [SerializeField] ARSignal ARS;
    public GameObject ARL, ARR;

    void Update()
    {
        if (ARS.ARLbool) { ARL.SetActive(true); }
        else { ARL.SetActive(false); }

        if (ARS.ARRbool) { ARR.SetActive(true); }
        else { ARR.SetActive(false); }
    }
}
