using System.Collections;
using System.Collections.Generic;
using System.Data;
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

            if(DriverCar.threshold == false)
            {
                DriverCar.threshold = true;
                Debug.Log(DriverCar.taskCount);
            }
            else
            {
                DriverCar.taskCount++;
                Debug.Log("TaskCOunt Added" + DriverCar.taskCount);
            }

            if(gameObject.transform.localPosition.z < 500 )
            {
                DriverCar.laneChangeDirection = 1;
                Debug.Log("Taskcount1" + DriverCar.LaneChangeTime[DriverCar.taskCount]);
            }
            else if(gameObject.transform.localPosition.z > 500 )
            {
                DriverCar.laneChangeDirection = 2;
                Debug.Log("Taskcount2" + DriverCar.LaneChangeTime[DriverCar.taskCount]);
            }
        }

        if(other.gameObject.CompareTag("DriverCar") && DriverCar.TrialBool == true)
        {
            TrialCarLeft.SetActive(true);
            TrialCarRight.SetActive(true);
        }
    }
}
