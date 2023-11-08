using PathCreation;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TrialManager : MonoBehaviour
{
    [SerializeField] DemoCarController DC;
    [SerializeField] DC_Collidor DC_C;
    [SerializeField] LeadingCar LC;

    public PathCreator PathCreator_1_L, PathCreator_1_R, PathCreator_2_L, PathCreator_2_R;

    public Transform TargetCar;
    public GameObject TC_1_L1, TC_1_L2, TC_1_L3, TC_1_L4, TC_1_R1, TC_1_R2, TC_1_R3, TC_1_R4;
    public GameObject TC_2_L1, TC_2_L2, TC_2_L3, TC_2_L4, TC_2_R1, TC_2_R2, TC_2_R3, TC_2_R4;
    public GameObject TC_Parent_1, TC_Parent_2;
    public GameObject TaskEndPoint_1, TaskEndPoint_2;
    public GameObject TrialStartNotice, TaskStartNotice;
    public GameObject TC_LC;
    public GameObject LC_1_RearLight;

    Vector3 StartPos_TC_1_L, StartPos_TC_1_R;
    Vector3 StartPos_TC_2_L, StartPos_TC_2_R;
    Vector3 TargetCarVelocity;

    public int MoveTrialCar, TurnOnTrialCars;
    float TC_Speed;
    float OvertakeTimer;
    float StoppingDistance;
    public bool RespawnTrigger;
    bool First;

    private void Start()
    {
        GetStartPos();
        First = true;
    }

    void FixedUpdate()
    {
        if (DC.DrivingPhase)
        {
            if (LC.StartTrial)
                Trial();

            if (RespawnTrigger)
                Respawn();
        }
    }

    void GetStartPos()
    {
        StartPos_TC_1_L = TC_1_L1.transform.position;
        StartPos_TC_1_R = TC_1_R1.transform.position;
        StartPos_TC_2_L = TC_2_L1.transform.position;
        StartPos_TC_2_R = TC_2_R1.transform.position;
    }

    private void Trial()
    {
        TC_Speed += Time.deltaTime;

        if (TurnOnTrialCars == 1)
        {
            TaskEndPoint_1.SetActive(true);
            TC_Parent_1.SetActive(true);
            TurnOnTrialCars = 0;
        }
        else if (TurnOnTrialCars == 2)
        {
            TaskEndPoint_2.SetActive(true);
            TC_Parent_2.SetActive(true);
            TurnOnTrialCars = 0;
        }

        if (First)
            LaneChangeThenStop();

        if (MoveTrialCar == 1)
            MoveTrialCarLeft();
        else if (MoveTrialCar == 2)
            MoveTrialCarRight();
    } 

    private void MoveTrialCarLeft()
    {

        TC_1_L1.transform.position = PathCreator_1_L.path.GetPointAtDistance(TC_Speed * 47f);
        TC_1_R1.transform.position = PathCreator_1_R.path.GetPointAtDistance(TC_Speed * 44f);
        TC_1_L2.transform.position = PathCreator_1_L.path.GetPointAtDistance(TC_Speed * 41f);
        TC_1_R2.transform.position = PathCreator_1_R.path.GetPointAtDistance(TC_Speed * 38f);
        TC_1_L3.transform.position = PathCreator_1_L.path.GetPointAtDistance(TC_Speed * 35f);
        TC_1_R3.transform.position = PathCreator_1_R.path.GetPointAtDistance(TC_Speed * 32f);
        TC_1_L4.transform.position = PathCreator_1_L.path.GetPointAtDistance(TC_Speed * 29f);
        TC_1_R4.transform.position = PathCreator_1_R.path.GetPointAtDistance(TC_Speed * 26f);
    }

    private void MoveTrialCarRight()
    {
        TC_2_L1.transform.position = PathCreator_2_L.path.GetPointAtDistance(TC_Speed * 48f);
        TC_2_R1.transform.position = PathCreator_2_R.path.GetPointAtDistance(TC_Speed * 45f);
        TC_2_L2.transform.position = PathCreator_2_L.path.GetPointAtDistance(TC_Speed * 42f);
        TC_2_R2.transform.position = PathCreator_2_R.path.GetPointAtDistance(TC_Speed * 39f);
        TC_2_L3.transform.position = PathCreator_2_L.path.GetPointAtDistance(TC_Speed * 36f);
        TC_2_R3.transform.position = PathCreator_2_R.path.GetPointAtDistance(TC_Speed * 33f);
        TC_2_L4.transform.position = PathCreator_2_L.path.GetPointAtDistance(TC_Speed * 30f);
        TC_2_R4.transform.position = PathCreator_2_R.path.GetPointAtDistance(TC_Speed * 27f);
    }

    void LaneChangeThenStop()
    {
        TC_LC.SetActive(true);

        OvertakeTimer += Time.deltaTime;
        TargetCarVelocity.z = TargetCar.GetComponent<Rigidbody>().velocity.z;

        if (TargetCarVelocity.z > 27 || TargetCarVelocity.z < -27)
            StoppingDistance = 111.08f; //80, 120 
        else if (TargetCarVelocity.z > 25 || TargetCarVelocity.z < -25)
            StoppingDistance = 104f; // 75
        else if (TargetCarVelocity.z > 22 || TargetCarVelocity.z < -22)
            StoppingDistance = 96f; //66
        else if (TargetCarVelocity.z > 19 || TargetCarVelocity.z < -19)
            StoppingDistance = 84f; // 57

        if (OvertakeTimer <= 8)
        {
            float DistanceBetween_DC_LC = TC_LC.transform.position.z - TargetCar.transform.position.z;

            if (DistanceBetween_DC_LC < StoppingDistance)
                TargetCarVelocity.z *= 1.5f;
            else
                TargetCarVelocity.z *= 0.9f;

            if (OvertakeTimer >= 4)
                    TargetCarVelocity.x = 1.5f;
        }

        if (OvertakeTimer > 8)
            TargetCarVelocity.x = 0;

        if (OvertakeTimer > 16)
        {
            TargetCarVelocity.z = 0;
            LC_1_RearLight.SetActive(true);
        }

        if(OvertakeTimer > 22)
            TC_LC.SetActive(false);

        TC_LC.GetComponent<Rigidbody>().velocity = TargetCarVelocity;
    }

    private void Respawn()
    {
        MoveTrialCar = 0;
        TC_Speed = 0;

        TC_1_L1.transform.position = StartPos_TC_1_L;
        TC_1_L2.transform.position = StartPos_TC_1_L;
        TC_1_L3.transform.position = StartPos_TC_1_L;
        TC_1_L4.transform.position = StartPos_TC_1_L;
        TC_1_R1.transform.position = StartPos_TC_1_R;
        TC_1_R2.transform.position = StartPos_TC_1_R;
        TC_1_R3.transform.position = StartPos_TC_1_R;
        TC_1_R4.transform.position = StartPos_TC_1_R;
        TC_2_L1.transform.position = StartPos_TC_2_L;
        TC_2_L2.transform.position = StartPos_TC_2_L;
        TC_2_L3.transform.position = StartPos_TC_2_L;
        TC_2_L4.transform.position = StartPos_TC_2_L;
        TC_2_R1.transform.position = StartPos_TC_2_R;
        TC_2_R2.transform.position = StartPos_TC_2_R;
        TC_2_R3.transform.position = StartPos_TC_2_R;
        TC_2_R4.transform.position = StartPos_TC_2_R;

        TC_Parent_1.SetActive(false);
        TC_Parent_2.SetActive(false);

        RespawnTrigger = false;
    }
}
