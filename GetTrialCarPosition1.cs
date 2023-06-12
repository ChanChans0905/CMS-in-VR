using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GetTrialCarPosition1 : MonoBehaviour
{
    [SerializeField] DemoCarController DriverCar;
    public GameObject TC1,TC2,TC3,TC4,TC5,TC6;
    public float TC1position, TC2position, TC3position,TC4position,TC5position,TC6position;
    public bool TC1bool, TC2bool, TC3bool, TC4bool, TC5bool, TC6bool;


    void Update()
    {
        TC1position = TC1.transform.position.z;
        TC2position = TC2.transform.position.z;
        TC3position = TC3.transform.position.z;
        TC4position = TC4.transform.position.z;
        TC5position = TC5.transform.position.z;
        TC6position = TC6.transform.position.z;

        if (MathF.Abs(Mathf.Abs(DriverCar.transform.position.x) - Mathf.Abs(TC1position)) <= 15) { TC1bool = true;}
        else if (MathF.Abs(Mathf.Abs(DriverCar.transform.position.x) - Mathf.Abs(TC2position)) <= 15) { TC2bool = true; }
        else if (MathF.Abs(Mathf.Abs(DriverCar.transform.position.x) - Mathf.Abs(TC3position)) <= 15) { TC3bool = true; }
        else if (MathF.Abs(Mathf.Abs(DriverCar.transform.position.x) - Mathf.Abs(TC4position)) <= 15) { TC4bool = true; }
        else if (MathF.Abs(Mathf.Abs(DriverCar.transform.position.x) - Mathf.Abs(TC5position)) <= 15) { TC5bool = true; }
        else if (MathF.Abs(Mathf.Abs(DriverCar.transform.position.x) - Mathf.Abs(TC6position)) <= 15) { TC6bool = true; }

        if (MathF.Abs(Mathf.Abs(DriverCar.transform.position.x) - Mathf.Abs(TC1position)) >= 15) { TC1bool = false;}
        else if (MathF.Abs(Mathf.Abs(DriverCar.transform.position.x) - Mathf.Abs(TC2position)) >= 15) { TC2bool = false; }
        else if (MathF.Abs(Mathf.Abs(DriverCar.transform.position.x) - Mathf.Abs(TC3position)) >= 15) { TC3bool = false; }
        else if (MathF.Abs(Mathf.Abs(DriverCar.transform.position.x) - Mathf.Abs(TC4position)) >= 15) { TC4bool = false; }
        else if (MathF.Abs(Mathf.Abs(DriverCar.transform.position.x) - Mathf.Abs(TC5position)) >= 15) { TC5bool = false; }
        else if (MathF.Abs(Mathf.Abs(DriverCar.transform.position.x) - Mathf.Abs(TC6position)) >= 15) { TC6bool = false; }
    }
}
