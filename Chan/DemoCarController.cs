using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using VolvoCars.Data;
using Unity.VisualScripting;

public class DemoCarController : MonoBehaviour
{

    [Header("Settings")]
    [SerializeField] private bool brakeToReverse = true;
    [SerializeField] private AnimationCurve availableForwardTorque = AnimationCurve.Constant(0, 50, 2700);
    [SerializeField] private AnimationCurve availableReverseTorque = AnimationCurve.Linear(0, 2700, 15, 0);

    [Header("Data")] // This is how you reference custom data, both for read and write purposes.
    [SerializeField] private VolvoCars.Data.PropulsiveDirection propulsiveDirection = default;
    [SerializeField] private VolvoCars.Data.WheelTorque wheelTorque = default;
    [SerializeField] private VolvoCars.Data.UserSteeringInput userSteeringInput = default;
    [SerializeField] private VolvoCars.Data.Velocity velocity = default;
    [SerializeField] private VolvoCars.Data.GearLeverIndication gearLeverIndication = default;
    //[SerializeField] private VolvoCars.Data.DoorIsOpenR1L doorIsOpenR1L = default; // R1L stands for Row 1 Left.
    //[SerializeField] private VolvoCars.Data.LampBrake lampBrake = default;

    [SerializeField] LeadingCar LC;
    [SerializeField] DC_Collidor DC_C;
    [SerializeField] CSV_Save_Processed CSV_P;

    public GameObject VolvoCar;
    public GameObject CMS_LD_SW, CMS_LD_TM, CMS_RD_SW, CMS_RD_TM, CMSStitched, TraditionalMirrorLeft, TraditionalMirrorRight;
    public GameObject CMS_CAM_L, CMS_CAM_R, CMS_CAM_S, CAM_TM_L, CAM_TM_R;

    // Default
    public int SampleNumber;
    public int taskCount;
    public bool ResetTrigger, RespawnTrigger;
    public float waitTimer;
    public bool FirstTaskCountThreshold;
    public bool DrivingPhase;

    // CMS
    public int CMSchangeCount;
    public bool CMSchangeBool;

    // AR
    public bool Activate_AR;

    // Questionnaire
    public int QuestionnaireCount;
    public bool FinalQuestionnaireBool;

    // Array
    public bool SelectArray;
    public int laneChangeDirection;
    public int[,] TaskScenario = new int[7, 7];
    public int[,] LaneChangeTime = new int[7, 7]; // make the 2 dimension list by using counter-balance
    public int[,] FollowingCarSpeed = new int[7, 7];
    public int[] CMScombination = new int[80];
    int[,] CMScombination_Array = new int[70, 7];

    // Data
    public float Acc, Br, SW, SteeringWheel_Data, Pedal_Data;
    public float VelocityValue;
    int ReactionStarted, ReactionNoCount;
    public int TotalFirstReactionValue, NumOfCollision;
    float LogTimer;
    public int LaneChangeComplete;
    public float FC1Lposition, FC1Rposition, LC1position, FC2Lposition, FC2Rposition, LC2position, DCposition;

    #region Private variables not shown in the inspector
    private VolvoCars.Data.Value.Public.WheelTorque wheelTorqueValue = new VolvoCars.Data.Value.Public.WheelTorque(); // This is the value type used by the wheelTorque data item.     
    //private VolvoCars.Data.Value.Public.LampGeneral lampValue = new VolvoCars.Data.Value.Public.LampGeneral(); // This is the value type used by lights/lamps
    private float totalTorque;  // The total torque requested by the user, will be split between the four wheels
    private float steeringReduction; // Used to make it easier to drive with keyboard in higher speeds
    public const float MAX_BRAKE_TORQUE = 8000; // [Nm]
    #endregion

