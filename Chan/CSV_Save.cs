using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using UnityEngine;

public class CSV_Save : MonoBehaviour
{
    [SerializeField] DemoCarController DC;
    [SerializeField] LeadingCar LC;
    [SerializeField] FollowingCar FC;

    public GameObject Volvocar, Gaze;
    public GameObject LC1, LC2, FCL1, FCL2, FCR1, FCR2;

    public bool DataLoggingStart, DataLoggingEnd, Create_CSV_File;

    string FilePath;
    string csvDirectoryName = "Data";
    string csvSeparator = ",";
    string[] csvHeaders = new string[] { "FrameNumber", "CMS_Combination", "TaskCount", "LC_LaneChangeStartingTime", "LC_StoppingTime", "FirstReactionTime", "FC_Speed", "NumberOfCollision", "DC_LaneChangeCompleteTime","Steering Wheel","Pedal", "Gaze_X", "Gaze_Y", "Gaze_Z",
                                         "DC_Pos_X","DC_Pos_Y","DC_Pos_Z", "DC_Rot_X","DC_Rot_Y","DC_Rot_Z","DC_Rot_W", "LC_Pos_X","LC_Pos_Y","LC_Pos_Z", "FCL_Pos_X","FCL_Pos_Y","FCL_Pos_Z", "FCR_Pos_X", "FCR_Pos_Y", "FCR_Pos_Z" };

    float[] SaveData = new float[30];
    float FrameNumber;


    private void Start()
    {
        string dir = Application.dataPath + "/" + csvDirectoryName;
        Directory.CreateDirectory(dir);
    }

    void FixedUpdate()
    {
        if (DataLoggingStart)
        {
            if (Create_CSV_File)
            {
                string csvFileName = "Data_SampleNumber_" + DC.SampleNumber + "_" + DC.CMScombination[DC.CMSchangeCount] + "_" + DC.taskCount + ".csv";

                FilePath = Application.dataPath + "/" + csvDirectoryName + "/" + csvFileName;

                using (StreamWriter sw = File.CreateText(FilePath))
                {
                    string finalString = "";
                    for (int i = 0; i < csvHeaders.Length; i++)
                    {
                        if (finalString != "")
                        {
                            finalString += csvSeparator;
                        }
                        finalString += csvHeaders[i];
                    }
                    finalString += csvSeparator;
                    sw.WriteLine(finalString);
                }
                Create_CSV_File = false;
                FrameNumber = 0;
            }

            FrameNumber += Time.fixedDeltaTime;

            SaveData[0] = FrameNumber; // FrameNumber
            SaveData[1] = DC.CMScombination[DC.CMSchangeCount]; // CMS_Combination
            SaveData[2] = DC.taskCount; // TaskCount
            SaveData[3] = DC.LaneChangeTime[DC.taskCount]; // LC_laneChangeStartingTime
            SaveData[4] = LC.LC_StoppingTime; // LC_StoppingTime
            SaveData[5] = DC.TotalFirstReactionValue; // FirstReactionTime
            SaveData[6] = DC.FollowingCarSpeed[DC.taskCount]; // FC_Speed
            SaveData[7] = DC.NumOfCollision;
            SaveData[8] = DC.LaneChangeComplete;
            SaveData[9] = DC.SteeringWheel_Data;
            SaveData[10] = DC.Pedal_Data;
            SaveData[11] = Gaze.transform.localPosition.x;
            SaveData[12] = Gaze.transform.localPosition.y;
            SaveData[13] = Gaze.transform.localPosition.z;
            SaveData[14] = Volvocar.transform.position.x;
            SaveData[15] = 0;
            SaveData[16] = Volvocar.transform.position.z;
            SaveData[17] = Volvocar.transform.rotation.x;
            SaveData[18] = Volvocar.transform.rotation.y;
            SaveData[19] = Volvocar.transform.rotation.z;
            SaveData[20] = Volvocar.transform.rotation.w;

            if (DC.MainTask)
            {
                if (LC.LC_Direction == 1)
                {
                    SaveData[21] = LC1.transform.position.x;
                    SaveData[22] = 0;
                    SaveData[23] = LC1.transform.position.z;
                    SaveData[24] = FCL1.transform.position.x;
                    SaveData[25] = 0;
                    SaveData[26] = FCL1.transform.position.z;
                    SaveData[27] = FCR1.transform.position.x;
                    SaveData[28] = 0;
                    SaveData[29] = FCR1.transform.position.z;
                }
                else if (LC.LC_Direction == 2)
                {
                    SaveData[21] = LC2.transform.position.x;
                    SaveData[22] = 0;
                    SaveData[23] = LC2.transform.position.z;
                    SaveData[24] = FCL2.transform.position.x;
                    SaveData[25] = 0;
                    SaveData[26] = FCL2.transform.position.z;
                    SaveData[27] = FCR2.transform.position.x;
                    SaveData[28] = 0;
                    SaveData[29] = FCR2.transform.position.z;
                }
            }
            AppendToCsv(SaveData);
        }
    }

    public void AppendToCsv(float[] data)
    {
        using (StreamWriter sw = File.AppendText(FilePath))
        {
            string csvFinalString = "";
            for (int i = 0; i < data.Length; i++)
            {
                if (csvFinalString != "")
                {
                    csvFinalString += csvSeparator;
                }
                csvFinalString += data[i];
            }
            csvFinalString += csvSeparator;
            sw.WriteLine(csvFinalString);
        }

        if(DataLoggingEnd)
        {
            DataLoggingStart = false;
            DataLoggingEnd = false;
            DC.ResetData = true;
        }
    }
}
