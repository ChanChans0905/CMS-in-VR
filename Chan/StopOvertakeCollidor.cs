using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopOvertakeCollidor : MonoBehaviour
{
    [SerializeField] FollowingCar FC;
    [SerializeField] LeadingCar LC;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "FC")
            FC.StopOvertake = true;

        if (other.gameObject.CompareTag("WayPoint"))
        {
            LC.WayPointTrigger = true;
            LC.TaskStart = false;
        }
    }
}