    private void Update()
    {
        if (LogitechGSDK.LogiUpdate() && LogitechGSDK.LogiIsConnected(0))
        {
            LogitechGSDK.DIJOYSTATE2ENGINES rec;
            rec = LogitechGSDK.LogiGetStateUnity(0);

            // Driving inputs 
            float rawSteeringInput /*= Input.GetAxis("Horizontal")*/;
            float rawForwardInput /*= Input.GetAxis("Vertical")*/;
            float parkInput = Input.GetAxis("Jump");

            VelocityValue = velocity.Value;
            Acc = (32768f - rec.lY) / 65536f;
            Br = (rec.lRz - 32768f) / 65536f;
            rawForwardInput = Acc + Br;
            Pedal_Data = rawForwardInput;

            SW = rec.lX / 32656f;
            rawSteeringInput = SW;

            // Steering
            //steeringReduction = 1 - Mathf.Min(Mathf.Abs(velocity.Value) / 30f, 0.85f);

            userSteeringInput.Value = rawSteeringInput /* * steeringReduction*/;
            SteeringWheel_Data = userSteeringInput.Value;

            if (parkInput > 0)
            { // Park request ("hand brake")
                if (Mathf.Abs(velocity.Value) > 5f / 3.6f)
                {
                    totalTorque = -MAX_BRAKE_TORQUE; // Regular brakes
                }
                else
                {
                    totalTorque = -9000; // Parking brake and/or gear P
                    propulsiveDirection.Value = 0;
                    gearLeverIndication.Value = 0;
                }

            }
            else if (propulsiveDirection.Value == 1)
            { // Forward

                if (rawForwardInput >= 0 && velocity.Value > -1.5f)
                {
                    totalTorque = Mathf.Min(availableForwardTorque.Evaluate(Mathf.Abs(velocity.Value)), -1800 + 7900 * rawForwardInput - 9500 * rawForwardInput * rawForwardInput + 9200 * rawForwardInput * rawForwardInput * rawForwardInput);
                }
                else
                {
                    totalTorque = -Mathf.Abs(rawForwardInput) * MAX_BRAKE_TORQUE;
                    if (Mathf.Abs(velocity.Value) < 0.01f && brakeToReverse)
                    {
                        propulsiveDirection.Value = -1;
                        gearLeverIndication.Value = 1;
                    }
                }

            }
            else if (propulsiveDirection.Value == -1)
            { // Reverse
                if (rawForwardInput <= 0 && velocity.Value < 1.5f)
                {
                    float absInput = Mathf.Abs(rawForwardInput);
                    totalTorque = Mathf.Min(availableReverseTorque.Evaluate(Mathf.Abs(velocity.Value)), -1800 + 7900 * absInput - 9500 * absInput * absInput + 9200 * absInput * absInput * absInput);
                }
                else
                {
                    totalTorque = -Mathf.Abs(rawForwardInput) * MAX_BRAKE_TORQUE;
                    if (Mathf.Abs(velocity.Value) < 0.01f)
                    {
                        propulsiveDirection.Value = 1;
                        gearLeverIndication.Value = 3;
                    }
                }

            }
            //else if (RespawnTrigger == true) totalTorque = -9000;
            else
            { // No direction (such as neutral gear or P)
                totalTorque = 0;
                if (Mathf.Abs(velocity.Value) < 1f)
                {
                    if (rawForwardInput > 0)
                    {
                        propulsiveDirection.Value = 1;
                        gearLeverIndication.Value = 3;
                    }
                    else if (rawForwardInput < 0 && brakeToReverse)
                    {
                        propulsiveDirection.Value = -1;
                        gearLeverIndication.Value = 1;
                    }
                }
                else if (gearLeverIndication.Value == 0)
                {
                    totalTorque = -9000;
                }
            }

            if (velocity.Value >= 27.7f)
                totalTorque = 0;

            ApplyWheelTorques(totalTorque);

            if (SelectArray)
                ApplyArray();

            if (LC.LC_StoppingTime == 1)
                LogData();

            if (RespawnTrigger)
                Respawn();

            if (ResetTrigger)
                ResetData();

            if (CMSchangeBool)
                CMSchange();
        }
    }

    private void ApplyWheelTorques(float totalWheelTorque)
    {
        // Set the torque values for the four wheels.
        wheelTorqueValue.fL = 0.4f * totalWheelTorque / 4f;
        wheelTorqueValue.fR = 0.4f * totalWheelTorque / 4f;
        wheelTorqueValue.rL = 1.6f * totalWheelTorque / 4f;
        wheelTorqueValue.rR = 1.6f * totalWheelTorque / 4f;

        // Update the wheel torque data item with the new values. This is accessible to other scripts, such as chassis dynamics.
        wheelTorque.Value = wheelTorqueValue;
    }

