using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UIElements;

public class TaskCounter : MonoBehaviour
{
    public GameObject FollowingCarLeft, FollowingCarRight, LaneChangingCar, TCParent, WayPoint_LC;
    [SerializeField] DemoCarController DriverCar;
    [SerializeField] TrialManager TM;
    public bool TaskStartBool;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("DriverCar") && TM.TrialTask)
        {
            FollowingCarLeft.SetActive(true);
            FollowingCarRight.SetActive(true);
            LaneChangingCar.SetActive(true);
            WayPoint_LC.SetActive(true);
        }
    }
}
