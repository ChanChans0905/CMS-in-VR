using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AR_Collidor_L : MonoBehaviour
{
    [SerializeField] AR_Manager AR;

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "LC" || other.tag == "FC" || other.tag == "TC")
            AR.TurnOn_AR_Signal_L = 1;
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "LC" || other.tag == "FC" || other.tag == "TC")
            AR.TurnOn_AR_Signal_L = 2;
    }
}