    public void CMSchange()
    {
        ResetCMS();

        switch (CMScombination[CMSchangeCount])
        {
            case 1: // Traditional Mirror
                {
                    TraditionalMirrorLeft.SetActive(true);
                    TraditionalMirrorRight.SetActive(true);
                    CAM_TM_L.SetActive(true);
                    CAM_TM_R.SetActive(true);
                    Activate_AR = false;
                    break;
                }
            case 2: // CMS under A-pillar
                {
                    CMS_LD_TM.SetActive(true);
                    CMS_RD_TM.SetActive(true);
                    CMS_CAM_L.SetActive(true);
                    CMS_CAM_R.SetActive(true);
                    Activate_AR = false;
                    break;
                }
            case 3: // CMS near the Steering Wheel
                {
                    CMS_LD_SW.SetActive(true);
                    CMS_RD_SW.SetActive(true);
                    CMS_CAM_L.SetActive(true);
                    CMS_CAM_R.SetActive(true);
                    Activate_AR = false;
                    break;
                }
            case 4: // CMS Stitched
                {
                    CMSStitched.SetActive(true);
                    CMS_CAM_S.SetActive(true);
                    Activate_AR = false;
                    break;
                }
            case 5: // CMS under A-pillar with AR signal
                {
                    CMS_LD_TM.SetActive(true);
                    CMS_RD_TM.SetActive(true);
                    CMS_CAM_L.SetActive(true);
                    CMS_CAM_R.SetActive(true);
                    Activate_AR = true;
                    break;
                }
            case 6: // CMS near the Steering Wheel with AR signal
                {
                    CMS_LD_SW.SetActive(true);
                    CMS_RD_SW.SetActive(true);
                    CMS_CAM_L.SetActive(true);
                    CMS_CAM_R.SetActive(true);
                    Activate_AR = true;
                    break;
                }
            case 7: // CMS Stitched with AR signal
                {
                    CMSStitched.SetActive(true);
                    CMS_CAM_S.SetActive(true);
                    Activate_AR = true;
                    break;
                }
        }
        CMSchangeCount++;
        CMSchangeBool = false;
        SelectArray = false;
    }

    void ResetCMS()
    {
        CMS_LD_SW.SetActive(false);
        CMS_LD_TM.SetActive(false);
        CMS_RD_SW.SetActive(false);
        CMS_RD_TM.SetActive(false);
        CMSStitched.SetActive(false);
        CMS_CAM_L.SetActive(false);
        CMS_CAM_R.SetActive(false);
        CMS_CAM_S.SetActive(false);
        CAM_TM_L.SetActive(false);
        CAM_TM_R.SetActive(false);
        TraditionalMirrorLeft.SetActive(false);
        TraditionalMirrorRight.SetActive(false);
    }

    private void ApplyArray()
    {
        CMScombination_Array = new int[,] 
        {{4, 3, 7, 1, 5, 6, 2},{4, 3, 2, 1, 5, 6, 7},{4, 3, 5, 6, 1, 7, 2},{4, 6, 7, 5, 3, 2, 1},{4, 6, 2, 7, 5, 1, 3},{4, 2, 3, 7, 5, 1, 6},{4, 2, 6, 3, 5, 7, 1},{4, 5, 3, 2, 1, 6, 7},
{4, 5, 2, 1, 3, 7, 6},{4, 1, 3, 7, 6, 2, 5},{4, 1, 2, 3, 5, 7, 6},{4, 1, 5, 2, 3, 6, 7},{3, 4, 6, 7, 1, 2, 5},{3, 4, 7, 6, 1, 2, 5},{3, 4, 2, 6, 1, 7, 5},{3, 4, 2, 1, 5, 7, 6},
{3, 6, 4, 5, 1, 7, 2},{3, 6, 2, 7, 5, 4, 1},{3, 7, 4, 1, 6, 2, 5},{3, 7, 1, 5, 6, 2, 4},{3, 2, 5, 7, 1, 6, 4},{3, 5, 7, 1, 2, 6, 4},{3, 5, 2, 6, 4, 7, 1},{3, 1, 7, 6, 4, 2, 5},
{6, 4, 7, 2, 5, 3, 1},{6, 3, 4, 5, 2, 1, 7},{6, 3, 7, 1, 5, 4, 2},{6, 3, 1, 2, 7, 4, 5},{6, 7, 4, 1, 3, 5, 2},{6, 2, 7, 1, 4, 3, 5},{6, 2, 5, 4, 1, 3, 7},{6, 5, 7, 4, 1, 3, 2},
{6, 5, 7, 3, 4, 1, 2},{6, 1, 3, 7, 5, 4, 2},{6, 1, 2, 7, 3, 4, 5},{7, 4, 3, 5, 2, 1, 6},{7, 3, 5, 2, 6, 4, 1},{7, 3, 1, 4, 2, 6, 5},{7, 6, 2, 1, 5, 3, 4},{7, 2, 6, 3, 1, 4, 5},
{7, 2, 1, 3, 5, 4, 6},{7, 5, 3, 6, 2, 1, 4},{7, 5, 6, 3, 1, 2, 4},{7, 5, 1, 6, 3, 2, 4},{7, 1, 4, 2, 3, 6, 5},{7, 1, 5, 2, 4, 3, 6},{2, 4, 6, 1, 5, 3, 7},{2, 3, 7, 6, 5, 4, 1},
{2, 6, 3, 1, 5, 4, 7},{2, 6, 7, 3, 4, 5, 1},{2, 7, 3, 4, 1, 5, 6},{2, 7, 3, 5, 4, 6, 1},{2, 5, 4, 7, 3, 6, 1},{2, 5, 3, 4, 7, 6, 1},{2, 5, 1, 6, 7, 4, 3},{2, 1, 7, 5, 3, 6, 4},
{2, 1, 5, 3, 7, 4, 6},{5, 4, 7, 6, 2, 3, 1},{5, 4, 2, 3, 6, 1, 7},{5, 3, 4, 6, 1, 7, 2},{5, 3, 6, 4, 7, 1, 2},{5, 3, 7, 1, 6, 4, 2},{5, 6, 3, 4, 2, 7, 1},{5, 6, 1, 3, 2, 4, 7},
{5, 7, 2, 4, 1, 3, 6},{5, 2, 7, 1, 4, 6, 3},{5, 2, 1, 4, 3, 6, 7},{5, 1, 7, 4, 6, 3, 2},{1, 4, 6, 5, 2, 3, 7},{1, 4, 2, 3, 5, 6, 7},{1, 3, 6, 2, 7, 4, 5},{1, 3, 2, 7, 6, 4, 5},
{1, 6, 3, 2, 4, 7, 5},{1, 6, 5, 4, 3, 7, 2},{1, 7, 4, 2, 5, 3, 6},{1, 7, 5, 6, 3, 4, 2},{1, 2, 6, 5, 3, 7, 4},{1, 2, 7, 4, 5, 3, 6},{1, 5, 3, 4, 7, 6, 2},{1, 5, 7, 6, 3, 4, 2},
};

        FollowingCarSpeed = new int[,] { { -1, 1, 2, 1, 2, 1, 2}, { -1, 2, 1, 2, 1, 2, 1}, {-1, 1, 1, 2, 2, 1, 1}, {-1, 2, 2, 1, 1, 2, 2}, {-1, 2, 1, 1, 2, 1, 2},
                                            {-1, 1, 2, 2, 1, 2, 1}, {-1, 2, 1, 1, 1, 2, 1} };

        TaskScenario = new int[,] { {-1, 1, 2, 3, 1, 2, 3}, {-1, 1, 3, 2, 2, 1, 3}, {-1, 1, 3, 1, 2, 3, 2}, {-1, 1, 2, 3, 2, 3, 1}, {-1, 2, 1, 2, 3, 3, 1},
                                          {-1, 2, 3, 1, 2, 3, 1}, {-1, 2, 1, 2, 3, 1, 3} };

        LaneChangeTime = new int[,] {{ -1, 1, 0, 3, 5, 7, 9 }, { -1, 3, 7, 1, 0, 5, 9 }, {-1, 9, 5, 1, 7, 0, 3 }, {-1, 7, 5, 9, 0, 1, 3}, {-1, 5, 1, 9, 7, 0, 3},
                                        {-1, 1, 7, 0, 5, 3, 9}, {-1, 9, 3, 7, 1, 0, 5} };

        for (int i = 0; i < CMScombination.Length; i++)
            CMScombination[i] = CMScombination_Array[SampleNumber, i];

        CMSchange();
    }

