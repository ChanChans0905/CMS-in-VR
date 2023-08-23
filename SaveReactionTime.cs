using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveReactionTime : MonoBehaviour
{
    [SerializeField] TimeLogger TimeLogger;
    [SerializeField] DemoCarController DriverCar;
    [SerializeField] CSV_Save CSV;

    private string csvFileName;
    private string[] csvHeaders = new string[6] { "Time", "Steering Wheel", "Acc", "Br", "ReactionStarted", "EventStartTime" };
    bool ChangeName;
    float ReactionStarted;

    private void Start()
    {
        csvFileName = "ReactionTime" + "_" + DriverCar.CMScombination[DriverCar.CMSchangeCount] + "_" + DriverCar.taskCount + ".csv";
    }

    void FixedUpdate()
    {
        if (TimeLogger.EventBool)
        {
            if(ChangeName)
            {
                csvFileName = "ReactionTime" + "_" + DriverCar.CMScombination[DriverCar.CMSchangeCount] + "_" + DriverCar.taskCount + ".csv";
                ChangeName = false;
            }

            float[] ReactionTime = new float[6];

            ReactionTime[0] = TimeLogger.TimeNumber;
            ReactionTime[1] = DriverCar.SteeringInput;
            ReactionTime[2] = DriverCar.Acc;
            ReactionTime[3] = DriverCar.Br;
            ReactionTime[4] = ReactionStarted;
            ReactionTime[5] = DriverCar.LaneChangeTime[DriverCar.taskCount];

            if ((DriverCar.Br <= -0.7 || Mathf.Abs(DriverCar.SteeringInput) > 0.02f))
                ReactionStarted = 1;

            CSV.AppendToCsv(ReactionTime, csvFileName, csvHeaders);

        }
        else
        {
            ReactionStarted = 0;
            ChangeName = true;
        }
            
    }
}
