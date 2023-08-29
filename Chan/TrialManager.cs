using PathCreation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrialManager : MonoBehaviour
{
    [SerializeField] DemoCarController DC;
    [SerializeField] DC_Collidor DC_C;

    public PathCreator PathCreator_1_L, PathCreator_1_R, PathCreator_2_L, PathCreator_2_R;

    public GameObject TC_1_L1, TC_1_L2, TC_1_L3, TC_1_L4, TC_1_R1, TC_1_R2, TC_1_R3, TC_1_R4;
    public GameObject TC_2_L1, TC_2_L2, TC_2_L3, TC_2_L4, TC_2_R1, TC_2_R2, TC_2_R3, TC_2_R4;
    public GameObject TC_Parent_1, TC_Parent_2;
    
    public GameObject TrialStartNotice, TaskStartNotice;

    public float TrialTaskTimer;
    public int MoveTrialCar, TurnOnTrialCars;
    public bool TrialTask, ActivateTC_Speed;
    float TC_Speed;
    bool TurnOffTrialCars;

    void FixedUpdate()
    {
        if (TrialTask)
        {
            Debug.Log("Trial");

            TrialTaskTimer += Time.deltaTime;

            if(ActivateTC_Speed)
                TC_Speed += Time.deltaTime * 50;

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

            if (TrialTaskTimer >= 5)
            {
                MoveTrialCar = 0;
                DC.respawnTrigger = true;
                TurnOffTrialCars = true;
                TaskStartNotice.SetActive(true);
            }

            if (TrialTaskTimer >= 7)
            {
                TC_Speed = 0;
                TurnOffTrialCars = false;
                TrialTaskTimer = 0;
                TrialTask = false;
            }

            if (TurnOffTrialCars)
            {
                TC_Parent_1.SetActive(false);
                TC_Parent_2.SetActive(false);
            }
        }
    }

    //TC1.transform.rotation = pathCreator.path.GetRotationAtDistance(TC1Distance);        

    void MoveTrialCarLeft()
    {
        TC_1_L1.transform.position = PathCreator_1_L.path.GetPointAtDistance(TC_Speed);
        TC_1_R2.transform.position = PathCreator_1_R.path.GetPointAtDistance(TC_Speed * 0.9f);
        TC_1_L2.transform.position = PathCreator_1_L.path.GetPointAtDistance(TC_Speed * 0.85f);
        TC_1_R2.transform.position = PathCreator_1_R.path.GetPointAtDistance(TC_Speed * 0.8f);
        TC_1_L3.transform.position = PathCreator_1_L.path.GetPointAtDistance(TC_Speed * 0.75f);
        TC_1_R3.transform.position = PathCreator_1_R.path.GetPointAtDistance(TC_Speed * 0.7f);
        TC_1_L4.transform.position = PathCreator_1_L.path.GetPointAtDistance(TC_Speed * 0.65f);
        TC_1_R4.transform.position = PathCreator_1_R.path.GetPointAtDistance(TC_Speed * 0.6f);
    }

    void MoveTrialCarRight()
    {
        TC_2_L1.transform.position = PathCreator_2_L.path.GetPointAtDistance(TC_Speed);
        TC_2_R2.transform.position = PathCreator_2_R.path.GetPointAtDistance(TC_Speed * 0.9f);
        TC_2_L2.transform.position = PathCreator_2_L.path.GetPointAtDistance(TC_Speed * 0.85f);
        TC_2_R2.transform.position = PathCreator_2_R.path.GetPointAtDistance(TC_Speed * 0.8f);
        TC_2_L3.transform.position = PathCreator_2_L.path.GetPointAtDistance(TC_Speed * 0.75f);
        TC_2_R3.transform.position = PathCreator_2_R.path.GetPointAtDistance(TC_Speed * 0.7f);
        TC_2_L4.transform.position = PathCreator_2_L.path.GetPointAtDistance(TC_Speed * 0.65f);
        TC_2_R4.transform.position = PathCreator_2_R.path.GetPointAtDistance(TC_Speed * 0.6f);
    }
}
