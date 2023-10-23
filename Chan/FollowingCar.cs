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
    public GameObject FC_Group_1, FC_Group_2;
    public GameObject LC_1, LC_2;
    public Transform TargetCar, Obstacle_1, Obstacle_2;
    Vector3 TargetCarVelocity;
    GameObject FCL_Velocity, FCR_Velocity, FCB_Velocity;
    public float OvertakeTimer;

    public PathCreator PathCreator_LC;
    public PathCreator PathCreator_RC;
    Vector3 StartPos_FCL_1, StartPos_FCL_2, StartPos_FCR_1, StartPos_FCR_2, StartPos_FCB_1, StartPos_FCB_2;
    float FC_Fast_Time = 4f;
    float FC_Slow_Time = 8f;
    float FC_Accel_Timer;
    Vector3 LC_StopPos_for_FC_L, LC_StopPos_for_FC_R, LC_StopPos_for_FC_B;
    //float FC_Fast_ReachingPercent, FC_Slow_ReachingPercent;
    int AccelSpeed;
    public bool RespawnTrigger;

    private void Start()
    {
        SaveStartPos();
    }

    private void FixedUpdate()
    {
        if (DC.DrivingPhase)
        {
            TargetCarVelocity.z = TargetCar.GetComponent<Rigidbody>().velocity.z;

            SetCarByDirection();

            if (LC.LC_StoppingTime == 1)
                ApplyLerp();

            ApplyVelocity();
        }

        if (RespawnTrigger)
            Respawn();
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
        FC_Accel_Timer += Time.fixedDeltaTime;
        float FC_Fast_ReachingPercent = FC_Accel_Timer / FC_Fast_Time;

        AccelSpeed = DC.FollowingCarSpeed[DC.CMSchangeCount - 1, DC.taskCount];

        if (LC.LC_Direction == 1)
            SetTargetObject(Obstacle_1, LC_1);
        else if (LC.LC_Direction == 2)
            SetTargetObject(Obstacle_2, LC_2);

        LC_StopPos_for_FC_L.x = FCL_Velocity.transform.position.x;
        LC_StopPos_for_FC_R.x = FCR_Velocity.transform.position.x;
        LC_StopPos_for_FC_B.x = FCB_Velocity.transform.position.x;

        if (AccelSpeed == 1)
        {
            // LCR is faster
            FCL_Velocity.transform.position = Vector3.Lerp(FCL_Velocity.transform.position, LC_StopPos_for_FC_L, FC_Fast_ReachingPercent / 150f);
            FCR_Velocity.transform.position = Vector3.Lerp(FCR_Velocity.transform.position, LC_StopPos_for_FC_R, FC_Fast_ReachingPercent / 30f);
            FCB_Velocity.transform.position = Vector3.Lerp(FCB_Velocity.transform.position, LC_StopPos_for_FC_B, FC_Fast_ReachingPercent / 150f);
            TargetCarVelocity.x = 0;
            TargetCarVelocity.y = 0;
            TargetCarVelocity.z = 0;
        }
        else if (AccelSpeed == 2)
        {
            // LCL is faster
            FCL_Velocity.transform.position = Vector3.Lerp(FCL_Velocity.transform.position, LC_StopPos_for_FC_L, FC_Fast_ReachingPercent / 30f);
            FCR_Velocity.transform.position = Vector3.Lerp(FCR_Velocity.transform.position, LC_StopPos_for_FC_R, FC_Fast_ReachingPercent / 150f);
            FCB_Velocity.transform.position = Vector3.Lerp(FCB_Velocity.transform.position, LC_StopPos_for_FC_B, FC_Fast_ReachingPercent / 150f);
            TargetCarVelocity.x = 0;
            TargetCarVelocity.y = 0;
            TargetCarVelocity.z = 0;
        }
    }

    void Respawn()
    {
        TargetCarVelocity = Vector3.zero;
        ApplyVelocity();
        AccelSpeed = 0;
        FC_Accel_Timer = 0;
        OvertakeTimer = 0;
        FCL_1.transform.position = StartPos_FCL_1;
        FCL_2.transform.position = StartPos_FCL_2;
        FCR_1.transform.position = StartPos_FCR_1;
        FCR_2.transform.position = StartPos_FCR_2;
        FCB_1.transform.position = StartPos_FCB_1;
        FCB_2.transform.position = StartPos_FCB_2;
        FC_Group_1.SetActive(false);
        FC_Group_2.SetActive(false);
        RespawnTrigger = false;
    }
}

