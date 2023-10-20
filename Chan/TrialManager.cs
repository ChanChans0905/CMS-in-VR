using PathCreation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrialManager : MonoBehaviour
{
    [SerializeField] DemoCarController DC;
    [SerializeField] DC_Collidor DC_C;
    [SerializeField] LeadingCar LC;

    public PathCreator PathCreator_1_L, PathCreator_1_R, PathCreator_2_L, PathCreator_2_R;

    public GameObject TC_1_L1, TC_1_L2, TC_1_L3, TC_1_L4, TC_1_R1, TC_1_R2, TC_1_R3, TC_1_R4;
    public GameObject TC_2_L1, TC_2_L2, TC_2_L3, TC_2_L4, TC_2_R1, TC_2_R2, TC_2_R3, TC_2_R4;
    public GameObject TC_Parent_1, TC_Parent_2;
    
    public GameObject TrialStartNotice, TaskStartNotice;

    Vector3 StartPos_TC_1_L, StartPos_TC_1_R;
    Vector3 StartPos_TC_2_L, StartPos_TC_2_R;

    public int MoveTrialCar, TurnOnTrialCars;
    float TC_Speed;
    public bool RespawnTrigger;

    private void Start()
    {
        GetStartPos();
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
            TC_Parent_1.SetActive(true);
            TurnOnTrialCars = 0;
        }
        else if (TurnOnTrialCars == 2)
        {
            TC_Parent_2.SetActive(true);
            TurnOnTrialCars = 0;
        }

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
