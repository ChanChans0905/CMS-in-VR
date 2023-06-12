using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARSignal : MonoBehaviour
{
    [SerializeField] DemoCarController DriverCar;
    public bool ARRbool, ARLbool;

    void Update()
    {
        if (DriverCar.FCLbool) { ARLbool = true;}
        else if(DriverCar.FCLbool == false) { ARLbool = false;}

        if (DriverCar.FCRbool) { ARRbool = true;}
        else if(DriverCar.FCRbool == false) { ARRbool = false;}
    }
}
