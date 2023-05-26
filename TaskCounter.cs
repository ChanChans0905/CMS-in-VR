using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TaskCounter : MonoBehaviour
{
    public GameObject FollowingCarLeft, FollowingCarRight, LaneChangingCar, TrialCarLeft, TrialCarRight;
    [SerializeField] DemoCarController DriverCar;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("DriverCar") && DriverCar.TrialBool == false)
        {
            FollowingCarLeft.SetActive(true);
            FollowingCarRight.SetActive(true);
            LaneChangingCar.SetActive(true);
            DriverCar.taskCount++;
        }

        if(other.gameObject.CompareTag("DriverCar") && DriverCar.TrialBool == true)
        {
            TrialCarLeft.SetActive(true);
            TrialCarRight.SetActive(true);
        }
    }
}
