using PathCreation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingCar_New : MonoBehaviour
{
    [SerializeField] DemoCarController DriverCar;
    [SerializeField] LaneChangeCar LaneChangeCar;
    public GameObject CarLeft, CarRight;
    public Transform TargetCar;
    Vector3 TargetCarVelocity;
    Vector3 CarLeftVelocity, CarRightVelocity;
    public float OvertakeTimer;
    bool StopOvertake;
    bool WayPointTrigger;
    float DistanceTravelledForCarLeft, DistanceTravelledForCarRight;
    public PathCreator PathCreatorForCarLeft;
    public PathCreator PathCreatorForCarRight;
    float DisableTime;
    bool RespawnTrigger;
    Vector3 StartPos;

    private void FixedUpdate()
    {
        TargetCarVelocity.z = TargetCar.GetComponent<Rigidbody>().velocity.z;

        if (LaneChangeCar.TaskStart)
            Accel(DriverCar.taskCount);

        //if (WayPointTrigger)
        //    WayPointDrivingForCarLeft();

        if(LaneChangeCar.BeforeTaskStart)
        {
            CarLeft.GetComponent<Rigidbody>().velocity = TargetCarVelocity;
            CarRight.GetComponent<Rigidbody>().velocity = TargetCarVelocity;
        }

        if (DriverCar.respawnTrigger)
            Respawn();
    }

    private void Accel(int taskCount)
    {
        OvertakeTimer += Time.deltaTime;

        int StoppingTime = DriverCar.LaneChangeTime[taskCount];
        int AccelSpeed = DriverCar.FollowingCarSpeed[taskCount];

        if(OvertakeTimer < 8 + StoppingTime && StoppingTime != 0)
        {
            CarLeft.GetComponent<Rigidbody>().velocity = TargetCarVelocity;
            CarRight.GetComponent<Rigidbody>().velocity = TargetCarVelocity;
        }

        // accel
        if (OvertakeTimer > 8 + StoppingTime && StoppingTime != 0)
        {
            if(AccelSpeed == 1)
            {
                CarLeft.GetComponent<Rigidbody>().velocity = TargetCarVelocity * 1.3f;
                CarRight.GetComponent<Rigidbody>().velocity = TargetCarVelocity * 1.1f;
            }
            else
            {
                CarLeft.GetComponent<Rigidbody>().velocity = TargetCarVelocity * 1.1f;
                CarRight.GetComponent<Rigidbody>().velocity = TargetCarVelocity * 1.3f;
            }
        }

        if(StoppingTime == 0)
        {
            CarLeft.GetComponent<Rigidbody>().velocity = TargetCarVelocity;
            CarRight.GetComponent<Rigidbody>().velocity = TargetCarVelocity;
        }
    }

    //private void WayPointDrivingForCarLeft()
    //{
    //    DistanceTravelledForCarLeft += Time.deltaTime * 20;
    //    transform.position = PathCreatorForCarLeft.path.GetPointAtDistance(DistanceTravelledForCarLeft);
    //    transform.rotation = PathCreatorForCarLeft.path.GetRotationAtDistance(DistanceTravelledForCarLeft);
    //    DisableTime += Time.deltaTime;

    //    if (DisableTime > 20)
    //        RespawnTrigger = true;
    //}

    private void Respawn()
    {
        OvertakeTimer = 0;
        DistanceTravelledForCarLeft = 0;
        DistanceTravelledForCarRight = 0;
        DisableTime = 0;
        WayPointTrigger = false;
        RespawnTrigger = false;
        StopOvertake = false;
        gameObject.transform.position = StartPos;
        gameObject.transform.rotation = Quaternion.identity;
        gameObject.SetActive(false);
    }
}