    private void LogData()
    {
        LogTimer += Time.deltaTime;

        if (LogTimer <= 0.1f && (Br <= -0.7 || Mathf.Abs(SteeringWheel_Data) > 0.02f))
            ReactionNoCount = 2;

        if (LogTimer >= 0.1f && (Br <= -0.7 || Mathf.Abs(SteeringWheel_Data) > 0.02f))
            ReactionStarted = 1;

        TotalFirstReactionValue = ReactionStarted - ReactionNoCount;

        if (TotalFirstReactionValue == 1 && CSV_P.Log_FirstReactionTime == 0)
            CSV_P.Log_FirstReactionTime = LogTimer;

        if (LaneChangeComplete == 1 && CSV_P.Log_LaneChangeComplete == 0)
            CSV_P.Log_LaneChangeComplete = LogTimer;
    }

    void ResetData()
    {
        ReactionStarted = 0;
        ReactionNoCount = 0;
        TotalFirstReactionValue = 0;
        NumOfCollision = 0;
        LaneChangeComplete = 0;
        DC_C.DrivingIn2ndLane = false;
        ResetTrigger = false;
        CSV_P.Log_FirstReactionTime = 0;
        CSV_P.Log_LaneChangeComplete = 0;
        LogTimer = 0;
    }

    void Respawn()
    {
        if (waitTimer < 2)
            waitTimer += Time.deltaTime;
        if (waitTimer > 1)
        {
            totalTorque = -9000;
            DC_C.FadingEvent = true;
            DC_C.Activate_Fade = true;
            velocity.Value = 0;
            wheelTorque.Value = wheelTorqueValue;
            FixCarPos();
        }
    }

    public void FixCarPos()
    {
        VolvoCar.transform.localPosition = new Vector3(-2166, 0, 2300);
        VolvoCar.transform.rotation = Quaternion.Slerp(VolvoCar.transform.rotation, Quaternion.AngleAxis(-90, Vector3.up), 3f * Time.deltaTime);
    }
}