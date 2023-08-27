using PathCreation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrialManager : MonoBehaviour
{
    [SerializeField] DemoCarController DC;
    [SerializeField] DC_Collidor DC_C;

    public PathCreator pathCreator;

    public GameObject TC_1_L1, TC_1_L2, TC_1_L3, TC_1_L4, TC_1_R1, TC_1_R2, TC_1_R3, TC_1_R4;
    public GameObject TC_2_L1, TC_2_L2, TC_2_L3, TC_2_L4, TC_2_R1, TC_2_R2, TC_2_R3, TC_2_R4;
    
    public GameObject TrialStartNotice, TrialEndNotice;

    public int TurnOnAndMoveTrialCar, TurnOnTrialCars;
    public bool TrialTask, ActivateTC_Speed, TurnOnTrialStartNotice;
    bool CountTrialTime;
    float TrialTaskTimer, TC_Speed;
    float TrialStartNoticeTimer;

    void FixedUpdate()
    {
        if (TrialTask)
        {
            if (CountTrialTime)
                TrialTaskTimer += Time.deltaTime;

            if(ActivateTC_Speed)
                TC_Speed += Time.deltaTime * 50;

            if (TrialTask && TurnOnTrialStartNotice)
            {
                TrialStartNotice.SetActive(true);
                TrialStartNoticeTimer += Time.deltaTime;
                if (TrialStartNoticeTimer > 7)
                {
                    CountTrialTime = true;
                    TrialStartNoticeTimer = 0;
                    TurnOnTrialStartNotice = false;
                    TrialStartNotice.SetActive(false);
                }
            }

            if (TurnOnTrialCars == 1)
            {
                TC_1_L1.SetActive(true);
                TC_1_L2.SetActive(true);
                TC_1_L3.SetActive(true);
                TC_1_L4.SetActive(true);
                TC_1_R1.SetActive(true);
                TC_1_R2.SetActive(true);
                TC_1_R3.SetActive(true);
                TC_1_R4.SetActive(true);
                TurnOnTrialCars = 0;
            }
            else if (TurnOnTrialCars == 2)
            {
                TC_2_L1.SetActive(true);
                TC_2_L2.SetActive(true);
                TC_2_L3.SetActive(true);
                TC_2_L4.SetActive(true);
                TC_2_R1.SetActive(true);
                TC_2_R2.SetActive(true);
                TC_2_R3.SetActive(true);
                TC_2_R4.SetActive(true);
                TurnOnTrialCars = 0;
            }

            if (TurnOnAndMoveTrialCar == 1)
                MoveTrialCarLeft();
                
            else if (TurnOnAndMoveTrialCar == 2)
                MoveTrialCarRight();

            if (TrialTaskTimer >= 90)
            {
                TurnOnAndMoveTrialCar = 0;
                DC_C.FadingEvent = true;
                DC.respawnTrigger = true;
                TrialEndNotice.SetActive(true);
            }

            if (TrialTaskTimer >= 97)
            {
                CountTrialTime = false;
                TC_Speed = 0;
                DC_C.FadingEvent = false;
                TrialTaskTimer = 0;
                TrialEndNotice.SetActive(false);
                TrialTask = false;
            }
        }
    }

    //TC1.transform.rotation = pathCreator.path.GetRotationAtDistance(TC1Distance);        

    void MoveTrialCarLeft()
    {
        TC_1_L1.transform.position = pathCreator.path.GetPointAtDistance(TC_Speed);
        TC_1_R2.transform.position = pathCreator.path.GetPointAtDistance(TC_Speed * 0.9f);
        TC_1_L2.transform.position = pathCreator.path.GetPointAtDistance(TC_Speed * 0.85f);
        TC_1_R2.transform.position = pathCreator.path.GetPointAtDistance(TC_Speed * 0.8f);
        TC_1_L3.transform.position = pathCreator.path.GetPointAtDistance(TC_Speed * 0.75f);
        TC_1_R3.transform.position = pathCreator.path.GetPointAtDistance(TC_Speed * 0.7f);
        TC_1_L4.transform.position = pathCreator.path.GetPointAtDistance(TC_Speed * 0.65f);
        TC_1_R4.transform.position = pathCreator.path.GetPointAtDistance(TC_Speed * 0.6f);
    }

    void MoveTrialCarRight()
    {
        TC_2_L1.transform.position = pathCreator.path.GetPointAtDistance(TC_Speed);
        TC_2_R2.transform.position = pathCreator.path.GetPointAtDistance(TC_Speed * 0.9f);
        TC_2_L2.transform.position = pathCreator.path.GetPointAtDistance(TC_Speed * 0.85f);
        TC_2_R2.transform.position = pathCreator.path.GetPointAtDistance(TC_Speed * 0.8f);
        TC_2_L3.transform.position = pathCreator.path.GetPointAtDistance(TC_Speed * 0.75f);
        TC_2_R3.transform.position = pathCreator.path.GetPointAtDistance(TC_Speed * 0.7f);
        TC_2_L4.transform.position = pathCreator.path.GetPointAtDistance(TC_Speed * 0.65f);
        TC_2_R4.transform.position = pathCreator.path.GetPointAtDistance(TC_Speed * 0.6f);
    }
}
