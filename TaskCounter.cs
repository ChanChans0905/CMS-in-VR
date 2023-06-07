using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UIElements;

public class TaskCounter : MonoBehaviour
{
    public GameObject FollowingCarLeft, FollowingCarRight, LaneChangingCar, TrialCarLeft, TrialCarRight;
    [SerializeField] DemoCarController DriverCar;
    public float trialTime;
    [SerializeField] FadeInOut FadeInOut;
    public bool TrialBoolLocal;

    private void Update()
    {
        if(TrialBoolLocal == true)
        {
            trialTime += Time.deltaTime;
            if (trialTime >= 120)
            {
                DriverCar.TrialBool = false;
                FadeInOut.FadingEvent = true;
                DriverCar.respawnTrigger = true;
                TrialBoolLocal = false;
                // activate notice Text UI 
            }

            if (trialTime >= 125)
            {
                FadeInOut.FadingEvent = false;
                DriverCar.respawnTrigger = false;
                trialTime = 0;
                // deactivate notice Text UI
            }
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

        if(other.gameObject.CompareTag("DriverCar") && DriverCar.TrialBool == true)
        {
            TrialCarLeft.SetActive(true);
            TrialCarRight.SetActive(true);
            TrialBoolLocal = true;
        }
    }
}
