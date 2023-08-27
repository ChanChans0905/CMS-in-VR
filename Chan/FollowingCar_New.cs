using PathCreation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingCar_New : MonoBehaviour
{
    [SerializeField] DemoCarController DriverCar;
    [SerializeField] LaneChangeCar LaneChangeCar;
    public GameObject FCL_1, FCR_1, FCL_2, FCR_2;
    public Transform TargetCar;
    Vector3 TargetCarVelocity;
    GameObject FCL_Velocity, FCR_Velocity;
    public float OvertakeTimer;
    public bool StopOvertake;
    public PathCreator PathCreator_LC;
    public PathCreator PathCreator_RC;
    Vector3 StartPos_FCL_1, StartPos_FCL_2, StartPos_FCR_1, StartPos_FCR_2;
    bool StartAceel;

    private void Start()
    {
        StartPos_FCL_1 = FCL_1.transform.position;
        StartPos_FCL_2 = FCL_2.transform.position;
        StartPos_FCR_1 = FCR_1.transform.position;
        StartPos_FCR_2 = FCR_2.transform.position;
    }

    private void FixedUpdate()
    {
        TargetCarVelocity.z = TargetCar.GetComponent<Rigidbody>().velocity.z;

        if(LaneChangeCar.DrivingDirection == 1)
        {
            FCL_Velocity = FCL_1;
            FCR_Velocity = FCR_1;
        }
        else
        {
            FCL_Velocity = FCL_2;
            FCR_Velocity = FCR_2;
        }

        if (LaneChangeCar.TaskStart)
            StartAceel = true;
        else
        {
            FCL_Velocity.GetComponent<Rigidbody>().velocity = TargetCarVelocity;
            FCR_Velocity.GetComponent<Rigidbody>().velocity = TargetCarVelocity;
        }

        if(StartAceel && LaneChangeCar.TaskStartTime != 0)
            Accel(DriverCar.taskCount);

        if (StopOvertake)
            TargetCarVelocity.z = 0;


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
            FCL_Velocity.GetComponent<Rigidbody>().velocity = TargetCarVelocity;
            FCR_Velocity.GetComponent<Rigidbody>().velocity = TargetCarVelocity;
        }

        // accel
        if (OvertakeTimer > 8 + StoppingTime && StoppingTime != 0)
        {
            if(AccelSpeed == 1)
            {
                FCL_Velocity.GetComponent<Rigidbody>().velocity = TargetCarVelocity * 1.5f;
                FCR_Velocity.GetComponent<Rigidbody>().velocity = TargetCarVelocity * 1.3f;
            }
            else
            {
                FCL_Velocity.GetComponent<Rigidbody>().velocity = TargetCarVelocity * 1.3f;
                FCR_Velocity.GetComponent<Rigidbody>().velocity = TargetCarVelocity * 1.5f;
            }
        }

        if(StoppingTime == 0)
        {
            FCL_Velocity.GetComponent<Rigidbody>().velocity = TargetCarVelocity;
            FCR_Velocity.GetComponent<Rigidbody>().velocity = TargetCarVelocity;
        }
    }

    private void Respawn()
    {
        OvertakeTimer = 0;
        StopOvertake = false;
        FCL_1.transform.position = StartPos_FCL_1;
        FCL_2.transform.position = StartPos_FCL_2;
        FCR_1.transform.position = StartPos_FCR_1;
        FCR_2.transform.position = StartPos_FCR_2;
        //gameObject.SetActive(false);
    }
}

