using PathCreation;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class FollowingCar : MonoBehaviour
{
    [SerializeField] DemoCarController DC;
    [SerializeField] LeadingCar LC;
    public GameObject FCL_1, FCR_1, FCL_2, FCR_2, FCB_1, FCB_2;
    public GameObject LC_1, LC_2;
    public Transform TargetCar, Obstacle_1, Obstacle_2;
    Vector3 TargetCarVelocity;
    GameObject FCL_Velocity, FCR_Velocity, FCB_Velocity;
    public float OvertakeTimer;
    public bool StopOvertake;
    public PathCreator PathCreator_LC;
    public PathCreator PathCreator_RC;
    Vector3 StartPos_FCL_1, StartPos_FCL_2, StartPos_FCR_1, StartPos_FCR_2, StartPos_FCB_1, StartPos_FCB_2;
    bool StartAceel;
    float FC_Fast_Time = 4f;
    float FC_Slow_Time = 8f;
    float FC_Accel_Timer;
    Vector3 LC_StopPos_for_FC_L, LC_StopPos_for_FC_R, LC_StopPos_for_FC_B;
    //float FC_Fast_ReachingPercent, FC_Slow_ReachingPercent;
    int StoppingTime, AccelSpeed;
    bool RespawnTrigger;

    private void Start()
    {
        StartPos_FCL_1 = FCL_1.transform.position;
        StartPos_FCL_2 = FCL_2.transform.position;
        StartPos_FCR_1 = FCR_1.transform.position;
        StartPos_FCR_2 = FCR_2.transform.position;
        StartPos_FCB_1 = FCB_1.transform.position;
        StartPos_FCB_2 = FCB_2.transform.position;
    }

    private void FixedUpdate()
    {
        TargetCarVelocity.z = TargetCar.GetComponent<Rigidbody>().velocity.z;

        if (LC.DrivingDirection == 1)
        {
            FCL_Velocity = FCL_1;
            FCR_Velocity = FCR_1;
            FCB_Velocity = FCB_1;
            //LC.DrivingDirection = 0;
        }
        else if (LC.DrivingDirection == -1)
        {
            FCL_Velocity = FCL_2;
            FCR_Velocity = FCR_2;
            FCB_Velocity = FCB_2;
            //LC.DrivingDirection = 0;
        }

        if (LC.TaskStart)
            StartAceel = true;
        else
        {
            FCL_Velocity.GetComponent<Rigidbody>().velocity = TargetCarVelocity;
            FCR_Velocity.GetComponent<Rigidbody>().velocity = TargetCarVelocity;
            FCB_Velocity.GetComponent<Rigidbody>().velocity = TargetCarVelocity;
        }

        if (StartAceel && LC.TaskStartTime != 0)
            Accel(DC.taskCount);

        if (StopOvertake)
            TargetCarVelocity.z = 0;

        if (DC.respawnTrigger)
            RespawnTrigger = true;

        if (RespawnTrigger)
            Respawn();
    }

    private void Accel(int taskCount)
    {
        OvertakeTimer += Time.deltaTime;

        StoppingTime = DC.LaneChangeTime[DC.CMSchangeCount-1,taskCount];
        AccelSpeed = DC.FollowingCarSpeed[taskCount];

        if (OvertakeTimer < 8 + StoppingTime && StoppingTime != 0)
        {
            FCL_Velocity.GetComponent<Rigidbody>().velocity = TargetCarVelocity;
            FCR_Velocity.GetComponent<Rigidbody>().velocity = TargetCarVelocity;
            FCB_Velocity.GetComponent<Rigidbody>().velocity = TargetCarVelocity;
        }

        if (OvertakeTimer >= 8 + StoppingTime && StoppingTime != 0)
        {
            FC_Accel_Timer += Time.fixedDeltaTime;

            if (LC.LC_Direction == 1)
            {
                if (LC.StartScenario_Obstacle)
                {
                    LC_StopPos_for_FC_L = Obstacle_1.position;
                    LC_StopPos_for_FC_R = Obstacle_1.position;
                    LC_StopPos_for_FC_B = Obstacle_1.position;
                }
                else
                {
                    LC_StopPos_for_FC_L.z = LC_1.transform.position.z;
                    LC_StopPos_for_FC_R.z = LC_1.transform.position.z;
                    LC_StopPos_for_FC_B.z = LC_1.transform.position.z;
                }
            }
            else if (LC.LC_Direction == 2)
            {
                if (LC.StartScenario_Obstacle)
                {
                    LC_StopPos_for_FC_L = Obstacle_2.position;
                    LC_StopPos_for_FC_R = Obstacle_2.position;
                    LC_StopPos_for_FC_B = Obstacle_2.position;
                }
                else
                {
                    LC_StopPos_for_FC_L.z = LC_2.transform.position.z;
                    LC_StopPos_for_FC_R.z = LC_2.transform.position.z;
                    LC_StopPos_for_FC_B.z = LC_2.transform.position.z;
                }
            }


            LC_StopPos_for_FC_L.x = FCL_Velocity.transform.position.x;
            LC_StopPos_for_FC_R.x = FCR_Velocity.transform.position.x;
            LC_StopPos_for_FC_B.x = FCB_Velocity.transform.position.x;

            float FC_Fast_ReachingPercent = FC_Accel_Timer / FC_Fast_Time;
            float FC_Slow_ReachingPercent = FC_Accel_Timer / FC_Slow_Time;

            if (AccelSpeed == 1)
            {
                // LCR is faster
                FCL_Velocity.transform.position = Vector3.Lerp(FCL_Velocity.transform.position, LC_StopPos_for_FC_L, FC_Fast_ReachingPercent / 400f);
                FCR_Velocity.transform.position = Vector3.Lerp(FCR_Velocity.transform.position, LC_StopPos_for_FC_R, FC_Fast_ReachingPercent / 100f);
                FCB_Velocity.transform.position = Vector3.Lerp(FCB_Velocity.transform.position, LC_StopPos_for_FC_B, FC_Fast_ReachingPercent / 100f);
                TargetCarVelocity.x = 0;
                TargetCarVelocity.z = 0;
                TargetCarVelocity.y = 0;
                FCL_Velocity.GetComponent<Rigidbody>().velocity = TargetCarVelocity;
                FCR_Velocity.GetComponent<Rigidbody>().velocity = TargetCarVelocity;
                FCB_Velocity.GetComponent<Rigidbody>().velocity = TargetCarVelocity;
            }
            else if (AccelSpeed == 2)
            {
                // LCL is faster
                FCL_Velocity.transform.position = Vector3.Lerp(FCL_Velocity.transform.position, LC_StopPos_for_FC_L, FC_Fast_ReachingPercent / 150f);
                FCR_Velocity.transform.position = Vector3.Lerp(FCR_Velocity.transform.position, LC_StopPos_for_FC_R, FC_Fast_ReachingPercent / 600f);
                FCB_Velocity.transform.position = Vector3.Lerp(FCB_Velocity.transform.position, LC_StopPos_for_FC_B, FC_Fast_ReachingPercent / 150f);
                TargetCarVelocity.x = 0;
                TargetCarVelocity.z = 0;
                TargetCarVelocity.y = 0;
                FCL_Velocity.GetComponent<Rigidbody>().velocity = TargetCarVelocity;
                FCR_Velocity.GetComponent<Rigidbody>().velocity = TargetCarVelocity;
                FCB_Velocity.GetComponent<Rigidbody>().velocity = TargetCarVelocity;
                //Debug.Log(LC_StopPos_for_FC_L);
            }
        }

        if (StoppingTime == 0)
        {
            FCL_Velocity.GetComponent<Rigidbody>().velocity = TargetCarVelocity;
            FCR_Velocity.GetComponent<Rigidbody>().velocity = TargetCarVelocity;
            FCB_Velocity.GetComponent<Rigidbody>().velocity = TargetCarVelocity;
            Debug.Log("C");
        }
    }

    private void Respawn()
    {
        StartAceel = false;
        AccelSpeed = 0;
        FC_Accel_Timer = 0;
        OvertakeTimer = 0;
        StopOvertake = false;
        FCL_1.transform.position = StartPos_FCL_1;
        FCL_2.transform.position = StartPos_FCL_2;
        FCR_1.transform.position = StartPos_FCR_1;
        FCR_2.transform.position = StartPos_FCR_2;
        FCB_1.transform.position = StartPos_FCB_1;
        FCB_2.transform.position = StartPos_FCB_2;
        FCL_1.SetActive(false);
        FCL_2.SetActive(false);
        FCR_1.SetActive(false);
        FCR_2.SetActive(false);
        FCB_1.SetActive(false);
        FCB_2.SetActive(false);
        RespawnTrigger = false;
    }
}

