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

    public GameObject VolvoCar;
    public GameObject CMS_LD_SW, CMS_LD_TM, CMS_RD_SW, CMS_RD_TM, CMSCenter, CMSStitched, TraditionalMirrorLeft, TraditionalMirrorRight;
    public GameObject CSV_Manager;
    public bool Activate_AR;
    public bool ResetData;



    public bool respawnTrigger;

    public float waitTimer;
    public int taskCount, NumOfCollision;
    public int CMSchangeCount;
    public int QuestionnaireCount;
    public bool CMSchangeBool;
    public bool FinalQuestionnaireBool;

    public int laneChangeDirection;
    public bool FirstTaskCountThreshold;

    public float FC1Lposition, FC1Rposition, LC1position, FC2Lposition, FC2Rposition, LC2position, DCposition;
    public bool FCLbool, FCRbool, LCbool, TCLbool, TCRbool;
    public bool UsingAR,MainTask;
    public float Acc, Br, SteeringWheel_Data, Pedal_Data;
    //bool GameStartNoticeBool;

    int ReactionStarted, ReactionNoCount ;
    public int TotalFirstReactionValue;

    public int[] TaskScenario = new int[6];
    public int[] LaneChangeTime = new int[8]; // make the 2 dimension list by using counter-balance
    public int[] FollowingCarSpeed = new int[8];
    public int[] CMScombination = new int[7];
    public int ARSignalActivateDistance;

    float FirstReactionTimer;
    public int SampleNumber;
    public bool SampleSelection;
    public int LaneChangeComplete;

    #region Private variables not shown in the inspector
    private VolvoCars.Data.Value.Public.WheelTorque wheelTorqueValue = new VolvoCars.Data.Value.Public.WheelTorque(); // This is the value type used by the wheelTorque data item.     
    //private VolvoCars.Data.Value.Public.LampGeneral lampValue = new VolvoCars.Data.Value.Public.LampGeneral(); // This is the value type used by lights/lamps
    private float totalTorque;  // The total torque requested by the user, will be split between the four wheels
    private float steeringReduction; // Used to make it easier to drive with keyboard in higher speeds
    public const float MAX_BRAKE_TORQUE = 8000; // [Nm]
    #endregion

    private void Start()
    {
        TaskScenario = new int[] { 2, 1, 2, 1, 2, 3 };
        CMScombination = new int[] { 6, 3, 2, 5, 7, 4, 1 };
        LaneChangeTime = new int[] { 9, 5, 3, 7, 1, 0, 0, 0 };
        FollowingCarSpeed = new int[] { 0, 0, 0, 0, 1, 1, 1, 1 };
        ARSignalActivateDistance = 10;

        CMSchange();
    }

    private void Update()
    {
        if (LogitechGSDK.LogiUpdate() && LogitechGSDK.LogiIsConnected(0))
        {
            LogitechGSDK.DIJOYSTATE2ENGINES rec;
            rec = LogitechGSDK.LogiGetStateUnity(0);

            // Driving inputs 
            float rawSteeringInput = Input.GetAxis("Horizontal");
            float rawForwardInput = Input.GetAxis("Vertical");
            float parkInput = Input.GetAxis("Jump");

            Acc = (32768f - rec.lY) / 65536f;
            Br = (rec.lRz - 32768f) / 65536f;

            rawForwardInput = Acc + Br;
            Pedal_Data = rawForwardInput;

            // Steering
            steeringReduction = 1 - Mathf.Min(Mathf.Abs(velocity.Value) / 30f, 0.85f);

            userSteeringInput.Value = rawSteeringInput * steeringReduction*2f;
            SteeringWheel_Data = userSteeringInput.Value;

            // ************ORG  ***************

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
            else if (respawnTrigger == true) totalTorque = -9000;
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

            // *********** ORG ****************



            //Debug.Log(Acc + " , " +  Br);

            if (velocity.Value >= 27.5f)
            {
                totalTorque = 0;
            }
            ApplyWheelTorques(totalTorque);

            if(SampleSelection)
            {
                CSV_Manager.SetActive(true);
            }

            if(LC.LC_StoppingTime == 1)
            {
                FirstReactionTimer += Time.deltaTime;
                if (FirstReactionTimer <= 0.1f && (Br <= -0.7 || Mathf.Abs(SteeringWheel_Data) > 0.02f))
                    ReactionNoCount = 1;

                if (FirstReactionTimer >= 0.1f && (Br <= -0.7 || Mathf.Abs(SteeringWheel_Data) > 0.02f))
                    ReactionStarted = 1;
                    

                TotalFirstReactionValue = ReactionStarted - ReactionNoCount;
            }

            if (respawnTrigger)
            {
                if(waitTimer < 2)
                    waitTimer += Time.deltaTime;
                if (waitTimer > 1.5)
                {
                    DC_C.FadingEvent = true;
                    DC_C.Activate_Fade = true;
                    velocity.Value = 0;
                    wheelTorque.Value = wheelTorqueValue;
                    VolvoCar.transform.localPosition = new Vector3(-2166, 0, 2300);
                    VolvoCar.transform.rotation = Quaternion.Slerp(VolvoCar.transform.rotation, Quaternion.AngleAxis(-90, Vector3.up), 3f * Time.deltaTime);
                }
            }

            if(ResetData)
            {
                Debug.Log("Reset");
                ReactionStarted = 0;
                ReactionNoCount = 0;
                TotalFirstReactionValue = 0;
                NumOfCollision = 0;
                LaneChangeComplete = 0;
                ResetData = false;
            }

            if (CMSchangeBool)
            {
                CMSchange();
            }
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
        int[] CMScombination = { 3, 6, 2, 5, 7, 4, 1 };

        CMSCenter.SetActive(false);
        CMS_LD_SW.SetActive(false);
        CMS_LD_TM.SetActive(false);
        CMS_RD_SW.SetActive(false);
        CMS_RD_TM.SetActive(false);
        CMSStitched.SetActive(false);
        TraditionalMirrorLeft.SetActive(false);
        TraditionalMirrorRight.SetActive(false);

        switch (CMScombination[CMSchangeCount])
        {
            case 1: // Traditional Mirror
                {
                    TraditionalMirrorLeft.SetActive(true);
                    TraditionalMirrorRight.SetActive(true);
                    Activate_AR = false;
                    break;
                }
            case 2: // CMS under A-pillar
                {
                    CMSCenter.SetActive(true);
                    CMS_LD_TM.SetActive(true);
                    CMS_RD_TM.SetActive(true);
                    Activate_AR = false;
                    break;
                }
            case 3: // CMS near the Steering Wheel
                {
                    CMSCenter.SetActive(true);
                    CMS_LD_SW.SetActive(true);
                    CMS_RD_SW.SetActive(true);
                    Activate_AR = false;
                    break;
                }
            case 4: // CMS Stitched
                {
                    CMSStitched.SetActive(true);
                    Activate_AR = false;
                    break;
                }
            case 5: // CMS under A-pillar with AR signal
                {
                    CMSCenter.SetActive(true);
                    CMS_LD_TM.SetActive(true);
                    CMS_RD_TM.SetActive(true);
                    Activate_AR = true;
                    break;
                }
            case 6: // CMS near the Steering Wheel with AR signal
                {
                    CMSCenter.SetActive(true);
                    CMS_LD_SW.SetActive(true);
                    CMS_RD_SW.SetActive(true);
                    Activate_AR = true;
                    break;
                }
            case 7: // CMS Stitched with AR signal
                {
                    CMSStitched.SetActive(true);
                    Activate_AR = true;
                    break;
                }
        }
        CMSchangeCount++;
        CMSchangeBool = false;
    }
}
