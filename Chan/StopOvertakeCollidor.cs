using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopOvertakeCollidor : MonoBehaviour
{
    [SerializeField] FollowingCar_New FollowingCar_New;
    [SerializeField] LaneChangeCar LaneChangeCar;
    float threshold;
    int changestate;

    void Update()
    {
        if(threshold < 1) threshold += Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("FC"))
        {
            if(threshold > 0.5f)
            {
                changestate++;
                threshold = 0;
            }

            if (changestate == 2)
            {
                FollowingCar_New.StopOvertake = true;
                changestate = 0;
                threshold = 0;
            }
        }
        if (other.gameObject.CompareTag("WayPoint"))
        {
            LaneChangeCar.WayPointTrigger = true;
            LaneChangeCar.TaskStart = false;
        }
    }
}
