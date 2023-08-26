using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UIElements;

public class TaskCounter : MonoBehaviour
{
    public GameObject FollowingCarLeft, FollowingCarRight, LaneChangingCar, TCParent, WayPoint_LC;
    [SerializeField] DemoCarController DriverCar;
    [SerializeField] TrialCar TC;
    [SerializeField] FadeInOut FadeInOut;
    public GameObject TrialEndNotice;
    public bool TaskStartBool;


    private void Update()
    {
        if (DriverCar.TrialBoolTaskCounter)
        {
            if (DriverCar.TrialTime>= 90)
            {
                DriverCar.TrialBool = false;
                FadeInOut.FadingEvent = true;
                DriverCar.respawnTrigger = true;
                TrialEndNotice.SetActive(true);
                TCParent.SetActive(false); 
            }

            if (DriverCar.TrialTime>= 97)
            {
                DriverCar.waitTimer = 0;
                FadeInOut.FadingEvent = false;
                DriverCar.respawnTrigger = false;
                TrialEndNotice.SetActive(false);
                DriverCar.TrialTime = 0;
                DriverCar.TrialBoolTaskCounter= false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("DriverCar") && !DriverCar.TrialBool)
        {
            FollowingCarLeft.SetActive(true);
            FollowingCarRight.SetActive(true);
            LaneChangingCar.SetActive(true);
            WayPoint_LC.SetActive(true);

            if(!DriverCar.threshold)
                DriverCar.threshold = true;
            else
                DriverCar.taskCount++;
        }
         
        if(other.gameObject.CompareTag("DriverCar") && DriverCar.TrialBool)
        {
            TCParent.SetActive(true);
            TC.TC1Distance = 0;
            TC.TrialStartNoticeTimer = 0;
            if (DriverCar.TrialBoolTaskCounter == false)
                DriverCar.TrialBoolTaskCounter = true;
        }
    }
}
