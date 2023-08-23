using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeLogger : MonoBehaviour
{
    public float TimeNumber, LaneChangeEndTime;
    public bool EventBool, LaneChangeComplete;

    void FixedUpdate()
    {
        TimeNumber += Time.deltaTime;
    }
}
