using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UIElements;

public class TaskCounter : MonoBehaviour
{
    public GameObject FollowingCarLeft, FollowingCarRight, LaneChangingCar, TrialCar1, TrialCar2;
    [SerializeField] DemoCarController DriverCar;
    [SerializeField] FollowingCar FC1;
    [SerializeField] FollowingCar2 FC2;
    [SerializeField] LeadingCar LC1;
    [SerializeField] LeadingCar2 LC2;
    [SerializeField] TrialCar TC1;
    [SerializeField] TrialCar TC2;
    [SerializeField] FadeInOut FadeInOut;
    public GameObject TrialStartNotive, TrialEndNotice;
    public bool TaskStartBool;


    private void Update()
    {
        if (DriverCar.TrialBoolTaskCounter)
        {
            if (DriverCar.TrialTime>= 1)
            {
                DriverCar.TrialBool = false;
                FadeInOut.FadingEvent = true;
                DriverCar.respawnTrigger = true;
                TrialEndNotice.SetActive(true);
                TC1.ActivateTC = false;
                TC2.ActivateTC = false;
            }

            if (DriverCar.TrialTime>= 7)
            {
                DriverCar.waitTimer = 0;
                FadeInOut.FadingEvent = false;
                DriverCar.respawnTrigger = false;
                TrialEndNotice.SetActive(false);
                DriverCar.TrialTime = 0;
                DriverCar.TrialBoolTaskCounter= false;
            }
        }

        if(DriverCar.respawnTrigger && DriverCar.TrialBool == false)
        {
            //FC1.Respawn = true; FC2.Respawn = true;
            //LC1.Respawn = true; LC2.Respawn = true;
        }
        else if(DriverCar.respawnTrigger && DriverCar.TrialBool)
        {
            TC1.ActivateTC = false;
            TC2.ActivateTC = false;
        }
    }

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
                Debug.Log("Threshold" + DriverCar.taskCount);
            }
            else
            {
                DriverCar.taskCount++;
                Debug.Log("TaskCOunt Added" + DriverCar.taskCount);
            }

        }
         
        if(other.gameObject.CompareTag("DriverCar") && DriverCar.TrialBool)
        {
            TrialCar1.SetActive(true); TrialCar2.SetActive(true);
            if(DriverCar.TrialBoolTaskCounter == false) DriverCar.TrialBoolTaskCounter = true;
            TC1.ActivateTC = true;
            TC2.ActivateTC = true;

            
        }
    }
}
