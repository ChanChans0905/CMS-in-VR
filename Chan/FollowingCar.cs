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
    public bool RespawnTrigger;

    private void Start()
    {
        SaveStartPos();
    }

    private void FixedUpdate()
    {
        TargetCarVelocity.z = TargetCar.GetComponent<Rigidbody>().velocity.z;

        SetCarByDirection();

        if (LC.TaskStart)
        {
            if (LC.TaskStartTime > 0)
                Move(DC.taskCount);
            else if(LC.TaskStartTime == 0)
                ApplyVelocity();
        }

        if (RespawnTrigger)
            Respawn();
    }

    void Move(int taskCount)
    {
        OvertakeTimer += Time.deltaTime;

        StoppingTime = DC.LaneChangeTime[DC.CMSchangeCount - 1, taskCount];
        AccelSpeed = DC.FollowingCarSpeed[DC.CMSchangeCount - 1, taskCount];

        if (OvertakeTimer < 8 + StoppingTime && StoppingTime != 0)
            ApplyVelocity();

        if (OvertakeTimer >= 8 + StoppingTime && StoppingTime != 0)
        {
            FC_Accel_Timer += Time.fixedDeltaTime;

            if (LC.LC_Direction == 1)
                SetTargetObject(Obstacle_1, LC_1);
            else if (LC.LC_Direction == 2)
                SetTargetObject(Obstacle_2, LC_2);

            LC_StopPos_for_FC_L.x = FCL_Velocity.transform.position.x;
            LC_StopPos_for_FC_R.x = FCR_Velocity.transform.position.x;
            LC_StopPos_for_FC_B.x = FCB_Velocity.transform.position.x;

            ApplyLerp();
        }

        if (StoppingTime == 0)
            ApplyVelocity();
    }

    void SetCarByDirection()
    {
        if (LC.DrivingDirection == 1)
        {
            FCL_Velocity = FCL_1;
            FCR_Velocity = FCR_1;
            FCB_Velocity = FCB_1;
        }
        else if (LC.DrivingDirection == -1)
        {
            FCL_Velocity = FCL_2;
            FCR_Velocity = FCR_2;
            FCB_Velocity = FCB_2;
        }
    }

    void SaveStartPos()
    {
        StartPos_FCL_1 = FCL_1.transform.position;
        StartPos_FCL_2 = FCL_2.transform.position;
        StartPos_FCR_1 = FCR_1.transform.position;
        StartPos_FCR_2 = FCR_2.transform.position;
        StartPos_FCB_1 = FCB_1.transform.position;
        StartPos_FCB_2 = FCB_2.transform.position;
        FCL_Velocity = FCL_1;
        FCR_Velocity = FCR_1;
        FCB_Velocity = FCB_1;
    }

    void ApplyVelocity()
    {
        FCL_Velocity.GetComponent<Rigidbody>().velocity = TargetCarVelocity;
        FCR_Velocity.GetComponent<Rigidbody>().velocity = TargetCarVelocity;
        FCB_Velocity.GetComponent<Rigidbody>().velocity = TargetCarVelocity;
    }

    void SetTargetObject(Transform TargetObstacle, GameObject TargetCar)
    {
        if (LC.StartScenario_Obstacle)
        {
            LC_StopPos_for_FC_L = TargetObstacle.position;
            LC_StopPos_for_FC_R = TargetObstacle.position;
            LC_StopPos_for_FC_B = TargetObstacle.position;
        }
        else
        {
            LC_StopPos_for_FC_L.z = TargetCar.transform.position.z;
            LC_StopPos_for_FC_R.z = TargetCar.transform.position.z;
            LC_StopPos_for_FC_B.z = TargetCar.transform.position.z;
        }
    }

    void ApplyLerp()
    {
        float FC_Fast_ReachingPercent = FC_Accel_Timer / FC_Fast_Time;

        if (AccelSpeed == 1)
        {
            // LCR is faster
            FCL_Velocity.transform.position = Vector3.Lerp(FCL_Velocity.transform.position, LC_StopPos_for_FC_L, FC_Fast_ReachingPercent / 150f);
            FCR_Velocity.transform.position = Vector3.Lerp(FCR_Velocity.transform.position, LC_StopPos_for_FC_R, FC_Fast_ReachingPercent / 50f);
            FCB_Velocity.transform.position = Vector3.Lerp(FCB_Velocity.transform.position, LC_StopPos_for_FC_B, FC_Fast_ReachingPercent / 150f);
            TargetCarVelocity.x = 0;
            TargetCarVelocity.y = 0;
            TargetCarVelocity.z = 0;
            ApplyVelocity();
        }
        else if (AccelSpeed == 2)
        {
            // LCL is faster
            FCL_Velocity.transform.position = Vector3.Lerp(FCL_Velocity.transform.position, LC_StopPos_for_FC_L, FC_Fast_ReachingPercent / 50f);
            FCR_Velocity.transform.position = Vector3.Lerp(FCR_Velocity.transform.position, LC_StopPos_for_FC_R, FC_Fast_ReachingPercent / 150f);
            FCB_Velocity.transform.position = Vector3.Lerp(FCB_Velocity.transform.position, LC_StopPos_for_FC_B, FC_Fast_ReachingPercent / 150f);
            TargetCarVelocity.x = 0;
            TargetCarVelocity.y = 0;
            TargetCarVelocity.z = 0;
            ApplyVelocity();
        }
    }

    void Respawn()
    {
        TargetCarVelocity = Vector3.zero;
        ApplyVelocity() ;
        StartAceel = false;
        AccelSpeed = 0;
        FC_Accel_Timer = 0;
        OvertakeTimer = 0;
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

