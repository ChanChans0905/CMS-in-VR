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
    public float trialTime;
    [SerializeField] FadeInOut FadeInOut;
    public bool TrialBoolLocal;
    public GameObject TrialStartNotive, TrialEndNotice;


    private void Update()
    {
        if(TrialBoolLocal == true)
        {
            trialTime += Time.deltaTime;
            if (trialTime >= 10)
            {
                DriverCar.TrialBool = false;
                FadeInOut.FadingEvent = true;
                DriverCar.respawnTrigger = true;
                TrialEndNotice.SetActive(true);
                TrialCar1.SetActive(false); TrialCar2.SetActive(false);
            }

            if (trialTime >= 17)
            {
                DriverCar.waitTimer = 0;
                FadeInOut.FadingEvent = false;
                DriverCar.respawnTrigger = false;
                TrialEndNotice.SetActive(false);
                trialTime = 0;
                TrialBoolLocal = false;
            }
        }

        if(DriverCar.respawnTrigger && DriverCar.TrialBool == false)
        {
            FC1.Respawn = true; FC2.Respawn = true;
            LC1.Respawn = true; LC2.Respawn = true;
        }
        else if(DriverCar.respawnTrigger && DriverCar.TrialBool)
        {
            TrialCar1.SetActive(false);
            TrialCar2.SetActive(false);
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
                Debug.Log(DriverCar.taskCount);
            }
            else
            {
                DriverCar.taskCount++;
                Debug.Log("TaskCOunt Added" + DriverCar.taskCount);
            }

        }

        if(other.gameObject.CompareTag("DriverCar") && DriverCar.TrialBool)
        {
            TrialCar1.SetActive(true);
            TrialCar2.SetActive(true);
            TrialBoolLocal = true;
        }
    }
}
