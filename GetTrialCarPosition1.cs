using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GetTrialCarPosition1 : MonoBehaviour
{
    [SerializeField] DemoCarController DriverCar;
    public GameObject TC1,TC2,TC3,TC4,TC5,TC6, TC7, TC8;
    float TC1position, TC2position, TC3position,TC4position,TC5position,TC6position, TC7position, TC8position, DCPosition;
    public bool TC1bool, TC2bool, TC3bool, TC4bool, TC5bool, TC6bool, TC7bool, TC8bool;

    void Update()
    {
        if(DriverCar.ARbool)
        {
            TC1position = TC1.transform.position.z;
            TC2position = TC2.transform.position.z;
            TC3position = TC3.transform.position.z;
            TC4position = TC4.transform.position.z;
            TC5position = TC5.transform.position.z;
            TC6position = TC6.transform.position.z;
            TC7position = TC7.transform.position.z;
            TC8position = TC8.transform.position.z;
            DCPosition = DriverCar.VolvoCar.transform.position.z;

            if (MathF.Abs(DCPosition - TC1position) <= 10) TC1bool = true; else TC1bool = false;
            if (MathF.Abs(DCPosition - TC2position) <= 10) TC2bool = true; else TC2bool = false;  
            if (MathF.Abs(DCPosition - TC3position) <= 10) TC3bool = true; else TC3bool = false;
            if (MathF.Abs(DCPosition - TC4position) <= 10) TC4bool = true; else TC4bool = false;
            if (MathF.Abs(DCPosition - TC5position) <= 10) TC5bool = true; else TC5bool = false;
            if (MathF.Abs(DCPosition - TC6position) <= 10) TC6bool = true; else TC6bool = false;
            if (MathF.Abs(DCPosition - TC7position) <= 10) TC5bool = true; else TC7bool = false;
            if (MathF.Abs(DCPosition - TC8position)  <= 10) TC6bool = true; else TC8bool = false;
        }



    }
}
