using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using VolvoCars.Data;

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
    [SerializeField] private VolvoCars.Data.DoorIsOpenR1L doorIsOpenR1L = default; // R1L stands for Row 1 Left.
    [SerializeField] private VolvoCars.Data.LampBrake lampBrake = default;

    public bool respawnTrigger = false;
    public GameObject VolvoCar;
    public GameObject Questionnaire;
    public float waitTimer;
    public int taskCount;
    public int CMSchangeCount;
    public int QuestionnaireCount;
    public bool CMSchangeBool = false;
    public bool FinalQuestionnaireBool;
    public bool TrialBool = true;
    public GameObject CMS_LD_SW, CMS_LD_TM, CMS_RD_SW, CMS_RD_TM, CMSCenter, CMSStitched, ARSignalLeft, ARSignalRight, ARSignalRear, ARSignalStitched, TraditionalMirrorLeft, TraditionalMirrorRight;
    public int[] LaneChangeTime = { 0, 0, 0, 1, 3, 5, 7, 9 };
    public int[] CMScombination = { 1, 2, 3, 4, 5, 6, 7 };
    public int[] FollowingCarSpeed = { 0, 0, 0, 0, 1, 1, 1, 1 };
    public int laneChangeDirection;
    public bool threshold = false;
    public bool noticeBool = true;
    public float TrialTimeCount;

    #region Private variables not shown in the inspector
    private VolvoCars.Data.Value.Public.WheelTorque wheelTorqueValue = new VolvoCars.Data.Value.Public.WheelTorque(); // This is the value type used by the wheelTorque data item.     
    private VolvoCars.Data.Value.Public.LampGeneral lampValue = new VolvoCars.Data.Value.Public.LampGeneral(); // This is the value type used by lights/lamps
    private float totalTorque;  // The total torque requested by the user, will be split between the four wheels
    private float steeringReduction; // Used to make it easier to drive with keyboard in higher speeds
    public const float MAX_BRAKE_TORQUE = 8000; // [Nm]
    #endregion

    private void Start()
    {
        LaneChangeTime = ShuffleArray(LaneChangeTime);
        CMScombination = ShuffleArray(CMScombination);
        FollowingCarSpeed = ShuffleArray(FollowingCarSpeed);
        CMSchange();
    }

    private void Update()
    {
        // Driving inputs 
        float rawSteeringInput = Input.GetAxis("Horizontal");
        float rawForwardInput = Input.GetAxis("Vertical");
        float parkInput = Input.GetAxis("Jump");

        // Steering
        steeringReduction = 1 - Mathf.Min(Mathf.Abs(velocity.Value) / 30f, 0.85f);
        userSteeringInput.Value = rawSteeringInput * steeringReduction;

        #region Wheel torques 

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
                if(velocity.Value >= 27.5)
                {
                    totalTorque = 0;
                }
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

        ApplyWheelTorques(totalTorque);
        #endregion


        if (respawnTrigger)
        {
            waitTimer += Time.deltaTime;
            if (waitTimer > 1)
            {
                velocity.Value = 0;
                wheelTorque.Value = wheelTorqueValue;
                VolvoCar.transform.localPosition = new Vector3(0, 0, 0);
                VolvoCar.transform.rotation = Quaternion.Slerp(VolvoCar.transform.rotation, transform.rotation, 0.5f * Time.deltaTime);
            }
        }

        if (CMSchangeBool)
        {
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
        taskCount = 0;
        CMSCenter.SetActive(false);
        CMS_LD_SW.SetActive(false);
        CMS_LD_TM.SetActive(false);
        CMS_RD_SW.SetActive(false);
        CMS_RD_TM.SetActive(false);
        CMSStitched.SetActive(false);
        TraditionalMirrorLeft.SetActive(false);
        TraditionalMirrorRight.SetActive(false);
        // AR signal setactive false, stitched false

        switch (CMScombination[CMSchangeCount])
        {
            case 1: // Traditional Mirror
                {
                    TraditionalMirrorLeft.SetActive(true);
                    TraditionalMirrorRight.SetActive(true);
                    Debug.Log("Case : " + 1);
                    break;

                }
            case 2: // CMS beside Traditional Mirror
                {
                    CMSCenter.SetActive(true);
                    CMS_LD_TM.SetActive(true);
                    CMS_RD_TM.SetActive(true);
                    //ARSignalLeft.SetActive(false);
                    //ARSignalRight.SetActive(false);
                    //ARSignalRear.SetActive(false);
                    Debug.Log("Case : " + 2);
                    break;
                }
            case 3: // CMS near the Steering Wheel
                {
                    CMSCenter.SetActive(true);
                    CMS_LD_SW.SetActive(true);
                    CMS_RD_SW.SetActive(true);
                    //ARSignalLeft.SetActive(false);
                    //ARSignalRight.SetActive(false);
                    //ARSignalRear.SetActive(false);
                    Debug.Log("Case : " + 3);
                    break;
                }
            case 4: // CMS Stitched
                {
                    CMSStitched.SetActive(true);
                    //ARSignalStitched.SetActive(false);
                    Debug.Log("Case : " + 7);
                    break;
                }
            case 5: // CMS beside Traditional Mirror with AR signal
                {
                    CMSCenter.SetActive(true);
                    CMS_LD_TM.SetActive(true);
                    CMS_RD_TM.SetActive(true);
                    Debug.Log("Case : " + 4);
                    break;
                }
            case 6: // CMS near the Steering Wheel with AR signal
                {
                    CMSCenter.SetActive(true);
                    CMS_LD_SW.SetActive(true);
                    CMS_RD_SW.SetActive(true);
                    Debug.Log("Case : " + 5);
                    break;
                }
            case 7: // CMS Stitched with AR signal
                {
                    CMSStitched.SetActive(true);
                    Debug.Log("Case : " + 6);
                    break;
                }
        }
        CMSchangeCount++;
        CMSchangeBool = false;
    }

    static public T[] ShuffleArray<T>(T[] array)
    {
        int random1, random2;
        T temp;

        for (int i = 0; i < array.Length; ++i)
        {

            random1 = UnityEngine.Random.Range(0, array.Length);
            random2 = UnityEngine.Random.Range(0, array.Length);

            temp = array[random1];
            array[random1] = array[random2];
            array[random2] = temp;
        }

        return array;
    }

}
