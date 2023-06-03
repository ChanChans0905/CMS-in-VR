using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UIElements;

public class TaskCounter : MonoBehaviour
{
    public GameObject FollowingCarLeft, FollowingCarRight, LaneChangingCar, TrialCarLeft, TrialCarRight;
    [SerializeField] DemoCarController DriverCar;
    public bool threshold = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("DriverCar") && DriverCar.TrialBool == false)
        {
            FollowingCarLeft.SetActive(true);
            FollowingCarRight.SetActive(true);
            LaneChangingCar.SetActive(true);

            if (threshold == false)
            {
                threshold = true;
            }
            else
            {
                DriverCar.taskCount++;
            }

            if (gameObject.transform.localPosition.z < 500)
            {
                DriverCar.laneChangeDirection = 1;
            }
            else if (gameObject.transform.localPosition.z > 500)
            {
                DriverCar.laneChangeDirection = 2;
            }
        }

        if (other.gameObject.CompareTag("DriverCar") && DriverCar.TrialBool == true)
        {
            TrialCarLeft.SetActive(true);
            TrialCarRight.SetActive(true);
        }
    }
}
